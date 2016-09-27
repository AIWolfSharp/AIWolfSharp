//
// ServerStarter.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Net;
using AIWolf.Server;
using AIWolf.Server.Net;
using System;

namespace AIWolf.ServerStarter
{
    /// <summary>
    /// Main class to start server application.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class ServerStarter
    {
        public static void Main(string[] args)
        {
            int port = 10000;
            int playerNum = 12;

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
                }
            }

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
