//
// DirectStarter.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using AIWolf.Server;
using AIWolf.Server.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AIWolf.DirectStarter
{
    /// <summary>
    /// クライアントを指定して直接シミュレーションを実行する
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class DirectStarter
    {
        public static void Main(string[] args)
        {
            Dictionary<string, int> clsCountMap = new Dictionary<string, int>();
            Dictionary<string, string> clsDllNameMap = new Dictionary<string, string>();
            string logDir = "./log/";
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i].Equals("-c"))
                    {
                        i++;
                        string clsName = args[i];
                        i++;
                        clsDllNameMap[clsName] = args[i];
                        i++;
                        clsCountMap[clsName] = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-l"))
                    {
                        i++;
                        logDir = args[i];
                    }
                }
            }
            if (clsCountMap.Count == 0)
            {
                Console.Error.WriteLine("Usage:" + typeof(DirectStarter) + " -c clientClass dllName num [-c clientClass dllName num ...] [-l logDir]");
                return;
            }

            Start(clsCountMap, clsDllNameMap, logDir);
        }

        public static void Start(Dictionary<string, int> clsCountMap, Dictionary<string, string> clsDllNameMap, string logDir)
        {
            int playerNum = clsCountMap.Values.Sum();
            List<IPlayer> playerList = new List<IPlayer>();
            foreach (string clsName in clsCountMap.Keys)
            {
                int num = clsCountMap[clsName];
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(clsDllNameMap[clsName]);
                }
                catch (FileNotFoundException)
                {
                    Console.Error.WriteLine("Can not find " + clsDllNameMap[clsName]);
                    Environment.Exit(0);
                }
                catch (Exception)
                {
                    Console.Error.WriteLine("Error in loading " + clsDllNameMap[clsName]);
                    Environment.Exit(0);
                }

                for (int i = 0; i < num; i++)
                {
                    IPlayer player = null;
                    try
                    {
                        player = (IPlayer)Activator.CreateInstance(assembly.GetType(clsName));
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Error in creating instance of " + clsName);
                        Environment.Exit(0);
                    }
                    playerList.Add(player);
                }
            }

            string logFile = Path.Combine(logDir, "aiwolfGame" + CalendarTools.ToTimeString() + ".log");
            IGameServer gameServer = new DirectConnectServer(playerList);
            GameSetting gameSetting = GameSetting.GetDefaultGame(playerNum);
            AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
            game.SetRand(new Random((int)gameSetting.RandomSeed));
            game.Start();
        }
    }
}
