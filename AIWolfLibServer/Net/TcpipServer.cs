//
// TcpipServer.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using AIWolf.Common.Net;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace AIWolf.Server.Net
{
    public class TcpipServer : IGameServer
    {
        /// <summary>
        /// Server Port.
        /// </summary>
        int port;

        /// <summary>
        /// IP Address
        /// </summary>
        IPAddress ipAddress;

        /// <summary>
        /// Connection limit.
        /// </summary>
        int limit;

        bool isWaitForClient;

        Dictionary<TcpClient, Agent> connectionAgentMap;
        Dictionary<Agent, TcpClient> agentConnectionMap;

        /// <summary>
        /// Current game data.
        /// </summary>
        GameData gameData;

        /// <summary>
        /// Game Setting.
        /// </summary>
        GameSetting gameSetting;

        public List<Agent> ConnectedAgentList
        {
            get
            {
                lock (connectionAgentMap)
                {
                    return connectionAgentMap.Values.ToList();
                }
            }
        }

        static Logger serverLogger = LogManager.GetCurrentClassLogger();

        Dictionary<Agent, string> nameMap;

        HashSet<IServerListener> serverListenerSet;

        Dictionary<Agent, int> lastTalkIdxMap;
        Dictionary<Agent, int> lastWhisperIdxMap;

        TcpListener serverSocket;

        public TcpipServer(int port, int limit, GameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
            this.port = port;
            this.limit = limit;
            ipAddress = IPAddress.Parse("127.0.0.1");
            nameMap = new Dictionary<Agent, string>();
            serverListenerSet = new HashSet<IServerListener>();

            connectionAgentMap = new Dictionary<TcpClient, Agent>();
            agentConnectionMap = new Dictionary<Agent, TcpClient>();
            lastTalkIdxMap = new Dictionary<Agent, int>();
            lastWhisperIdxMap = new Dictionary<Agent, int>();
        }

        public void WaitForConnection()
        {
            foreach (TcpClient client in connectionAgentMap.Keys)
            {
                if (client != null && client.Connected)
                {
                    client.Close();
                }
            }

            connectionAgentMap.Clear();
            agentConnectionMap.Clear();
            nameMap.Clear();

            Console.WriteLine("Waiting for connection...");
            serverSocket = new TcpListener(ipAddress, port);
            serverSocket.Start();

            isWaitForClient = true;

            while (connectionAgentMap.Count < limit && isWaitForClient)
            {
                TcpClient client = serverSocket.AcceptTcpClient();
                lock (connectionAgentMap)
                {
                    Agent agent = null;
                    for (int i = 1; i <= limit; i++)
                    {
                        if (!connectionAgentMap.ContainsValue(Agent.GetAgent(i)))
                        {
                            agent = Agent.GetAgent(i);
                            break;
                        }
                    }
                    if (agent == null)
                    {
                        throw new IllegalPlayerNumException("Fail to create agent");
                    }
                    connectionAgentMap[client] = agent;
                    agentConnectionMap[agent] = client;
                    string name = RequestName(agent);
                    nameMap[agent] = name;

                    foreach (var listener in serverListenerSet)
                    {
                        listener.Connected(client, agent, name);
                    }
                }
            }
            isWaitForClient = false;
            serverSocket.Stop();
        }

        public void StopWaitForConnection()
        {
            isWaitForClient = false;
            serverSocket.Stop();
            foreach (var client in connectionAgentMap.Keys)
            {
                client.Close();
            }
            connectionAgentMap.Clear();
            agentConnectionMap.Clear();
        }

        /// <summary>
        /// Send data to client.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="request"></param>
        public void Send(Agent agent, Request request)
        {
            try
            {
                string message;
                if (request == Common.Data.Request.DAILY_INITIALIZE || request == Common.Data.Request.INITIALIZE)
                {
                    lastTalkIdxMap.Clear();
                    lastWhisperIdxMap.Clear();
                    Packet packet = new Packet(request, gameData.GetGameInfoToSend(agent), gameSetting);
                    message = DataConverter.GetInstance().Convert(packet);
                }
                else if (request == Common.Data.Request.NAME || request == Common.Data.Request.ROLE)
                {
                    Packet packet = new Packet(request);
                    message = DataConverter.GetInstance().Convert(packet);
                }
                else if (request != Common.Data.Request.FINISH)
                {
                    List<TalkToSend> talkList = gameData.GetGameInfoToSend(agent).TalkList;
                    List<TalkToSend> whisperList = gameData.GetGameInfoToSend(agent).WhisperList;
                    talkList = Minimize(agent, talkList, lastTalkIdxMap);
                    whisperList = Minimize(agent, whisperList, lastWhisperIdxMap);

                    Packet packet = new Packet(request, talkList, whisperList);
                    message = DataConverter.GetInstance().Convert(packet);
                }
                else
                {
                    Packet packet = new Packet(request, gameData.GetFinalGameInfoToSend(agent));
                    message = DataConverter.GetInstance().Convert(packet);
                }
                serverLogger.Info("=>" + agent + ":" + message);

                TcpClient client = agentConnectionMap[agent];
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.WriteLine(message);
                sw.Flush();
            }
            catch (IOException)
            {
                // serverLogger.Fatal(e.Message);
                throw new LostClientException();
            }
        }

        /// <summary>
        /// Delete talks already sent.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="list"></param>
        /// <param name="lastIdxMap"></param>
        /// <returns></returns>
        private List<TalkToSend> Minimize(Agent agent, List<TalkToSend> list, Dictionary<Agent, int> lastIdxMap)
        {
            int lastIdx = list.Count;
            if (lastIdxMap.ContainsKey(agent) && list.Count >= lastIdxMap[agent])
            {
                list = list.GetRange(lastIdxMap[agent], lastIdx - lastIdxMap[agent]);
            }
            lastIdxMap[agent] = lastIdx;
            return list;
        }

        public Object Request(Agent agent, Request request)
        {
            try
            {
                TcpClient client = agentConnectionMap[agent];
                StreamWriter sw = new StreamWriter(client.GetStream());
                StreamReader sr = new StreamReader(client.GetStream());
                Send(agent, request);

                string line = sr.ReadLine();
                serverLogger.Info("<=" + agent + ":" + line);

                if (line.Length == 0)
                {
                    line = null;
                }
                if (request == Common.Data.Request.TALK || request == Common.Data.Request.WHISPER || request == Common.Data.Request.NAME || request == Common.Data.Request.ROLE)
                {
                    return line;
                }
                else if (request == Common.Data.Request.ATTACK || request == Common.Data.Request.DIVINE || request == Common.Data.Request.GUARD || request == Common.Data.Request.VOTE)
                {
                    return DataConverter.GetInstance().ToAgent(line);
                }
                else
                {
                    return null;
                }
            }
            catch (IOException e)
            {
                throw new LostClientException("Lost connection with " + agent, e, agent);
            }
        }

        public void Init(Agent agent)
        {
            Send(agent, Common.Data.Request.INITIALIZE);
        }

        public void DayStart(Agent agent)
        {
            Send(agent, Common.Data.Request.DAILY_INITIALIZE);
        }

        public void DayFinish(Agent agent)
        {
            Send(agent, Common.Data.Request.DAILY_FINISH);
        }

        public string RequestName(Agent agent)
        {
            return (string)Request(agent, Common.Data.Request.NAME);
        }

        public Role? RequestRequestRole(Agent agent)
        {
            string roleString = (string)Request(agent, Common.Data.Request.ROLE);
            try
            {
                return (Role?)Enum.Parse(typeof(Role), roleString);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public string RequestTalk(Agent agent)
        {
            return (string)Request(agent, Common.Data.Request.TALK);
        }

        public string RequestWhisper(Agent agent)
        {
            return (string)Request(agent, Common.Data.Request.WHISPER);
        }

        public Agent RequestVote(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.VOTE);
        }

        public Agent RequestDivineTarget(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.DIVINE);
        }

        public Agent RequestGuardTarget(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.GUARD);
        }

        public Agent RequestAttackTarget(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.ATTACK);
        }

        public void Finish(Agent agent)
        {
            Send(agent, Common.Data.Request.FINISH);
            Send(agent, Common.Data.Request.FINISH);
        }

        public void SetGameData(GameData gameData)
        {
            this.gameData = gameData;
        }

        public void SetGameSetting(GameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
        }

        public void Close()
        {
            if (serverSocket != null && serverSocket.Server.Connected)
            {
                serverSocket.Stop();
            }
            foreach (TcpClient client in connectionAgentMap.Keys)
            {
                client.Close();
            }
            connectionAgentMap.Clear();
            agentConnectionMap.Clear();
        }

        public bool AddServerListener(IServerListener listener)
        {
            return serverListenerSet.Add(listener);
        }

        public bool RemoveServerListener(IServerListener listener)
        {
            return serverListenerSet.Remove(listener);
        }
    }
}
