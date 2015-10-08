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
    class TcpipClient : IGameClient
    {
        string host;
        int port;

        TcpClient tcpClient;

        IPlayer player;
        Role? requestRole; // Nullable

        bool isRunning;

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
                tcpClient = new TcpClient();
                tcpClient.Connect(Dns.GetHostAddresses(host), port);

                Thread th = new Thread(new ThreadStart(Run));
                th.Start();

                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
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
                        if (obj is string)
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
                tcpClient.Close();
            }
            catch (Exception)
            {
                throw new AIWolfRuntimeException();
            }
            finally
            {
                isRunning = false;
            }
        }

        public object Recieve(Packet packet)
        {
            GameInfo gameInfo = packet.GameInfo.ToGameInfo();
            GameSetting gameSetting = packet.GameSetting;

            object returnObject = null;
            switch (packet.Request)
            {
                case Request.Initialize:
                    player.Initialize(gameInfo, gameSetting);
                    break;
                case Request.DailyInitialize:
                    player.Update(gameInfo);
                    player.DayStart();
                    break;
                case Request.DailyFinish:
                    player.Update(gameInfo);
                    break;
                case Request.Name:
                    returnObject = player.Name;
                    break;
                case Request.Role:
                    if (requestRole != null)
                    {
                        returnObject = requestRole.ToString();
                    }
                    else
                    {
                        returnObject = "none";
                    }
                    break;
                case Request.Attack:
                    player.Update(gameInfo);
                    returnObject = player.Attack();
                    break;
                case Request.Talk:
                    player.Update(gameInfo);
                    returnObject = player.Talk();
                    if (returnObject == null)
                    {
                        returnObject = Talk.SKIP;
                    }
                    break;
                case Request.Whisper:
                    player.Update(gameInfo);
                    returnObject = player.Whisper();
                    if (returnObject == null)
                    {
                        returnObject = Talk.SKIP;
                    }
                    break;
                case Request.Divine:
                    player.Update(gameInfo);
                    returnObject = player.Divine();
                    break;
                case Request.Guard:
                    player.Update(gameInfo);
                    returnObject = player.Guard();
                    break;
                case Request.Vote:
                    player.Update(gameInfo);
                    returnObject = player.Vote();
                    break;
                case Request.Finish:
                    player.Update(gameInfo);
                    Finish();
                    break;
                default:
                    break;
            }
            return returnObject;
        }

        public void Finish()
        {
            isRunning = false;
            player.Finish();
        }
    }
}
