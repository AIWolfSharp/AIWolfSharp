using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System.Collections.Generic;

namespace AIWolf.Server
{
    /// <summary>
    /// Record game information of a day.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class GameData
    {
        const int firstDay = 1;

        /// <summary>
        /// The day of the data.
        /// </summary>
        public int Day { get; private set; }

        /// <summary>
        /// Status of each agents.
        /// </summary>
        Dictionary<Agent, Status> agentStatusMap;

        /// <summary>
        /// Roles of each agents.
        /// </summary>
        Dictionary<Agent, Role> agentRoleMap;

        public List<Talk> TalkList { get; }

        public List<Talk> WhisperList { get; }

        public List<Vote> VoteList { get; }

        public List<Vote> AttackVoteList { get; }

        /// <summary>
        /// Result divine.
        /// </summary>
        public Judge Divine { get; set; }

        /// <summary>
        /// Result guard.
        /// </summary>
        public Guard Guard { get; set; }

        /// <summary>
        /// Agents who killed by villagers.
        /// </summary>
        public Agent Executed { get; set; }

        /// <summary>
        /// Agents who killed by werewolf.
        /// </summary>
        public Agent Attacked { get; set; }

        /// <summary>
        /// Agents who sudden death.
        /// </summary>
        public List<Agent> SuddendeathList { get; }

        /// <summary>
        /// Game data of one day before.
        /// </summary>
        public GameData DayBefore { get; private set; }

        public List<Agent> AgentList
        {
            get
            {
                return new List<Agent>(agentRoleMap.Keys);
            }
        }

        int talkIdx;

        int whisperIdx;

        /// <summary>
        /// ゲームの設定
        /// </summary>
        GameSetting gameSetting;

        public GameData(GameSetting gameSetting)
        {
            agentStatusMap = new Dictionary<Agent, Status>();
            agentRoleMap = new Dictionary<Agent, Role>();
            TalkList = new List<Talk>();
            WhisperList = new List<Talk>();
            VoteList = new List<Vote>();
            AttackVoteList = new List<Vote>();
            SuddendeathList = new List<Agent>();

            this.gameSetting = gameSetting;
        }

        /// <summary>
        /// Get specific game information.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public GameInfo GetGameInfo(Agent agent)
        {
            return GetGameInfoToSend(agent).ToGameInfo();
        }

        /// <summary>
        /// Get final game information.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public GameInfo GetFinalGameInfo(Agent agent)
        {
            return GetFinalGameInfoToSend(agent).ToGameInfo();
        }

        /// <summary>
        /// Get game info with all information.
        /// </summary>
        /// <returns></returns>
        public GameInfo GetGameInfo()
        {
            return GetFinalGameInfo(null);
        }

        public GameInfoToSend GetGameInfoToSend(Agent agent)
        {
            GameData today = this;
            GameInfoToSend gi = new GameInfoToSend();

            int day = today.Day;
            if (agent != null)
            {
                gi.Agent = agent.AgentIdx;
            }

            GameData yesterday = today.DayBefore;

            if (yesterday != null)
            {
                Agent executed = yesterday.Executed;
                if (executed != null)
                {
                    gi.ExecutedAgent = executed.AgentIdx;
                }

                Agent attacked = yesterday.Attacked;
                if (attacked != null)
                {
                    gi.AttackedAgent = attacked.AgentIdx;
                }

                if (gameSetting.VoteVisible)
                {
                    List<VoteToSend> voteList = new List<VoteToSend>();
                    foreach (Vote vote in yesterday.VoteList)
                    {
                        voteList.Add(new VoteToSend(vote));
                    }
                    gi.VoteList = voteList;
                }

                if (agent != null && (today.GetRole(agent) == Role.MEDIUM && executed != null))
                {
                    Species result = yesterday.GetRole(executed).GetSpecies();
                    gi.MediumResult = new JudgeToSend(new Judge(day, agent, executed, result));
                }

                if (agent == null || today.GetRole(agent) == Role.SEER)
                {
                    Judge divine = yesterday.Divine;
                    if (divine != null && divine.Target != null)
                    {
                        Species result = yesterday.GetRole(divine.Target).GetSpecies();
                        gi.DivineResult = new JudgeToSend(new Judge(day, divine.Agent, divine.Target, result));
                    }
                }

                if (agent == null || today.GetRole(agent) == Role.WEREWOLF)
                {
                    List<VoteToSend> attackVoteList = new List<VoteToSend>();
                    foreach (Vote vote in yesterday.AttackVoteList)
                    {
                        attackVoteList.Add(new VoteToSend(vote));
                    }
                    gi.AttackVoteList = attackVoteList;
                }
                if (agent == null || today.GetRole(agent) == Role.BODYGUARD)
                {
                    Guard guard = yesterday.Guard;
                    if (guard != null)
                    {
                        gi.GuardedAgent = guard.Target.AgentIdx;
                    }
                }
            }
            List<TalkToSend> talkList = new List<TalkToSend>();
            foreach (Talk talk in today.TalkList)
            {
                talkList.Add(new TalkToSend(talk));
            }
            gi.TalkList = talkList;

            Dictionary<int, string> statusMap = new Dictionary<int, string>();
            foreach (Agent a in agentStatusMap.Keys)
            {
                statusMap[a.AgentIdx] = agentStatusMap[a].ToString();
            }
            gi.StatusMap = statusMap;

            Dictionary<int, string> roleMap = new Dictionary<int, string>();
            Role? role = agentRoleMap.ContainsKey(agent) ? (Role?)agentRoleMap[agent] : null;

            if (Role.WEREWOLF == role || agent == null)
            {
                List<TalkToSend> whisperList = new List<TalkToSend>();
                foreach (Talk talk in today.WhisperList)
                {
                    whisperList.Add(new TalkToSend(talk));
                }
                gi.WhisperList = whisperList;
            }

            if (role != null)
            {
                roleMap[agent.AgentIdx] = role.ToString();
                if (today.GetRole(agent) == Role.WEREWOLF)
                {
                    foreach (Agent target in today.AgentList)
                    {
                        if (today.GetRole(target) == Role.WEREWOLF)
                        {
                            roleMap[target.AgentIdx] = Role.WEREWOLF.ToString();
                        }
                    }
                }
                if (today.GetRole(agent) == Role.FREEMASON)
                {
                    foreach (Agent target in today.AgentList)
                    {
                        if (today.GetRole(target) == Role.FREEMASON)
                        {
                            roleMap[target.AgentIdx] = Role.FREEMASON.ToString();
                        }
                    }
                }
            }
            gi.RoleMap = roleMap;
            gi.Day = day;

            return gi;
        }

        public GameInfoToSend GetFinalGameInfoToSend(Agent agent)
        {
            GameInfoToSend gi = GetGameInfoToSend(agent);

            Dictionary<int, string> roleMap = new Dictionary<int, string>();
            foreach (Agent a in agentRoleMap.Keys)
            {
                roleMap[a.AgentIdx] = agentRoleMap[a].ToString();
            }
            gi.RoleMap = roleMap;

            return gi;
        }

        /// <summary>
        /// Add new agent with their status and role.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="status"></param>
        /// <param name="role"></param>
        public void AddAgent(Agent agent, Status status, Role role)
        {
            agentRoleMap[agent] = role;
            agentStatusMap[agent] = status;
        }

        /// <summary>
        /// Get status of agent.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public Status GetStatus(Agent agent)
        {
            return agentStatusMap[agent];
        }

        public Role GetRole(Agent agent)
        {
            return agentRoleMap[agent];
        }

        public void AddTalk(Agent agent, Talk talk)
        {
            TalkList.Add(talk);
        }

        public void AddWhisper(Agent agent, Talk whisper)
        {
            WhisperList.Add(whisper);
        }

        /// <summary>
        /// Add vote data.
        /// </summary>
        /// <param name="vote"></param>
        public void AddVote(Vote vote)
        {
            VoteList.Add(vote);
        }

        public void AddAttack(Vote attack)
        {
            AttackVoteList.Add(attack);
        }

        /// <summary>
        /// Create GameData of next day.
        /// </summary>
        /// <returns></returns>
        public GameData NextDay()
        {
            GameData gameData = new GameData(gameSetting);

            gameData.Day = this.Day + 1;
            gameData.agentStatusMap = new Dictionary<Agent, Status>(agentStatusMap);
            if (Executed != null)
            {
                gameData.agentStatusMap[Executed] = Status.DEAD;
            }
            if (Attacked != null)
            {
                gameData.agentStatusMap[Attacked] = Status.DEAD;
            }
            gameData.agentRoleMap = new Dictionary<Agent, Role>(agentRoleMap);

            gameData.DayBefore = this;

            return gameData;
        }

        public List<Agent> GetFilteredAgentList(List<Agent> agentList, Species species)
        {
            List<Agent> resultList = new List<Agent>();
            foreach (Agent agent in agentList)
            {
                if (GetRole(agent).GetSpecies() == species)
                {
                    resultList.Add(agent);
                }
            }
            return resultList;
        }

        public List<Agent> GetFilteredAgentList(List<Agent> agentList, Status status)
        {
            List<Agent> resultList = new List<Agent>();
            foreach (Agent agent in agentList)
            {
                if (GetStatus(agent) == status)
                {
                    resultList.Add(agent);
                }
            }
            return resultList;
        }

        public List<Agent> GetFilteredAgentList(List<Agent> agentList, Role role)
        {
            List<Agent> resultList = new List<Agent>();
            foreach (Agent agent in agentList)
            {
                if (GetRole(agent) == role)
                {
                    resultList.Add(agent);
                }
            }
            return resultList;
        }

        public List<Agent> GetFilteredAgentList(List<Agent> agentList, Team team)
        {
            List<Agent> resultList = new List<Agent>();
            foreach (Agent agent in agentList)
            {
                if (GetRole(agent).GetTeam() == team)
                {
                    resultList.Add(agent);
                }
            }
            return resultList;
        }

        public int NextTalkIdx
        {
            get
            {
                return talkIdx++;
            }
        }

        public int NextWhisperIdx
        {
            get
            {
                return whisperIdx++;
            }
        }
    }
}
