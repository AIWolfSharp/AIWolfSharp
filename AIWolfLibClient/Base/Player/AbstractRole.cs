using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System.Collections.Generic;

namespace AIWolf.Client.Base.Player
{
    public abstract class AbstractRole
    {
        protected Dictionary<int, GameInfo> GameInfoMap { get; set; } = new Dictionary<int, GameInfo>();

        protected int Day { get; set; }

        protected Agent Me { get; set; }

        protected Role MyRole { get; set; }

        protected GameSetting GameSetting { get; set; }

        protected string Name
        {
            get
            {
                return MyRole.ToString() + "Player:ID=" + Me.AgentIdx;
            }
        }

        protected GameInfo LatestDayGameInfo
        {
            get
            {
                return GameInfoMap[Day];
            }
        }

        public void Update(GameInfo gameInfo)
        {
            Day = gameInfo.Day;
            GameInfoMap[Day] = gameInfo;
        }

        protected GameInfo GetGameInfo(int day)
        {
            return GameInfoMap.ContainsKey(day) ? GameInfoMap[day] : null;
        }

        public void Initialize(GameInfo gameInfo, GameSetting gameSetting)
        {
            GameInfoMap.Clear();
            GameSetting = gameSetting;
            Day = gameInfo.Day;
            GameInfoMap[Day] = gameInfo;
            MyRole = gameInfo.Role;
            Me = gameInfo.Agent;
            return;
        }

        public abstract void DayStart();

        public abstract string Talk();

        public abstract string Whisper();

        public abstract Agent Vote();

        public abstract Agent Attack();

        public abstract Agent Divine();

        public abstract Agent Guard();

        public abstract void Finish();
    }
}