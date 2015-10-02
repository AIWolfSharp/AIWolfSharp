using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System.Collections.Generic;

namespace AIWolf.Server.Net
{
    interface IGameServer
    {
        List<Agent> ConnectedAgentList { get; }

        /// <summary>
        /// Set GameSetting.
        /// </summary>
        /// <param name="gameSetting"></param>
        void SetGameSetting(GameSetting gameSetting);

        void Init(Agent agent);

        /// <summary>
        /// Request agent's name.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        string RequestName(Agent agent);

        /// <summary>
        /// Request roles that agent request.
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        Role? RequestRequestRole(Agent agent);

        string RequestTalk(Agent agent);

        string RequestWhisper(Agent agent);

        Agent RequestVote(Agent agent);

        Agent RequestDivineTarget(Agent agent);

        Agent RequestGuardTarget(Agent agent);

        Agent RequestAttackTarget(Agent agent);

        void SetGameData(GameData gameData);

        /// <summary>
        /// Called when day started.
        /// </summary>
        /// <param name="agent"></param>
        void DayStart(Agent agent);

        /// <summary>
        /// Called when day finished.
        /// </summary>
        /// <param name="agent"></param>
        void DayFinish(Agent agent);

        /// <summary>
        /// Send finished message.
        /// </summary>
        /// <param name="agent"></param>
        void Finish(Agent agent);

        /// <summary>
        /// Close connections.
        /// </summary>
        void Close();
    }
}
