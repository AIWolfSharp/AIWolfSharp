using AIWolf.Common.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Packet to send data to client.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class Packet
    {
        [DataMember(Name = "request")]
        public Request Request { get; set; }

        [DataMember(Name = "gameInfo")]
        public GameInfoToSend GameInfo { get; set; }

        [DataMember(Name = "gameSetting")]
        public GameSetting GameSetting { get; }

        [DataMember(Name = "talkHistory")]
        public List<TalkToSend> TalkHistory { get; set; }

        [DataMember(Name = "whisperHistory")]
        public List<TalkToSend> WhisperHistory { get; set; }

        public Packet()
        {
        }

        public Packet(Request request)
        {
            Request = request;
        }

        /// <summary>
        /// Create packet with game information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gameInfoToSend"></param>
        public Packet(Request request, GameInfoToSend gameInfoToSend)
        {
            Request = request;
            GameInfo = gameInfoToSend;
        }

        /// <summary>
        /// Create packet with game inforamtion and game setting.
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

        public Packet(Request request, List<TalkToSend> talkHistoryList, List<TalkToSend> whisperHistoryList)
        {
            Request = request;
            TalkHistory = talkHistoryList;
            WhisperHistory = whisperHistoryList;
        }
    }
}
