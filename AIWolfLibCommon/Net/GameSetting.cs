using AIWolf.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Settings of game.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class GameSetting : ICloneable
    {
        /// <summary>
        /// Num of each roles.
        /// <para>
        /// Bodyguard, FreeMason, Medium, Possessed, Seer, Villager, Werewolf.
        /// </para>
        /// </summary>
        static readonly int[][] roleNumArray =
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
        static readonly int[] seminarArray =
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
            setting.EnableNoAttack = false;
            setting.VoteVisible = true;
            setting.VotableInFirstDay = false;

            Role[] roles = (Role[])Enum.GetValues(typeof(Role));
            for (int i = 0; i < roles.Length; i++)
            {
                setting.RoleNumMap[roles[i]] = roleNumArray[agentNum][i];
            }
            return setting;
        }

        public static GameSetting GetSeminarGame()
        {
            GameSetting setting = new GameSetting();
            setting.MaxTalk = 10;
            setting.EnableNoAttack = false;
            setting.VoteVisible = true;

            Role[] roles = (Role[])Enum.GetValues(typeof(Enum));
            for (int i = 0; i < roles.Length; i++)
            {
                setting.RoleNumMap[roles[i]] = seminarArray[i];
            }
            return setting;
        }

        /// <summary>
        /// Number of each characters.
        /// </summary>
        [DataMember(Name = "roleNumMap")]
        public Dictionary<Role, int> RoleNumMap { get; set; }

        /// <summary>
        /// Max number of talk.
        /// </summary>
        [DataMember(Name = "maxTalk")]
        public int MaxTalk { get; set; }

        /// <summary>
        /// Is the game permit to attack no one?
        /// </summary>
        [DataMember(Name = "enableNoAttack")]
        public bool EnableNoAttack { get; set; }

        /// <summary>
        /// Can agents see who vote to who?
        /// </summary>
        [DataMember(Name = "voteVisible")]
        public bool VoteVisible { get; set; }

        /// <summary>
        /// Are there vote in first day?
        /// </summary>
        [DataMember(Name = "votableInFirstDay")]
        public bool VotableInFirstDay { get; private set; }

        /// <summary>
        /// Random seed.
        /// </summary>
        [DataMember(Name = "randomSeed")]
        public int RandomSeed { get; set; }

        public GameSetting()
        {
            RoleNumMap = new Dictionary<Role, int>();
            RandomSeed = Environment.TickCount;
        }

        [DataMember(Name = "playerNum")]
        public int PlayerNum
        {
            get
            {
                return RoleNumMap.Values.Sum();
            }
        }

        /// <summary>
        /// Create copy.
        /// </summary>
        public object Clone()
        {
            GameSetting gameSetting = new GameSetting();
            gameSetting.EnableNoAttack = EnableNoAttack;
            gameSetting.VotableInFirstDay = VotableInFirstDay;
            gameSetting.VoteVisible = VoteVisible;
            gameSetting.MaxTalk = MaxTalk;
            gameSetting.RandomSeed = RandomSeed;
            gameSetting.RoleNumMap = new Dictionary<Role, int>(RoleNumMap);
            return gameSetting;
        }
    }
}
