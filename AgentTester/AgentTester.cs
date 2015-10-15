﻿using AIWolf.Client.Base.Smpl;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Server;
using AIWolf.Server.Net;
using System;
using System.Collections.Generic;

namespace AIWolf.AgentTester
{
    /// <summary>
    /// エージェントをテストするためのクラス
    /// <para>
    /// 自作エージェントをPlayerとして指定し，ランダムプレイヤーと対戦することで 相手が予想外の行動を行った際に発生するExceptionを探すことが出来ます．
    /// </para>
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class AgentTester
    {
        static void Main(string[] args)
        {
            // ここにテストしたい自分のPlayerを指定してください．
            IPlayer player = new SampleRoleAssignPlayer();

            // ///////////////////////////////////////////
            // これ以降は変更しないでください．

            Type ptyp = player.GetType();
            for (int j = 0; j < 10; j++)
            {
                foreach (Role requestRole in Enum.GetValues(typeof(Role)))
                {
                    if (requestRole == Role.FREEMASON)
                    {
                        continue;
                    }

                    player = (IPlayer)Activator.CreateInstance(ptyp);

                    Dictionary<IPlayer, Role?> playerMap = new Dictionary<IPlayer, Role?>();
                    playerMap[player] = requestRole;
                    for (int i = 0; i < 14; i++)
                    {
                        playerMap[new RandomPlayer()] = null;
                    }

                    DirectConnectServer gameServer = new DirectConnectServer(playerMap);
                    GameSetting gameSetting = GameSetting.GetDefaultGame(playerMap.Count);
                    AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
                    game.SetRand(new Random());
                    game.Start();
                }
            }
        }
    }
}