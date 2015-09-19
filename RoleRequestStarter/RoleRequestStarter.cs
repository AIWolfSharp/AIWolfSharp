using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using AIWolf.Server;
using AIWolf.Server.Net;
using AIWolf.Server.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AIWolf.RoleRequestStarter
{
    /// <summary>
    /// 役割を指定してスタートするStarter
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class RoleRequestStarter
    {
        static readonly string description =
            "このクラスを利用して，自作Playerごとに役職を決めてゲームをスタートできます．\n" +
             "Usage:" + typeof(RoleRequestStarter) + " -n agentNum -a dllName -c playerClass role [-c playerClass role] [-d defaultPlayer]\n" +
            "-n ゲームに参加するエージェント数を決定します．\n" +
            "-a プレイヤーdllファイル名を指定します．\n" +
            "-c 'プレイヤークラス'　'設定したい役職'　で，役職を指定して利用するPlayerを設定します．\n" +
            "-c 'プレイヤークラス'　で，役職を指定せずに利用するPlayerを指定します．\n" +
            "-d デフォルトのプレイヤークラスを指定します．指定しなければSampleRoleAssignPlayerが使われます．\n" +
            "-l ログを保存するディレクトリの指定．デフォルトは./log/\n" +
            "例えば，MyPlayer.dll中の自作のAIWolf.MyPlayerをbodyguardとして12体のエージェントで人狼を実行したければ\n" +
            typeof(RoleRequestStarter) + " -n 12 -a MyPlayer.dll -c AIWolf.MyPlayer bodyguard\n" +
            "としてください．\n";

        public static void Main(string[] args)
        {
            List<KeyValuePair<string, Role?>> playerRoleList = new List<KeyValuePair<string, Role?>>();
            string defaultClsName = "AIWolf.Client.Base.Smpl.SampleRoleAssignPlayer";
            string defaultAssemblyName = "AIWolfLibClient.dll";
            string playerAssemblyName = null;
            int playerNum = -1;
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
                        try
                        {
                            if (i > args.Length - 1 || args[i].StartsWith("-"))
                            {
                                i--;
                                playerRoleList.Add(new KeyValuePair<string, Role?>(clsName, null));
                                continue;
                            }
                            Role role = (Role)Enum.Parse(typeof(Role), args[i].ToUpper());
                            playerRoleList.Add(new KeyValuePair<string, Role?>(clsName, role));
                        }
                        catch (ArgumentException)
                        {
                            Console.Error.WriteLine("No such role as " + args[i]);
                            return;
                        }
                    }
                    else if (args[i].Equals("-a"))
                    {
                        i++;
                        playerAssemblyName = args[i];
                    }
                    else if (args[i].Equals("-n"))
                    {
                        i++;
                        playerNum = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-d"))
                    {
                        i++;
                        defaultClsName = args[i];
                    }
                    else if (args[i].Equals("-l"))
                    {
                        i++;
                        logDir = args[i];
                    }
                    else if (args[i].Equals("-h"))
                    {
                        Console.WriteLine(description);
                        Console.WriteLine("利用可能な役職");
                        foreach (Role role in Enum.GetValues(typeof(Role)))
                        {
                            Console.WriteLine(role);
                        }
                    }
                }
            }
            if (playerNum < 0)
            {
                Console.Error.WriteLine("Usage:" + typeof(RoleRequestStarter) + " -a dllName -n agentNum -c playerClass role [-c playerClass role...] [-d defaultPlayer] [-l logDir]");
                Environment.Exit(0);
            }

            Assembly defaultAssesmbly = Assembly.LoadFrom(defaultAssemblyName);
            Assembly playerAssesmbly = playerAssemblyName != null ? Assembly.LoadFrom(playerAssemblyName) : null;

            Dictionary<IPlayer, Role?> playerMap = new Dictionary<IPlayer, Role?>();
            foreach (KeyValuePair<string, Role?> pair in playerRoleList)
            {
                if (playerAssesmbly == null)
                {
                    Console.Error.WriteLine("Please specify player dll file name.");
                    Environment.Exit(0);
                }
                playerMap.Add((IPlayer)Activator.CreateInstance(playerAssesmbly.GetType(pair.Key)), pair.Value);
            }
            while (playerMap.Count < playerNum)
            {
                playerMap.Add((IPlayer)Activator.CreateInstance(defaultAssesmbly.GetType(defaultClsName)), null);
            }

            Start(playerMap, logDir);
        }

        /// <summary>
        /// 一人のRoleを指定してDirectに実行
        /// </summary>
        /// <returns></returns>
        public static AIWolfGame Start(IPlayer player, Role role, int playerNum, string defaultClsName, string logDir)
        {
            Dictionary<IPlayer, Role?> playerMap = new Dictionary<IPlayer, Role?>();

            playerMap.Add(player, role);
            while (playerMap.Count < playerNum)
            {
                playerMap.Add((IPlayer)Activator.CreateInstance(Type.GetType(defaultClsName)), null);
            }
            return Start(playerMap, logDir);
        }

        /// <summary>
        /// すべてのプレイヤーインスタンスとそのRoleを設定して開始
        /// </summary>
        /// <param name="playerMap"></param>
        /// <param name="logDir"></param>
        public static AIWolfGame Start(Dictionary<IPlayer, Role?> playerMap, string logDir)
        {
            DirectConnectServer gameServer = new DirectConnectServer(playerMap);
            GameSetting gameSetting = GameSetting.GetDefaultGame(playerMap.Count);
            AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
            if (logDir != null)
            {
                string logFile = Path.Combine(logDir, "contest" + CalendarTools.ToTimeString() + ".log");
                game.setLogFile(logFile);
            }
            game.SetRand(new Random());
            //		game.setShowConsoleLog(false);
            game.Start();
            return game;
        }

        /// <summary>
        /// すべてのプレイヤーインスタンスとそのRoleを設定して開始
        /// </summary>
        /// <param name="playerMap"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static AIWolfGame Start(Dictionary<IPlayer, Role?> playerMap, IGameLogger logger)
        {
            DirectConnectServer gameServer = new DirectConnectServer(playerMap);
            GameSetting gameSetting = GameSetting.GetDefaultGame(playerMap.Count);
            AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
            game.GameLogger = logger;
            game.SetRand(new Random());
            //		game.setShowConsoleLog(false);
            game.Start();
            return game;
        }

    }
}
