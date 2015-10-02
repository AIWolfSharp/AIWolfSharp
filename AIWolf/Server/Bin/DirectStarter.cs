using AIWolf.Common.Bin;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using AIWolf.Server.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AIWolf.Server.Bin
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
                        int num = int.Parse(args[i]);
                        clsCountMap[clsName] = num;
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
                Console.Error.WriteLine("Usage:" + typeof(ClientStarter) + " -c clientClass num [-c clientClass num ...] [-l logDir]");
                return;
            }

            Start(clsCountMap, logDir);
        }

        public static void Start(Dictionary<string, int> clsCountMap, string logDir)
        {
            int playerNum = clsCountMap.Values.Sum();
            List<IPlayer> playerList = new List<IPlayer>();
            foreach (string clsName in clsCountMap.Keys)
            {
                int num = clsCountMap[clsName];
                for (int i = 0; i < num; i++)
                {
                    playerList.Add((IPlayer)Activator.CreateInstance(Type.GetType(clsName)));
                }
            }

            string logFile = Path.Combine(logDir, "AIWolfGame" + CalendarTools.ToTimeString(DateTime.Now) + ".log");
            IGameServer gameServer = new DirectConnectServer(playerList);
            GameSetting gameSetting = GameSetting.GetDefaultGame(playerNum);
            AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
            //		game.setLogFile(logFile);
            game.SetRand(new Random(gameSetting.RandomSeed));
            //		game.init();
            game.Start();
            //		game.finish();
        }
    }
}
