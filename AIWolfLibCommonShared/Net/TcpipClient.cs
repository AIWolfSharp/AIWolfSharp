using AIWolf.Common.Data;
using System;
using System.IO;
#if WINDOWS_UWP
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
#else
using System.Net;
using System.Net.Sockets;
using System.Threading;
#endif

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

        IPlayer player;
        public Role? RequestRole { get; set; }

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
            this.RequestRole = requestRole;
            Running = false;
        }

#if WINDOWS_UWP
        StreamSocket socket;
#else
        TcpClient tcpClient;
#endif
#if WINDOWS_UWP
        public async Task<bool> Connect(IPlayer player)
#else
        public bool Connect(IPlayer player)
#endif
        {
            this.player = player;

            try
            {
#if WINDOWS_UWP
                socket = new StreamSocket();
                await socket.ConnectAsync(new HostName(host), port.ToString());
                Task.Run(() => { Run(); });
#else
                tcpClient = new TcpClient();
                tcpClient.Connect(Dns.GetHostAddresses(host), port);
                Thread th = new Thread(new ThreadStart(Run));
                th.Start();
#endif
                Connecting = true;
                return true;
            }
            catch (Exception e)
            {
#if !WINDOWS_UWP
                Console.Error.WriteLine(e.StackTrace);
#endif
                Connecting = false;
                return false;
            }
        }

#if WINDOWS_UWP
        string ReadLine(StreamReader sr)
        {
            StringBuilder sb = new StringBuilder();
            while (sr.Peek() > -1)
            {
                int c = sr.Read();
                if (c != '\n')
                {
                    sb.Append((char)c);
                }
                else
                {
                    return sb.ToString();
                }
            }
            throw new AIWolfRuntimeException("Socket read error in ReadLine(StreamReader)");
        }
#endif

        public void Run()
        {
            try
            {
                string line;
#if WINDOWS_UWP
                StreamReader sr = new StreamReader(socket.InputStream.AsStreamForRead(), Encoding.ASCII);
                StreamWriter sw = new StreamWriter(socket.OutputStream.AsStreamForWrite());
                while ((line = ReadLine(sr)) != null)
#else
                StreamReader sr = new StreamReader(tcpClient.GetStream());
                StreamWriter sw = new StreamWriter(tcpClient.GetStream());
                while ((line = sr.ReadLine()) != null)
#endif
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
                    //throw new AIWolfRuntimeException();
                    throw;
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
                    if (RequestRole != null)
                    {
                        returnObject = RequestRole.ToString();
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
