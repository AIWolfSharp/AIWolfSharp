using AIWolf.Common.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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

        TcpClient tcpClient;

        IPlayer player;
        Role? requestRole;

        public bool Running { get; private set; }
        public bool Connecting { get; private set; }

        GameInfo lastGameInfo;

        public TcpipClient(string host, int port)
        {
            this.host = host;
            this.port = port;
            Running = false;
        }

        public TcpipClient(string host, int port, Role? requestRole)
        {
            this.host = host;
            this.port = port;
            this.requestRole = requestRole;
            Running = false;
        }

        public bool Connect(IPlayer player)
        {
            this.player = player;

            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(Dns.GetHostAddresses(host), port);
                Connecting = true;

                Thread th = new Thread(new ThreadStart(Run));
                th.Start();

                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                Connecting = false;
                return false;
            }
        }

        public void Run()
        {
            try
            {
                // サーバと接続されたソケットを利用して処理を行う
                StreamReader sr = new StreamReader(tcpClient.GetStream());
                StreamWriter sw = new StreamWriter(tcpClient.GetStream());
                string line;
                while ((line = sr.ReadLine()) != null)
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
            }
            catch (Exception)
            {
                if (Connecting)
                {
                    Running = false;
                    Connecting = false;
                    throw new AIWolfRuntimeException();
                }
            }
            finally
            {
                OnCompleted(EventArgs.Empty);
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
                    Running = true;
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
                    if (returnObject == null)
                    {
                        returnObject = player.GetType().Name;
                    }
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
            player.Finish();
            Running = false;
        }
    }
}
