using AIWolf.Common.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        const int bufferSize = 8192;

        public event EventHandler Completed;
        protected virtual void OnCompleted(EventArgs e) { if (Completed != null) Completed(this, e); }

        byte[] recvBuffer = new byte[bufferSize];

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
                var connectEventArgs = new SocketAsyncEventArgs();
                connectEventArgs.RemoteEndPoint = new DnsEndPoint(host, port);
                connectEventArgs.Completed += ConnectCompleted;

                if (!socket.ConnectAsync(connectEventArgs))
                {
                    if (connectEventArgs.SocketError != SocketError.Success)
                    {
                        return false;
                    }
                    else
                    {
                        Task.Run(() => { ProcessIO(connectEventArgs); });
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return false;
            }
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessIO(e);
        }

        private void ProcessIO(SocketAsyncEventArgs e)
        {
            if (e.ConnectSocket == null)
            {
                Console.Error.WriteLine("Connection Failed");
                Environment.Exit(-1);
            }
            var ioEventArgs = new SocketAsyncEventArgs();
            ioEventArgs.Completed += IOCompleted;
            ioEventArgs.SetBuffer(recvBuffer, 0, bufferSize);
            if (!socket.ReceiveAsync(ioEventArgs))
            {
                ProcessReceive(ioEventArgs);
            }
        }

        private void IOCompleted(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                MemoryStream ms = new MemoryStream(bufferSize);
                StreamReader sr = new StreamReader(new MemoryStream(e.Buffer, e.Offset, e.BytesTransferred));
                string line = sr.ReadLine();
                //sr.Dispose();
                Packet packet = DataConverter.GetInstance().ToPacket(line);
                object obj = Recieve(packet);
                string s;
                if (packet.Request.HasReturn())
                {
                    if (obj == null)
                    {
                        s = "\n";
                    }
                    else if (obj is string)
                    {
                        s = obj + "\n";
                    }
                    else
                    {
                        s = DataConverter.GetInstance().Convert(obj) + "\n";
                    }
                    byte[] byteArray = Encoding.ASCII.GetBytes(s);
                    e.SetBuffer(byteArray, 0, byteArray.Length);
                    if (!socket.SendAsync(e))
                    {
                        ProcessSend(e);
                    }
                }
                else
                {
                    e.SetBuffer(recvBuffer, 0, bufferSize);
                    if (!socket.ReceiveAsync(e))
                    {
                        ProcessReceive(e);
                    }
                }
            }
            else
            {
                CloseSocket(e);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                e.SetBuffer(recvBuffer, 0, bufferSize);
                if (!socket.ReceiveAsync(e))
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                CloseSocket(e);
            }
        }

        private void CloseSocket(SocketAsyncEventArgs e)
        {
            try
            {
                Console.WriteLine("Close connection of " + player);
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
                throw new AIWolfRuntimeException();
            }
            finally
            {
                socket.Dispose();
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
