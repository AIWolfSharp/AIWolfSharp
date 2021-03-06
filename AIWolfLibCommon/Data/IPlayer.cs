//
// IPlayer.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Net;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Player for AI Wolf
    /// <para>
    /// Implements IPlayer to create Agent for AI Wolf.
    /// </para>
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Player name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Called when the game information updated.
        /// </summary>
        /// <param name="gameInfo"></param>
        void Update(GameInfo gameInfo);

        /// <summary>
        /// Called when the game started.
        /// </summary>
        /// <param name="gameInfo">Information about current game status.</param>
        /// <param name="gameSetting">Game settings.</param>
        void Initialize(GameInfo gameInfo, GameSetting gameSetting);

        /// <summary>
        /// Called when the day started.
        /// </summary>
        void DayStart();

        /// <summary>
        /// Each player can talk thier opinions throw this method.
        /// <para>
        /// Return texts must be written in aiwolf protocol.
        /// When you return null, it will be considered as SKIP.
        /// </para>
        /// </summary>
        /// <returns></returns>
        string Talk();

        /// <summary>
        /// Each wolves can talk their opinions throw this method.
        /// </summary>
        /// <returns>AIWolf protocol based whisper.</returns>
        string Whisper();

        /// <summary>
        /// Vote agent to execute.
        /// </summary>
        /// <returns></returns>
        Agent Vote();

        /// <summary>
        /// Decide agent who to be attacked by wolves.
        /// </summary>
        /// <returns></returns>
        Agent Attack();

        /// <summary>
        /// Decide agent to divine by seer.
        /// </summary>
        /// <returns></returns>
        Agent Divine();

        /// <summary>
        /// Decide agent to guard by bodyguard.
        /// </summary>
        /// <returns></returns>
        Agent Guard();

        /// <summary>
        /// Called when the game finished.
        /// <para>
        /// Before this method is called, gameinfo is updated with all information.
        /// </para>
        /// </summary>
        void Finish();
    }
}
