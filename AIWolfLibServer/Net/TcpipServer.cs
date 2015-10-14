using AIWolf.Common.Data;
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

        public TcpipServer(int port, int limit, GameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
            this.port = port;
            this.limit = limit;
            ipAddress = IPAddress.Parse("127.0.0.1");

            connectionAgentMap = new Dictionary<TcpClient, Agent>();
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

            //サーバソケットの作成
            Console.WriteLine("Waiting for connection...");
            TcpListener listener = new TcpListener(ipAddress, port);
            listener.Start();

            int idx = 0;
            isWaitForClient = true;
            while (connectionAgentMap.Count < limit && isWaitForClient)
            {
                TcpClient client = listener.AcceptTcpClient();
                lock (connectionAgentMap)
                {
                    Agent agent = Agent.GetAgent(idx++);
                    connectionAgentMap[client] = agent;

                    Console.WriteLine("Connect {0} ( {1}/{2} )", agent, connectionAgentMap.Count, limit);
                    serverLogger.Info("Connect {0} ( {1}/{2} )", agent, connectionAgentMap.Count, limit);
                }
            }
            agentConnectionMap = new Dictionary<Agent, TcpClient>();
            foreach (TcpClient client in connectionAgentMap.Keys)
            {
                agentConnectionMap[connectionAgentMap[client]] = client;
            }
            listener.Stop();
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
                if (request != Common.Data.Request.Finish)
                {
                    Packet packet = new Packet(request, gameData.GetGameInfoToSend(agent), gameSetting);
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
                if (request == Common.Data.Request.Talk || request == Common.Data.Request.Whisper || request == Common.Data.Request.Name || request == Common.Data.Request.Role)
                {
                    return line;
                }
                else if (request == Common.Data.Request.Attack || request == Common.Data.Request.Divine || request == Common.Data.Request.Guard || request == Common.Data.Request.Vote)
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
                throw new LostClientException("Lost connection with " + agent, e);
            }
        }

        public void Init(Agent agent)
        {
            Send(agent, Common.Data.Request.Initialize);
        }

        public void DayStart(Agent agent)
        {
            Send(agent, Common.Data.Request.DailyInitialize);
        }

        public void DayFinish(Agent agent)
        {
            Send(agent, Common.Data.Request.DailyFinish);
        }

        public string RequestName(Agent agent)
        {
            return (string)Request(agent, Common.Data.Request.Name);
        }

        public Role? RequestRequestRole(Agent agent)
        {
            string roleString = (string)Request(agent, Common.Data.Request.Role);
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
            return (string)Request(agent, Common.Data.Request.Talk);
        }

        public string RequestWhisper(Agent agent)
        {
            return (string)Request(agent, Common.Data.Request.Whisper);
        }

        public Agent RequestVote(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.Vote);
        }

        public Agent RequestDivineTarget(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.Divine);
        }

        public Agent RequestGuardTarget(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.Guard);
        }

        public Agent RequestAttackTarget(Agent agent)
        {
            return (Agent)Request(agent, Common.Data.Request.Attack);
        }

        public void Finish(Agent agent)
        {
            Send(agent, Common.Data.Request.Finish);
            Send(agent, Common.Data.Request.Finish);
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
            foreach (TcpClient client in agentConnectionMap.Values)
            {
                try
                {
                    client.Close();
                }
                catch (IOException e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                }
            }
        }
    }
}
