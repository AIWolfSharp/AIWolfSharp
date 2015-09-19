using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information which send to each player
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class GameInfo
    {
        public int Day { get; }
        /// <summary>
        /// yourself
        /// </summary>
        public Agent Agent { get; }
        /// <summary>
        /// player's role
        /// </summary>
        public Role Role
        {
            get
            {
                return RoleMap[Agent];
            }
        }

        /// <summary>
        /// Result of medium. medium only
        /// </summary>
        public Judge MediumResult { get; }
        /// <summary>
        /// Result of the dvine. seer only
        /// </summary>
        public Judge DivineResult { get; }
        /// <summary>
        /// agent who executed last night
        /// </summary>
        public Agent ExecutedAgent { get; }
        /// <summary>
        /// agent who attacked last night
        /// </summary>
        public Agent AttackedAgent { get; }
        /// <summary>
        /// agent who guarded last night
        /// </summary>
        public Agent GuardedAgent { get; }
        /// <summary>
        /// Vote list. You can find who vote to who
        /// </summary>
        public List<Vote> VoteList { get; }
        /// <summary>
        /// Attack vote list. werewolf only
        /// </summary>
        public List<Vote> AttackVoteList { get; }

        /// <summary>
        /// Today's talks
        /// </summary>
        public List<Talk> TalkList { get; }
        /// <summary>
        /// today's whispers. werewolf only
        /// </summary>
        public List<Talk> WhisperList { get; }

        /// <summary>
        /// statuses of all agents
        /// </summary>
        public Dictionary<Agent, Status> StatusMap { get; }
        /// <summary>
        /// Known roles of agents.
        /// <para>
        /// if you are human, you know only yourself.
        /// if you are werewolf, you know other werewolves.
        /// </para>
        /// </summary>
        public Dictionary<Agent, Role> RoleMap { get; }

        /// <summary>
        /// list of agents
        /// </summary>
        public List<Agent> AgentList
        {
            get
            {
                return new List<Agent>(StatusMap.Keys);
            }
        }

        /// <summary>
        /// list of alive agents
        /// </summary>
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
