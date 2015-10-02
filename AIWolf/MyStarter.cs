using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Server;
using AIWolf.Server.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AIWolf
{
    class MyStarter
    {
        static int port = 10000;
        static int playerNum = 12;
        static List<String> clsNameList = new List<String>();
        static List<String> roleRequestList = new List<String>();

        public static void Usage()
        {
            Console.Error.WriteLine("Usage:" + typeof(MyStarter) + " -p port -n playerNum -c clientClass [requestRole]");
        }

        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i].Equals("-p"))
                    {
                        i++;
                        port = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-n"))
                    {
                        i++;
                        playerNum = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-c"))
                    {
                        i++;
                        clsNameList.Add(args[i]);
                        i++;
                        if (i > args.Length - 1 || args[i].StartsWith("-"))
                        {
                            i--;
                            roleRequestList.Add("none");
                            continue;
                        }
                        roleRequestList.Add(args[i]);
                    }
                }
            }
            if (port < 0)
            {
                Usage();
                Environment.Exit(0);
            }

            Thread th = new Thread(new ThreadStart(Run));
            th.Start();

            for (int i = 0; i < clsNameList.Count; i++)
            {
                IPlayer player = null;
                try
                {
                    player = (IPlayer)Activator.CreateInstance(Type.GetType(clsNameList[i]));
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.StackTrace);
                }
                Role? roleRequest;
                if (roleRequestList[i].Equals("none"))
                {
                    roleRequest = null;
                }
                else
                {
                    roleRequest = (Role?)Enum.Parse(typeof(Role), roleRequestList[i]);
                }
                TcpipClient client = new TcpipClient("localhost", port, roleRequest);
                if (client.Connect(player))
                {
                    Console.WriteLine("Player connected to server:" + player);
                }
            }
        }

        private static void Run()
        {
            Console.WriteLine("Start AIWolf Server port:{0} playerNum:{1}", port, playerNum);
            GameSetting gameSetting = GameSetting.GetDefaultGame(playerNum);
            TcpipServer gameServer = new TcpipServer(port, playerNum, gameSetting);
            try
            {
                gameServer.WaitForConnection();
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
            AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
            game.SetRand(new Random());
            game.Start();
        }
    }
}
