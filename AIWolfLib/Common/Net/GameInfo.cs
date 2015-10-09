﻿using AIWolf.Common.Data;
using System.Collections.Generic;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information which send to each player.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class GameInfo
    {
        public int Day { get; set; }

        /// <summary>
        /// Yourself.
        /// </summary>
        public Agent Agent { get; set; }

        /// <summary>
        /// Player's role.
        /// </summary>
        public Role? Role
        {
            get
            {
                return Agent != null ? (Role?)RoleMap[Agent] : null;
            }
        }

        /// <summary>
        /// Result of medium. Medium only.
        /// </summary>
        public Judge MediumResult { get; set; }

        /// <summary>
        /// Result of the dvine. Seer only.
        /// </summary>
        public Judge DivineResult { get; set; }

        /// <summary>
        /// Agent who was executed last night.
        /// </summary>
        public Agent ExecutedAgent { get; set; }

        /// <summary>
        /// Agent who was attacked last night.
        /// </summary>
        public Agent AttackedAgent { get; set; }

        /// <summary>
        /// Agent who was guarded last night.
        /// </summary>
        public Agent GuardedAgent { get; set; }

        /// <summary>
        /// Vote list. You can find who vote to who.
        /// </summary>
        public List<Vote> VoteList { get; set; }

        /// <summary>
        /// Attack vote list. Werewolf only.
        /// </summary>
        public List<Vote> AttackVoteList { get; set; }

        /// <summary>
        /// Today's talks.
        /// </summary>
        public List<Talk> TalkList { get; set; }

        /// <summary>
        /// Today's whispers. Werewolf only.
        /// </summary>
        public List<Talk> WhisperList { get; set; }

        /// <summary>
        /// Statuses of all agents.
        /// </summary>
        public Dictionary<Agent, Status> StatusMap { get; set; }

        /// <summary>
        /// Known roles of agents.
        /// <para>
        /// If you are human, you know only yourself.
        /// If you are werewolf, you know other werewolves.
        /// </para>
        /// </summary>
        public Dictionary<Agent, Role> RoleMap { get; set; }

        /// <summary>
        /// List of agents.
        /// </summary>
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
