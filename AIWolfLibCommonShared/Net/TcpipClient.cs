﻿using AIWolf.Common.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Client Using TCP/IP Connection.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class TcpipClient : IGameClient
    {
        public event EventHandler Completed;
        protected virtual void OnCompleted(EventArgs e) { if (Completed != null) Completed(this, e); }

        string host;
        int port;

        Socket socket;

        IPlayer player;
        Role? requestRole;

        bool isRunning;

        GameInfo lastGameInfo;

        public TcpipClient(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public TcpipClient(string host, int port, Role? requestRole)
        {
            this.host = host;
            this.port = port;
            this.requestRole = requestRole;
        }

        public bool Connect(IPlayer player)
        {
            this.player = player;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(new DnsEndPoint(host, port));
                Task.Run(() =>
                {
                    try
                    {
                        StreamReader sr = new StreamReader(new NetworkStream(socket));
                        StreamWriter sw = new StreamWriter(new NetworkStream(socket));
                        string line;
                        isRunning = true;
                        while ((line = sr.ReadLine()) != null && isRunning)
                        {
                            Packet packet = DataConverter.GetInstance().ToPacket(line);

                            object obj = Recieve(packet);
                            if (packet.Request.HasReturn())
                            {
                                if (obj == null)
                                {
                                    sw.WriteLine();
                                }
                                else if (obj is string)
                                {
                                    sw.WriteLine(obj);
                                }
                                else
                                {
                                    sw.WriteLine(DataConverter.GetInstance().Convert(obj));
                                }
                                sw.Flush();
                            }
                        }
                        Console.WriteLine("Close connection of " + player);
                        sr.Close();
                        sw.Close();
                        socket.Close();
                        OnCompleted(EventArgs.Empty);
                    }
                    catch (Exception)
                    {
                        throw new AIWolfRuntimeException();
                    }
                    finally
                    {
                        isRunning = false;
                    }
                });
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return false;
            }
        }

        public object Recieve(Packet packet)
        {
            GameInfo gameInfo = lastGameInfo;
            GameSetting gameSetting = packet.GameSetting;

            if (packet.GameInfo != null)
            {
                gameInfo = packet.GameInfo.ToGameInfo();
                lastGameInfo = gameInfo;
            }

            if (packet.TalkHistory != null)
            {
                Talk lastTalk = null;
                if (gameInfo.TalkList != null && gameInfo.TalkList.Count != 0)
                {
                    lastTalk = gameInfo.TalkList[gameInfo.TalkList.Count - 1];
                }
                foreach (var talk in packet.TalkHistory)
                {
                    if (IsAfter(talk, lastTalk))
                    {
                        gameInfo.TalkList.Add(talk.ToTalk());
                    }
                }
            }

            if (packet.WhisperHistory != null)
            {
                Talk lastWhisper = null;
                if (gameInfo.WhisperList != null && gameInfo.WhisperList.Count != 0)
                {
                    lastWhisper = gameInfo.WhisperList[gameInfo.WhisperList.Count - 1];
                }
                foreach (var whisper in packet.WhisperHistory)
                {
                    if (IsAfter(whisper, lastWhisper))
                    {
                        gameInfo.WhisperList.Add(whisper.ToTalk());
                    }
                }
            }

            object returnObject = null;
            switch (packet.Request)
            {
                case Request.INITIALIZE:
                    player.Initialize(gameInfo, gameSetting);
                    break;
                case Request.DAILY_INITIALIZE:
                    player.Update(gameInfo);
                    player.DayStart();
                    break;
                case Request.DAILY_FINISH:
                    player.Update(gameInfo);
                    break;
                case Request.NAME:
                    returnObject = player.Name;
                    break;
                case Request.ROLE:
                    if (requestRole != null)
                    {
                        returnObject = requestRole.ToString();
                    }
                    else
                    {
                        returnObject = "none";
                    }
                    break;
                case Request.ATTACK:
                    player.Update(gameInfo);
                    returnObject = player.Attack();
                    break;
                case Request.TALK:
                    player.Update(gameInfo);
                    returnObject = player.Talk();
                    if (returnObject == null)
                    {
                        returnObject = Talk.SKIP;
                    }
                    break;
                case Request.WHISPER:
                    player.Update(gameInfo);
                    returnObject = player.Whisper();
                    if (returnObject == null)
                    {
                        returnObject = Talk.SKIP;
                    }
                    break;
                case Request.DIVINE:
                    player.Update(gameInfo);
                    returnObject = player.Divine();
                    break;
                case Request.GUARD:
                    player.Update(gameInfo);
                    returnObject = player.Guard();
                    break;
                case Request.VOTE:
                    player.Update(gameInfo);
                    returnObject = player.Vote();
                    break;
                case Request.FINISH:
                    player.Update(gameInfo);
                    Finish();
                    break;
                default:
                    break;
            }
            return returnObject;
        }

        /// <summary>
        /// Check is talk after lastTalk.
        /// <para>If it is same, return false.</para>
        /// </summary>
        /// <param name="talk"></param>
        /// <param name="lastTalk"></param>
        /// <returns></returns>
        private bool IsAfter(TalkToSend talk, Talk lastTalk)
        {
            if (lastTalk != null)
            {
                if (talk.Day < lastTalk.Day)
                {
                    return false;
                }
                if (talk.Day == lastTalk.Day && talk.Idx <= lastTalk.Idx)
                {
                    return false;
                }
            }
            return true;
        }

        public void Finish()
        {
            isRunning = false;
            player.Finish();
        }
    }
}
