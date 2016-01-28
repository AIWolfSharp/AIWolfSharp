﻿using AIWolf.Common.Data;
using AIWolf.Common.Net;

namespace AIWolf.CSPlayer
{
    public class SimplePlayer : IPlayer
    {
        Agent me;

        public string Name
        {
            get
            {
                return GetType().ToString();
            }
        }

        public Agent Attack()
        {
            return me;
        }

        public void DayStart()
        {
        }

        public Agent Divine()
        {
            return me;
        }

        public void Finish()
        {
        }

        public Agent Guard()
        {
            return me;
        }

        public void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            me = gameInfo.Agent;
        }

        public string Talk()
        {
            return Common.Data.Talk.OVER;
        }

        public void Update(GameInfo gameInfo)
        {
        }

        public Agent Vote()
        {
            return me;
        }

        public string Whisper()
        {
            return Common.Data.Talk.OVER;
        }
    }
}