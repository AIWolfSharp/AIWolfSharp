//
// WrapPythonPlayer.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using AIWolf.Common.Net;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace AIWolf.PythonPlayer
{
    public class WrapPythonPlayer : IPlayer
    {
        dynamic player;

        public WrapPythonPlayer()
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptSource source = engine.CreateScriptSourceFromFile("PythonPlayer.py");
            CompiledCode code = source.Compile();
            ScriptScope scope = engine.CreateScope();
            code.Execute(scope);
            player = engine.Execute("SimplePlayer()", scope);
        }

        public string Name
        {
            get
            {
                return player.get_Name();
            }
        }

        public Agent Attack()
        {
            return player.Attack();
        }

        public void DayStart()
        {
            player.DayStart();
        }

        public Agent Divine()
        {
            return player.Divine();
        }

        public void Finish()
        {
            player.Finish();
        }

        public Agent Guard()
        {
            return player.Guard();
        }

        public void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            player.Initialize(gameInfo, gameSetting);
        }

        public string Talk()
        {
            return player.Talk();
        }

        public void Update(GameInfo gameInfo)
        {
            player.Update(gameInfo);
        }

        public Agent Vote()
        {
            return player.Vote();
        }

        public string Whisper()
        {
            return player.Whisper();
        }
    }
}
