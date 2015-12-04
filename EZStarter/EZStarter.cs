using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Server;
using AIWolf.Server.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace EZStarter
{
    class EZStarter
    {
        static int port = 10000;
        static int playerNum = 12;

        static void Usage()
        {
            Console.WriteLine("Usage:" + typeof(EZStarter) + " -p port -n playerNum -c clientClass clientDllName [requestRole]");
        }

        static void Main(string[] args)
        {
            List<string> clsNameList = new List<string>();
            List<string> dllNameList = new List<string>();
            List<Role?> roleRequestList = new List<Role?>();

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
                        dllNameList.Add(args[i]);
                        i++;
                        try
                        {
                            if (i > args.Length - 1 || args[i].StartsWith("-")) // Roleでない
                            {
                                i--;
                                roleRequestList.Add(null);
                                continue;
                            }
                            roleRequestList.Add((Role)Enum.Parse(typeof(Role), args[i]));
                        }
                        catch (ArgumentException)
                        {
                            Console.Error.WriteLine("No such role as " + args[i]);
                            return;
                        }
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
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(dllNameList[i]);
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine("Can not find " + dllNameList[i]);
                    Usage();
                    Environment.Exit(-1);
                }
                catch (Exception)
                {
                    Console.Error.WriteLine("Error in loading " + dllNameList[i]);
                    Usage();
                    Environment.Exit(-1);
                }

                IPlayer player = null;
                try
                {
                    player = (IPlayer)Activator.CreateInstance(assembly.GetType(clsNameList[i]));
                }
                catch (Exception)
                {
                    Console.Error.WriteLine("Error in creating instance of " + clsNameList[i]);
                    Usage();
                    Environment.Exit(-1);
                }
                TcpipClient client = new TcpipClient("localhost", port, roleRequestList[i]);
                if (client.Connect(player))
                {
                    Console.WriteLine("Player connected to server:" + player);
                }
            }
        }

        static void Run()
        {
            Console.WriteLine("Start AIWolf Server port:{0} playerNum:{1}", port, playerNum);
            GameSetting gameSetting = GameSetting.GetDefaultGame(playerNum);
            TcpipServer gameServer = new TcpipServer(port, playerNum, gameSetting);
            gameServer.WaitForConnection();

            AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
            game.SetRand(new Random());
            game.Start();
        }
    }
}
