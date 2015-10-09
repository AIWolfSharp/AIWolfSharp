using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System.Collections.Generic;
using System;

namespace AIWolf
{
    public class TestPlayer : IPlayer
    {
        static int numInstance = 0;

        Dictionary<int, GameInfo> gameInfoMap = new Dictionary<int, GameInfo>();
        int day;
        Agent me;
        Role? myRole;
        GameSetting gameSetting;
        int id;


        public TestPlayer()
        {
            id = numInstance++;
        }

        public string GetName()
        {
            return "TestPlayer" + id;
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
            gameInfoMap.Clear();
            this.gameSetting = gameSetting;
            day = gameInfo.Day;
            gameInfoMap[day] = gameInfo;
            myRole = gameInfo.Role;
            me = gameInfo.Agent;
        }

        public string Talk()
        {
            return Common.Data.Talk.OVER;
        }

        public void Update(GameInfo gameInfo)
        {
            day = gameInfo.Day;
            gameInfoMap[day] = gameInfo;
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
