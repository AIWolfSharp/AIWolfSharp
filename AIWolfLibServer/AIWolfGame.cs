using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Common.Util;
using AIWolf.Server.Net;
using AIWolf.Server.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Server
{
    /// <summary>
    /// Game Class of AI Wolf Contest.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class AIWolfGame
    {
        Random rand;

        /// <summary>
        /// Settings of the game.
        /// </summary>
        GameSetting gameSetting;

        /// <summary>
        /// Server to connect clients.
        /// </summary>
        IGameServer gameServer;

        Dictionary<int, GameData> gameDataMap;

        GameData gameData;

        /// <summary>
        /// Show console log?
        /// </summary>
        bool isShowConsoleLog = true;

        /// <summary>
        /// Logger.
        /// </summary>
        public IGameLogger GameLogger { get; set; }

        /// <summary>
        /// Name of Agents.
        /// </summary>
        Dictionary<Agent, string> agentNameMap;

        public AIWolfGame(GameSetting gameSetting, IGameServer gameServer)
        {
            rand = new Random();
            this.gameSetting = gameSetting;
            this.gameServer = gameServer;
        }

        public void setLogFile(string logFile)
        {
            GameLogger = new FileGameLogger(logFile);
        }

        /// <summary>
        /// Set Random Class
        /// </summary>
        /// <param name="rand"></param>
        public void SetRand(Random rand)
        {
            this.rand = rand;
        }

        /// <summary>
        /// Initialize Game
        /// </summary>
        public void Init()
        {
            gameDataMap = new Dictionary<int, GameData>();
            gameData = new GameData(gameSetting);
            agentNameMap = new Dictionary<Agent, string>();
            gameServer.SetGameData(gameData);

            List<Agent> agentList = gameServer.ConnectedAgentList;

            if (agentList.Count != gameSetting.PlayerNum)
            {
                throw new IllegalPlayerNumException("Player num is " + gameSetting.PlayerNum + " but connected agent is " + agentList.Count);
            }

            agentList = agentList.Shuffle().ToList();

            Dictionary<Role, List<Agent>> requestRoleMap = new Dictionary<Role, List<Agent>>();
            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                requestRoleMap[role] = new List<Agent>();
            }
            List<Agent> noRequestAgentList = new List<Agent>();
            foreach (Agent agent in agentList)
            {
                Role? requestedRole = gameServer.RequestRequestRole(agent);
                if (requestedRole != null)
                {
                    if (requestRoleMap[(Role)requestedRole].Count < gameSetting.GetRoleNum((Role)requestedRole))
                    {
                        requestRoleMap[(Role)requestedRole].Add(agent);
                    }
                    else
                    {
                        noRequestAgentList.Add(agent);
                    }
                }
                else
                {
                    noRequestAgentList.Add(agent);
                    Console.WriteLine(agent + " request no role");
                }
            }

            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                List<Agent> requestedAgentList = requestRoleMap[role];
                for (int i = 0; i < gameSetting.GetRoleNum(role); i++)
                {
                    if (requestedAgentList.Count == 0)
                    {
                        gameData.AddAgent(noRequestAgentList[0], Status.ALIVE, role);
                        noRequestAgentList.RemoveAt(0);
                    }
                    else
                    {
                        gameData.AddAgent(requestedAgentList[0], Status.ALIVE, role);
                        requestedAgentList.RemoveAt(0);
                    }
                }
            }

            gameDataMap.Add(gameData.Day, gameData);

            gameServer.SetGameData(gameData);
            gameServer.SetGameSetting(gameSetting);
            foreach (Agent agent in agentList)
            {
                gameServer.Init(agent);
                agentNameMap[agent] = gameServer.RequestName(agent);
            }
        }

        /// <summary>
        /// Start game.
        /// </summary>
        public void Start()
        {
            Init();

            while (!IsGameFinished)
            {
                Log();

                Day();
                Night();
                if (GameLogger != null)
                {
                    GameLogger.Flush();
                }
            }
            Log();
            Finish();

            if (isShowConsoleLog)
            {
                Console.WriteLine("Winner:" + GetWinner());
            }
        }

        public void Finish()
        {
            if (GameLogger != null)
            {
                foreach (Agent agent in gameData.AgentList.OrderBy(x => x.AgentIdx))
                {
                    GameLogger.Log(string.Format("{0},status,{1},{2},{3},{4}", gameData.Day, agent.AgentIdx, gameData.GetRole(agent), gameData.GetStatus(agent), agentNameMap[agent]));
                }
                GameLogger.Log(string.Format("{0},result,{1},{2},{3}", gameData.Day, AliveHumanList.Count, AliveWolfList.Count, GetWinner()));
                GameLogger.Close();
            }

            foreach (Agent agent in gameData.AgentList)
            {
                gameServer.Finish(agent);
            }
        }

        /// <summary>
        /// Get won team.
        /// <para>
        /// If game not finished, return null.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public Team? GetWinner()
        {
            int humanSide = 0;
            int wolfSide = 0;
            foreach (Agent agent in gameData.AgentList)
            {
                if (gameData.GetStatus(agent) == Status.DEAD)
                {
                    continue;
                }
                if (gameData.GetRole(agent).GetSpecies() == Species.HUMAN)
                {
                    humanSide++;
                }
                else
                {
                    wolfSide++;
                }
            }
            if (wolfSide == 0)
            {
                return Team.VILLAGER;
            }
            else if (humanSide <= wolfSide)
            {
                return Team.WEREWOLF;
            }
            else
            {
                return null;
            }
        }

        private void Log()
        {
            if (!isShowConsoleLog)
            {
                return;
            }

            GameData yesterday = gameData.DayBefore;

            Console.WriteLine("=============================================");
            if (yesterday != null)
            {
                Console.WriteLine("Day {0:00}", yesterday.Day);
                Console.WriteLine("========talk========");
                foreach (Talk talk in yesterday.TalkList)
                {
                    Console.WriteLine(talk);
                }
                Console.WriteLine("========Whisper========");
                foreach (Talk whisper in yesterday.WhisperList)
                {
                    Console.WriteLine(whisper);
                }

                Console.WriteLine("========Actions========");
                foreach (Vote vote in yesterday.VoteList)
                {
                    Console.WriteLine("Vote:{0}->{1}", vote.Agent, vote.Target);
                }

                //			Console.WriteLine("Attack Vote Result");
                foreach (Vote vote in yesterday.AttackVoteList)
                {
                    Console.WriteLine("AttackVote:{0}->{1}", vote.Agent, vote.Target);
                }

                Judge divine = yesterday.Divine;
                Console.WriteLine("{0} executed", yesterday.Executed);
                if (divine != null)
                {
                    Console.WriteLine("{0} divine {1}. Result is {2}", divine.Agent, divine.Target, divine.Result);
                }
                Guard guard = yesterday.Guard;
                if (guard != null)
                {
                    Console.WriteLine("{0} guarded", guard);
                }
                Agent attacked = yesterday.Attacked;
                if (attacked != null)
                {
                    Console.WriteLine("{0} attacked", attacked);
                }
            }
            Console.WriteLine("======");
            List<Agent> agentList = gameData.AgentList;
            agentList.Sort();
            foreach (Agent agent in agentList)
            {
                Console.Write("{0}\t{1}\t{2}\t{3}", agent, agentNameMap[agent], gameData.GetStatus(agent), gameData.GetRole(agent));
                if (yesterday != null)
                {
                    if (yesterday.Executed == agent)
                    {
                        Console.Write("\texecuted");
                    }
                    if (yesterday.Attacked == agent)
                    {
                        Console.Write("\tattacked");
                    }
                    Judge divine = yesterday.Divine;
                    if (divine != null && divine.Target == agent)
                    {
                        Console.Write("\tdivined");
                    }
                    Guard guard = yesterday.Guard;
                    if (guard != null && guard.Target == agent)
                    {
                        Console.Write("\tguarded");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("Human:{0}\nWerewolf:{1}\n", AliveHumanList.Count, AliveWolfList.Count);

            Console.WriteLine("=============================================");

        }

        public void Day()
        {
            DayStart();
            Talk();

            if (gameData.Day != 0)
            {
                Vote();
            }
            Divine();
            if (gameData.Day != 0)
            {
                Guard();
                Attack();
            }
        }

        public void Night()
        {
            foreach (Agent agent in gameData.AgentList)
            {
                gameServer.DayFinish(agent);
            }

            //Vote
            List<Vote> voteList = gameData.VoteList;
            Agent executed = GetVotedAgent(voteList);
            if (executed != null)
            {
                if (gameData.GetStatus(executed) == Status.ALIVE && gameData.Day != 0)
                {
                    gameData.Executed = executed;
                    if (GameLogger != null)
                    {
                        GameLogger.Log(string.Format("{0},execute,{1},{2}", gameData.Day, executed.AgentIdx, gameData.GetRole(executed)));
                    }
                }

                //Attack
                if (!(AliveWolfList.Count == 1 && gameData.GetRole(gameData.Executed) == Role.WEREWOLF) && gameData.Day != 0)
                {
                    List<Vote> attackCandidateList = gameData.AttackVoteList;
                    attackCandidateList.RemoveAll(v => v.Agent == executed);

                    Agent attacked = GetAttackVotedAgent(attackCandidateList);
                    if (attacked == executed)
                    {
                        attacked = null;
                    }

                    bool isGuarded = false;
                    if (gameData.Guard != null)
                    {
                        if (gameData.Guard.Target.Equals(attacked) && attacked != null)
                        {
                            if (gameData.Executed == null || !gameData.Executed.Equals(gameData.Guard.Agent))
                            {
                                isGuarded = true;
                            }
                        }
                    }
                    if (!isGuarded && attacked != null)
                    {
                        gameData.Attacked = attacked;
                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},attack,{1},true", gameData.Day, attacked.AgentIdx));
                        }
                    }
                    else if (attacked != null)
                    {
                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},attack,{1},false", gameData.Day, attacked.AgentIdx));
                        }
                    }
                    else
                    {
                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},attack,-1,false", gameData.Day));
                        }
                    }
                }
            }
            gameData = gameData.NextDay();
            gameDataMap.Add(gameData.Day, gameData);
            gameServer.SetGameData(gameData);
        }

        public Agent GetVotedAgent(List<Vote> voteList)
        {
            Dictionary<Agent, int> counter = new Dictionary<Agent, int>();
            foreach (Vote vote in voteList)
            {
                if (gameData.GetStatus(vote.Target) == Status.ALIVE)
                {
                    if (counter.ContainsKey(vote.Target))
                    {
                        counter[vote.Target]++;
                    }
                    else
                    {
                        counter.Add(vote.Target, 1);
                    }
                }
            }

            int max = 0;
            if (counter.Count > 0)
            {
                max = counter.OrderBy(i => i.Value).Last().Value;
            }
            List<Agent> candidateList = new List<Agent>();
            foreach (Agent agent in counter.Keys)
            {
                if (counter[agent] == max)
                {
                    candidateList.Add(agent);
                }
            }

            if (candidateList.Count == 0)
            {
                return null;
            }
            else
            {
                return candidateList.Shuffle().First();
            }
        }

        public Agent GetAttackVotedAgent(List<Vote> voteList)
        {
            Dictionary<Agent, int> counter = new Dictionary<Agent, int>();
            foreach (Vote vote in voteList)
            {
                if (gameData.GetStatus(vote.Target) == Status.ALIVE && gameData.GetRole(vote.Target) != Role.WEREWOLF)
                {
                    if (counter.ContainsKey(vote.Target))
                    {
                        counter[vote.Target]++;
                    }
                    else
                    {
                        counter.Add(vote.Target, 1);
                    }
                }
            }
            if (!gameSetting.EnableNoAttack)
            {
                foreach (Agent agent in AliveHumanList)
                {
                    if (counter.ContainsKey(agent))
                    {
                        counter[agent]++;
                    }
                    else
                    {
                        counter.Add(agent, 1);
                    }
                }
            }

            int max = counter.OrderBy(i => i.Value).Last().Value;
            List<Agent> candidateList = new List<Agent>();
            foreach (Agent agent in counter.Keys)
            {
                if (counter[agent] == max)
                {
                    candidateList.Add(agent);
                }
            }

            if (candidateList.Count == 0)
            {
                return null;
            }
            else
            {
                return candidateList.Shuffle().First();
            }
        }

        public void DayStart()
        {
            if (GameLogger != null)
            {
                foreach (Agent agent in gameData.AgentList.OrderBy(x => x.AgentIdx))
                {
                    GameLogger.Log(string.Format("{0},status,{1},{2},{3},{4}", gameData.Day, agent.AgentIdx, gameData.GetRole(agent), gameData.GetStatus(agent), agentNameMap[agent]));
                }
            }

            foreach (Agent agent in gameData.AgentList)
            {
                gameServer.DayStart(agent);
            }
        }

        /// <summary>
        /// First, all agents have chances to talk.
        /// Next, wolves whispers.
        /// Continue them until all agents finish talking.
        /// </summary>
        public void Talk()
        {
            HashSet<Agent> overSet = new HashSet<Agent>();
            for (int i = 0; i < gameSetting.MaxTalk; i++)
            {
                bool continueTalk = false;

                List<Agent> aliveList = AliveAgentList.Shuffle().ToList();
                aliveList.RemoveAll(a => overSet.Contains(a));
                foreach (Agent a in overSet)
                {
                    aliveList.Add(a);
                }
                foreach (Agent agent in aliveList)
                {
                    if (overSet.Contains(agent))
                    {
                        continue;
                    }
                    string talkContent = gameServer.RequestTalk(agent);
                    if (talkContent != null)
                    {
                        if (talkContent.Length != 0)
                        {
                            Talk sentence = new Talk(gameData.NextTalkIdx, gameData.Day, agent, talkContent);
                            gameData.AddTalk(agent, sentence);
                            if (!talkContent.Equals(Common.Data.Talk.OVER))
                            {
                                continueTalk = true;
                                overSet.Clear();
                            }
                            else
                            {
                                overSet.Add(agent);
                            }
                            if (GameLogger != null)
                            {
                                GameLogger.Log(string.Format("{0},talk,{1},{2},{3}", gameData.Day, sentence.Idx, agent.AgentIdx, sentence.Content));
                            }
                        }
                    }
                }
                Whisper();

                if (!continueTalk)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Whisper by werewolf.
        /// </summary>
        public void Whisper()
        {
            HashSet<Agent> overSet = new HashSet<Agent>();
            for (int j = 0; j < gameSetting.MaxTalk; j++)
            {
                List<Agent> aliveList = AliveAgentList.Shuffle().ToList(); ;
                bool continueWhisper = false;
                aliveList.RemoveAll(a => overSet.Contains(a));
                foreach (Agent a in overSet)
                {
                    aliveList.Add(a);
                }
                foreach (Agent agent in aliveList)
                {
                    if (gameData.GetRole(agent) == Role.WEREWOLF)
                    {
                        if (overSet.Contains(agent))
                        {
                            continue;
                        }
                        string whisperContent = gameServer.RequestWhisper(agent);
                        if (whisperContent != null && whisperContent.Length != 0)
                        {
                            Talk whisper = new Talk(gameData.NextWhisperIdx, gameData.Day, agent, whisperContent);
                            gameData.AddWhisper(agent, whisper);
                            if (!whisperContent.Equals(Common.Data.Talk.OVER))
                            {
                                continueWhisper = true;
                                overSet.Clear();
                            }
                            else
                            {
                                overSet.Add(agent);
                            }

                            if (GameLogger != null)
                            {
                                GameLogger.Log(string.Format("{0},whisper,{1},{2},{3}", gameData.Day, whisper.Idx, agent.AgentIdx, whisper.Content));
                            }
                        }
                    }
                }
                if (!continueWhisper)
                {
                    break;
                }
            }
        }

        public void Vote()
        {
            foreach (Agent agent in AliveAgentList)
            {
                Agent target = gameServer.RequestVote(agent);
                if (gameData.GetStatus(target) == Status.DEAD || target == null || agent.Equals(target))
                {
                    target = GetRandomAgent(AliveAgentList, agent);
                }
                Vote vote = new Vote(gameData.Day, agent, target);
                gameData.AddVote(vote);

                if (GameLogger != null)
                {
                    GameLogger.Log(string.Format("{0},vote,{1},{2}", gameData.Day, vote.Agent.AgentIdx, vote.Target.AgentIdx));
                }
            }
        }

        public void Divine()
        {
            foreach (Agent agent in AliveAgentList)
            {
                if (gameData.GetRole(agent) == Role.SEER)
                {
                    Agent target = gameServer.RequestDivineTarget(agent);
                    if (gameData.GetStatus(target) == Status.DEAD || target == null)
                    {
                        //					target = getRandomAgent(agentList, agent);
                    }
                    else
                    {
                        Judge divine = new Judge(gameData.Day, agent, target, gameData.GetRole(target).GetSpecies());
                        gameData.Divine = divine;

                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},divine,{1},{2},{3}", gameData.Day, divine.Agent.AgentIdx, divine.Target.AgentIdx, divine.Result));
                        }
                    }
                }
            }
        }

        public void Guard()
        {
            foreach (Agent agent in AliveAgentList)
            {
                if (gameData.GetRole(agent) == Role.BODYGUARD)
                {
                    if (agent == gameData.Executed)
                    {
                        continue;
                    }
                    Agent target = gameServer.RequestGuardTarget(agent);
                    if (target == null || agent.Equals(target))
                    {
                        //					target = getRandomAgent(agentList, agent);
                    }
                    else
                    {
                        Guard guard = new Guard(gameData.Day, agent, target);
                        gameData.Guard = guard;

                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},guard,{1},{2},{3}", gameData.Day, guard.Agent.AgentIdx, guard.Target.AgentIdx, gameData.GetRole(guard.Target)));
                        }
                    }
                }
            }
        }

        public void Attack()
        {
            List<Agent> randomTargetCandidateList = AliveAgentList;
            randomTargetCandidateList.RemoveAll(x => gameData.GetRole(x) == Role.WEREWOLF);

            foreach (Agent agent in AliveAgentList)
            {
                if (gameData.GetRole(agent) == Role.WEREWOLF)
                {
                    Agent target = gameServer.RequestAttackTarget(agent);
                    if (gameData.GetStatus(target) == Status.DEAD || gameData.GetRole(target) == Role.WEREWOLF || target == null)
                    {
                        //					target = getRandomAgent(randomTargetCandidateList, agent);
                    }
                    else
                    {
                        Vote attackVote = new Vote(gameData.Day, agent, target);
                        gameData.AddAttack(attackVote);

                        if (GameLogger != null)
                        {
                            GameLogger.Log(string.Format("{0},attackVote,{1},{2}", gameData.Day, attackVote.Agent.AgentIdx, attackVote.Target.AgentIdx));
                        }
                    }
                }
            }
        }



        /// <summary>
        /// ランダムなエージェントを獲得する．ただし，withoutを除く．
        /// </summary>
        /// <param name="agentList"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        public Agent GetRandomAgent(List<Agent> agentList, params Agent[] without)
        {
            List<Agent> list = new List<Agent>(agentList);
            foreach (Agent agent in without)
            {
                list.Remove(agent);
            }
            return list.Shuffle().First();
        }

        /// <summary>
        /// Alive agents.
        /// </summary>
        public List<Agent> AliveAgentList
        {
            get
            {
                List<Agent> agentList = new List<Agent>();
                foreach (Agent agent in gameData.AgentList)
                {
                    if (gameData.GetStatus(agent) == Status.ALIVE)
                    {
                        agentList.Add(agent);
                    }
                }
                return agentList;
            }
        }

        public List<Agent> AliveHumanList
        {
            get
            {
                return gameData.GetFilteredAgentList(AliveAgentList, Species.HUMAN);
            }
        }

        public List<Agent> AliveWolfList
        {
            get
            {
                return gameData.GetFilteredAgentList(AliveAgentList, Species.WEREWOLF);
            }
        }

        public bool IsGameFinished
        {
            get
            {
                return GetWinner() != null;
            }
        }
    }
}
