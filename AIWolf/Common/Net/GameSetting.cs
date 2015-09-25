using AIWolf.Common.Data;
using System;
using System.Collections.Generic;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Settings of game
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class GameSetting : ICloneable
    {
        /// <summary>
        /// Num of each roles.
        /// <para>
        /// Bodyguard, FreeMason, Medium, Possessed, Seer, Villager, Werewolf
        /// </para>
        /// </summary>
        private static readonly int[][] roleNumArray =
        {
            null,//0
            null,//1
            null,//2
            null,//3
            new[] {0, 0, 0, 0, 1, 2, 1 },//4
            new[] {0, 0, 0, 1, 1, 2, 1 },//5
            new[] {0, 0, 0, 1, 1, 3, 1 },//6
            new[] {0, 0, 0, 0, 1, 4, 2 },//7
            new[] {0, 0, 1, 0, 1, 4, 2 },//8
            new[] {0, 0, 1, 0, 1, 5, 2 },//9
            new[] {1, 0, 1, 1, 1, 4, 2 },//10
            new[] {1, 0, 1, 1, 1, 5, 2 },//11
            new[] {1, 0, 1, 1, 1, 5, 3 },//12
            new[] {1, 0, 1, 1, 1, 6, 3 },//13
            new[] {1, 0, 1, 1, 1, 7, 3 },//14
            new[] {1, 0, 1, 1, 1, 8, 3 },//15
            new[] {1, 0, 1, 1, 1, 9, 3 },//16
            new[] {1, 0, 1, 1, 1, 10, 3 },//17
            new[] {1, 0, 1, 1, 1, 11, 3 },//18
        };

        /// <summary>
        /// セミナー用セット
        /// <para>
        /// 村人8， 人狼3， 占い，狩人
        /// </para>
        /// </summary>
        private static readonly int[] seminarArray =
        {
            1, 0, 0, 0, 1, 8, 3 //13
        };

        public static GameSetting GetDefaultGame(int agentNum)
        {
            if (agentNum < 5)
            {
                throw new ArgumentOutOfRangeException("agentNum must be bigger than 5 but " + agentNum);
            }
            if (agentNum > roleNumArray.Length)
            {
                throw new ArgumentOutOfRangeException("agentNum must be smaller than " + (roleNumArray.Length + 1) + " but " + agentNum);
            }

            GameSetting setting = new GameSetting();
            setting.MaxTalk = 10;
            setting.IsEnableNoAttack = false;
            setting.IsVoteVisible = true;
            setting.IsVotableInFirstDay = false;

            Role[] roles = (Role[])Enum.GetValues(typeof(Enum));
            for (int i = 0; i < roles.Length; i++)
            {
                setting.RoleNumMap.Add(roles[i], roleNumArray[agentNum][i]);
            }
            return setting;
        }

        public static GameSetting GetSeminarGame()
        {
            GameSetting setting = new GameSetting();
            setting.MaxTalk = 10;
            setting.IsEnableNoAttack = false;
            setting.IsVoteVisible = true;

            Role[] roles = (Role[])Enum.GetValues(typeof(Enum));
            for (int i = 0; i < roles.Length; i++)
            {
                setting.RoleNumMap.Add(roles[i], seminarArray[i]);
            }
            return setting;
        }

        /// <summary>
        /// number of each characters
        /// </summary>
        public Dictionary<Role, int> RoleNumMap { get; set; }

        /// <summary>
        /// max number of talk
        /// </summary>
        public int MaxTalk { get; set; }

        /// <summary>
        /// Is the game permit to attack no one
        /// </summary>
        public bool IsEnableNoAttack { get; set; }

        /// <summary>
        /// Can agents see who vote to who
        /// </summary>
        public bool IsVoteVisible { get; set; }

        /// <summary>
        /// Are there vote in first day?
        /// </summary>
        public bool IsVotableInFirstDay { get; private set; }

        /// <summary>
        /// Random Seed
        /// </summary>
        public long RandomSeed { get; set; }

        public GameSetting()
        {
            RoleNumMap = new Dictionary<Role, int>();
            RandomSeed = Environment.TickCount;
        }

        public int GetRoleNum(Role role)
        {
            if (RoleNumMap.ContainsKey(role))
            {
                return RoleNumMap[role];
            }
            else
            {
                return 0;
            }
        }

        public int GetPlayerNum()
        {
            int num = 0;
            foreach (int value in RoleNumMap.Values)
            {
                num += value;
            }
            return num;
        }

        /// <summary>
        /// Create Copy
        /// </summary>
        public GameSetting Clone()
        {
            GameSetting gameSetting = new GameSetting();
            gameSetting.IsEnableNoAttack = IsEnableNoAttack;
            gameSetting.IsVotableInFirstDay = IsVotableInFirstDay;
            gameSetting.IsVoteVisible = IsVoteVisible;
            gameSetting.MaxTalk = MaxTalk;
            gameSetting.RandomSeed = RandomSeed;
            gameSetting.RoleNumMap = new Dictionary<Role, int>(RoleNumMap);
            return gameSetting;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
