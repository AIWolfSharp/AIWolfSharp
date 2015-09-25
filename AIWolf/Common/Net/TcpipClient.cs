using AIWolf.Common.Data;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Client Using TCP/IP Connection
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class TcpipClient : GameClient
    {
        public string Host { get; set; }
        public int Port { get; set; }

        private TcpClient tcpClient;

        private Player player;

        public Role RequestRole { get; set; }

        private bool isRunning;

        public TcpipClient(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public TcpipClient(string host, int port, Role requestRole)
        {
            Host = host;
            Port = port;
            RequestRole = requestRole;
        }

        public bool Connect(Player player)
        {
            this.player = player;

            tcpClient = new TcpClient();
            tcpClient.Connect(Dns.GetHostAddresses(Host), Port);

            Thread th = new Thread(new ThreadStart(Run));

            return true;
        }

        private void Run()
        {
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
            Console.WriteLine("Close connection of" + player);
            sr.Close();
            sw.Close();
            tcpClient.Close();
            isRunning = false;
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
                    returnObject = player.GetName();
                    break;
                case Request.Role:
                    if (RequestRole != null)
                    {
                        returnObject = RequestRole.ToString();
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
                    finish();
                    break;
                default:
                    break;
            }
            return returnObject;
        }

        private void finish()
        {
            isRunning = false;
            player.Finish();
        }
    }
}
