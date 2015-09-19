using AIWolf.Common.Data;
using AIWolf.Common.Net;

namespace AIWolf.Client.Base.Player
{
    /// <summary>
    /// Player for AI Wolf.
    /// <para>
    /// Implements Player to create Agent for AI Wolf.
    /// </para>
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public abstract class AbstractPlayer : IPlayer
    {
        /// <summary>
        /// Player name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Called when the game information updated.
        /// </summary>
        /// <param name="gameInfo"></param>
        public abstract void Update(GameInfo gameInfo);

        /// <summary>
        /// Called when the game started.
        /// </summary>
        /// <param name="gameInfo">Information about current game status.</param>
        /// <param name="gameSetting">Game settings.</param>
        public abstract void Initialize(GameInfo gameInfo, GameSetting gameSetting);

        /// <summary>
        /// Called when the day started.
        /// </summary>
        public abstract void DayStart();

        /// <summary>
        /// Each player can talk thier opinions throw this method.
        /// <para>
        /// Return texts must be written in aiwolf protocol.
        /// When you return null, it will be considered as SKIP.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public abstract string Talk();

        /// <summary>
        /// Each wolves can talk their opinions throw this method.
        /// </summary>
        /// <returns>AIWolf protocol based whisper.</returns>
        public abstract string Whisper();

        /// <summary>
        /// Vote agent to execute.
        /// </summary>
        /// <returns></returns>
        public abstract Agent Vote();

        /// <summary>
        /// Decide agent who to be attacked by wolves.
        /// </summary>
        /// <returns></returns>
        public abstract Agent Attack();

        /// <summary>
        /// Decide agent to divine by seer.
        /// </summary>
        /// <returns></returns>
        public abstract Agent Divine();

        /// <summary>
        /// Decide agent to guard by bodyguard.
        /// </summary>
        /// <returns></returns>
        public abstract Agent Guard();

        /// <summary>
        /// Called when the game finished.
        /// <para>
        /// Before this method is called, gameinfo is updated with all information.
        /// </para>
        /// </summary>
        public abstract void Finish();
    }
}
