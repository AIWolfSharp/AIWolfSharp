using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System.Collections.Generic;

namespace AIWolf
{
    class TestPlayer : Player
    {
        private Dictionary<int, GameInfo> gameInfoMap = new Dictionary<int, GameInfo>();
        private int day;
        private Agent me;
        private Role myRole;
        private GameSetting gameSetting;

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

        public string GetName()
        {
            return "TestPlayer";
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
            gameInfoMap.Add(day, gameInfo);
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
            gameInfoMap.Add(day, gameInfo);
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
