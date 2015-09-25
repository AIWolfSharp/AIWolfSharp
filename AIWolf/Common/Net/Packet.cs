﻿using AIWolf.Common.Data;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Packet to send data to client
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class Packet
    {
        public Request Request { get; set; }
        public GameInfoToSend GameInfo { get; set; }
        public GameSetting GameSetting { get; }

        public Packet()
        {
        }

        /// <summary>
        /// Create Packet with GameInformation
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gameInfoToSend"></param>
        public Packet(Request request, GameInfoToSend gameInfoToSend)
        {
            Request = request;
            GameInfo = gameInfoToSend;
        }

        /// <summary>
        /// Create Packet with GameInforamtion and Game Setting
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gameInfoToSend"></param>
        /// <param name="gameSetting"></param>
        public Packet(Request request, GameInfoToSend gameInfoToSend, GameSetting gameSetting)
        {
            Request = request;
            GameInfo = gameInfoToSend;
            GameSetting = gameSetting;
        }
    }
}
