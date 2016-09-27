//
// TcpipAgentTester.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Server;
using AIWolf.Server.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AIWolf.TcpipAgentTester
{
    /// <summary>
    /// TCP/IP版エージェントテスター
    /// </summary>
    class TcpipAgentTester
    {
        // クライアントが終了したことを通知する
        static AutoResetEvent are = new AutoResetEvent(false);

        static void Usage()
        {
            Console.Error.WriteLine("Usage:" + typeof(TcpipAgentTester) + " [-c clientClass dllName ] [-h host] [-p port]");
            Environment.Exit(0);
        }

        static Type GetPlayerType(string className, string dllName)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(dllName);
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("Can not find {0}.", dllName);
                Environment.Exit(0);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error in loading {0}.", dllName);
                Environment.Exit(0);
            }
            Type playerType = assembly.GetType(className);
            if (playerType == null)
            {
                Console.Error.WriteLine("Can not get Type of {0} from {1}.", className, dllName);
                Environment.Exit(0);
            }
            return playerType;
        }

        static void Main(string[] args)
        {
            int port = 10000;
            string host = null;
            Type playerType = null;
            int playerNum = 15;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    try
                    {
                        if (args[i].Equals("-p"))
                        {
                            i++;
                            port = int.Parse(args[i]);
                            if (port < 0)
                            {
                                Usage();
                            }
                        }
                        else if (args[i].Equals("-h"))
                        {
                            i++;
                            host = args[i];
                        }
                        else if (args[i].Equals("-c"))
                        {
                            i++;
                            string className = args[i];
                            i++;
                            string dllName = args[i];
                            playerType = GetPlayerType(className, dllName);
                        }
                        else
                        {
                            Usage();
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Usage();
                    }
                }
            }

            if (host == null) // スタンドアロンモード（サーバーモード）
            {
                Console.WriteLine("Start AIWolf Server port:{0} playerNum:{1}", port, playerNum);
                GameSetting gameSetting = GameSetting.GetDefaultGame(playerNum);
                TcpipServer gameServer = new TcpipServer(port, playerNum, gameSetting);

                Task task = Task.Run(() =>
                {
                    gameServer.WaitForConnection();
                });

                List<TcpipClient> randomPlayerList = new List<TcpipClient>();
                for (int i = 0; i < playerNum - 1; i++)
                {
                    TcpipClient client = new TcpipClient("localhost", port, null);
                    randomPlayerList.Add(client);
                    client.Connect(new RandomPlayer());
                }
                if (playerType != null)
                {
                    IPlayer player = null;
                    try
                    {
                        player = (IPlayer)Activator.CreateInstance(playerType);
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Error in creating instance of {0}.", args[1]);
                        Environment.Exit(0);
                    }
                    TcpipClient client = new TcpipClient("localhost", port, null);
                    client.Connect(player);
                }

                task.Wait();

                foreach (Role requestRole in Enum.GetValues(typeof(Role)))
                {
                    if (requestRole == Role.FREEMASON)
                    {
                        continue;
                    }

                    IEnumerator<TcpipClient> ie = randomPlayerList.GetEnumerator();
                    foreach (Role role in Enum.GetValues(typeof(Role)))
                    {
                        if (role == Role.FREEMASON)
                        {
                            continue;
                        }
                        int i = 0;
                        if (role == requestRole)
                        {
                            i = 1;
                        }
                        while (i < gameSetting.RoleNumMap[role])
                        {
                            if (ie.MoveNext())
                            {
                                ie.Current.RequestRole = role;
                            }
                            i++;
                        }
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        new AIWolfGame(gameSetting, gameServer).Start();
                    }
                }
                gameServer.Close();
            }
            else // クライアントモード
            {
                if (playerType == null) // クライアントモードならクラス指定必須
                {
                    Console.Error.WriteLine("Player class must be specified in client mode.");
                    Usage();
                }

                IPlayer player = null;
                try
                {
                    player = (IPlayer)Activator.CreateInstance(playerType);
                }
                catch (Exception)
                {
                    Console.Error.WriteLine("Error in creating instance of {0}.", args[1]);
                    Environment.Exit(0);
                }
                TcpipClient client = new TcpipClient(host, port, null);
                client.Completed += Client_Completed;
                client.Connect(player);
                are.WaitOne();
            }
        }

        private static void Client_Completed(object sender, EventArgs e)
        {
            are.Set();
        }
    }
}