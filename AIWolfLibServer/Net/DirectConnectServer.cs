using AIWolf.Common;
using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIWolf.Server.Net
{
    /// <summary>
    /// Connect player and server directly.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class DirectConnectServer : IGameServer
    {
        /// <summary>
        /// Agents connected to the server.
        /// </summary>
        Dictionary<Agent, IPlayer> agentPlayerMap;

        /// <summary>
        /// Agents connected to the server.
        /// </summary>
        Dictionary<IPlayer, Agent> playerAgentMap;

        Dictionary<Agent, Role?> requestRoleMap;

        /// <summary>
        /// GameData.
        /// </summary>
        GameData gameData;

        /// <summary>
        /// Game Setting.
        /// </summary>
        GameSetting gameSetting;

        public DirectConnectServer(List<IPlayer> playerList)
        {
            agentPlayerMap = new Dictionary<Agent, IPlayer>();
            playerAgentMap = new Dictionary<IPlayer, Agent>();
            int idx = 1;
            foreach (IPlayer player in playerList)
            {
                Agent agent = Agent.GetAgent(idx++);
                agentPlayerMap[agent] = player;
                playerAgentMap[player] = agent;
            }
            requestRoleMap = new Dictionary<Agent, Role?>();
        }

        public DirectConnectServer(Dictionary<IPlayer, Role?> playerMap)
        {
            agentPlayerMap = new Dictionary<Agent, IPlayer>();
            playerAgentMap = new Dictionary<IPlayer, Agent>();
            requestRoleMap = new Dictionary<Agent, Role?>();

            int idx = 1;
            foreach (KeyValuePair<IPlayer, Role?> pair in playerMap)
            {
                Agent agent = Agent.GetAgent(idx++);
                agentPlayerMap[agent] = pair.Key;
                playerAgentMap[pair.Key] = agent;
                requestRoleMap[agent] = pair.Value;
            }
        }

        public List<Agent> ConnectedAgentList
        {
            get
            {
                return agentPlayerMap.Keys.ToList();
            }
        }

        public void SetGameData(GameData gameData)
        {
            this.gameData = gameData;
        }

        public void SetGameSetting(GameSetting gameSetting)
        {
            this.gameSetting = gameSetting;
        }

        public void Init(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Initialize(gameData.GetGameInfo(agent), (GameSetting)gameSetting.Clone());
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "Init", e);
            }
        }

        public string RequestName(Agent agent)
        {
            try
            {
                string name = agentPlayerMap[agent].GetName();
                if (name != null)
                {
                    return name;
                }
                else
                {
                    return agentPlayerMap[agent].GetType().Name;
                }
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestName", e);
            }
        }

        public Role? RequestRequestRole(Agent agent)
        {
            try
            {
                return requestRoleMap[agent];
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestRequestRole", e);
            }
        }

        public void DayStart(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
                agentPlayerMap[agent].DayStart();
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "DayStart", e);
            }
        }

        public void DayFinish(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "DayFinish", e);
            }
        }

        public string RequestTalk(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
                string talk = agentPlayerMap[agent].Talk();
                return talk;
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestTalk", e);
            }
        }

        public string RequestWhisper(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
                string whisper = agentPlayerMap[agent].Whisper();
                return whisper;
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestWhisper", e);
            }
        }

        public Agent RequestVote(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
                Agent target = agentPlayerMap[agent].Vote();
                return target;
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestVote", e);
            }
        }

        public Agent RequestDivineTarget(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
                Agent target = agentPlayerMap[agent].Divine();
                return target;
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestDivineTarget", e);
            }
        }


        public Agent RequestGuardTarget(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
                Agent target = agentPlayerMap[agent].Guard();
                return target;
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestGuardTarget", e);
            }
        }

        public Agent RequestAttackTarget(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetGameInfo(agent));
                Agent target = agentPlayerMap[agent].Attack();
                return target;
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "RequestAttackTarget", e);
            }
        }

        public void Finish(Agent agent)
        {
            try
            {
                agentPlayerMap[agent].Update(gameData.GetFinalGameInfo(agent));
                agentPlayerMap[agent].Finish();
            }
            catch (Exception e)
            {
                throw new AIWolfAgentException(agent, "Finish", e);
            }
        }

        public void Close()
        {
        }

        public Agent GetAgent(IPlayer player)
        {
            return playerAgentMap[player];
        }

        public IPlayer GetPlayer(Agent agent)
        {
            return agentPlayerMap[agent];
        }
    }
}
