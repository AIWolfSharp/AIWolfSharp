using AIWolf.Common.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information which send to each player.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class GameInfo
    {
        [DataMember(Name = "day")]
        public int Day { get; set; }

        /// <summary>
        /// Yourself.
        /// </summary>
        [DataMember(Name = "agent")]
        public Agent Agent { get; set; }

        /// <summary>
        /// Player's role.
        /// </summary>
        [DataMember(Name = "role")]
        public Role? Role
        {
            get
            {
                return Agent != null && RoleMap.ContainsKey(Agent) ? (Role?)RoleMap[Agent] : null;
            }
        }

        /// <summary>
        /// Result of medium. Medium only.
        /// </summary>
        [DataMember(Name = "mediumResult")]
        public Judge MediumResult { get; set; }

        /// <summary>
        /// Result of the dvine. Seer only.
        /// </summary>
        [DataMember(Name = "divineResult")]
        public Judge DivineResult { get; set; }

        /// <summary>
        /// Agent who was executed last night.
        /// </summary>
        [DataMember(Name = "executedAgent")]
        public Agent ExecutedAgent { get; set; }

        /// <summary>
        /// Agent who was attacked last night.
        /// </summary>
        [DataMember(Name = "attackedAgent")]
        public Agent AttackedAgent { get; set; }

        /// <summary>
        /// Agent who was guarded last night.
        /// </summary>
        [DataMember(Name = "guardedAgent")]
        public Agent GuardedAgent { get; set; }

        /// <summary>
        /// Vote list. You can find who vote to who.
        /// </summary>
        [DataMember(Name = "voteList")]
        public List<Vote> VoteList { get; set; }

        /// <summary>
        /// Attack vote list. Werewolf only.
        /// </summary>
        [DataMember(Name = "attackVoteList")]
        public List<Vote> AttackVoteList { get; set; }

        /// <summary>
        /// Today's talks.
        /// </summary>
        [DataMember(Name = "talkList")]
        public List<Talk> TalkList { get; set; }

        /// <summary>
        /// Today's whispers. Werewolf only.
        /// </summary>
        [DataMember(Name = "whisperList")]
        public List<Talk> WhisperList { get; set; }

        /// <summary>
        /// Statuses of all agents.
        /// </summary>
        [DataMember(Name = "statusMap")]
        public Dictionary<Agent, Status> StatusMap { get; set; }

        /// <summary>
        /// Known roles of agents.
        /// <para>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </para>
        /// </summary>
        [DataMember(Name = "roleMap")]
        public Dictionary<Agent, Role> RoleMap { get; set; }

        /// <summary>
        /// List of agents.
        /// </summary>
        [DataMember(Name = "agentList")]
        public List<Agent> AgentList
        {
            get
            {
                return new List<Agent>(StatusMap.Keys);
            }
        }

        /// <summary>
        /// List of alive agents.
        /// </summary>
        [DataMember(Name = "aliveAgentList")]
        public List<Agent> AliveAgentList
        {
            get
            {
                List<Agent> aliveAgentList = new List<Agent>();
                if (AgentList != null)
                {
                    foreach (Agent target in AgentList)
                    {
                        if (StatusMap[target] == Status.ALIVE)
                        {
                            aliveAgentList.Add(target);
                        }
                    }
                }
                return aliveAgentList;
            }
        }

        public GameInfo()
        {
            VoteList = new List<Vote>();
            AttackVoteList = new List<Vote>();
            TalkList = new List<Talk>();
            WhisperList = new List<Talk>();
            StatusMap = new Dictionary<Agent, Status>();
            RoleMap = new Dictionary<Agent, Role>();
        }
    }
}
