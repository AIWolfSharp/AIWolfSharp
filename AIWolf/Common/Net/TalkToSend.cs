using AIWolf.Common.Data;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// AI Wolf Talk
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class TalkToSend
    {
        // JSONデータの互換性のため小文字ではじめる
        /// <summary>
        /// index number of sentence
        /// </summary>
        public int idx { get; set; }

        /// <summary>
        /// told day
        /// </summary>
        public int day { get; set; }

        /// <summary>
        /// agent
        /// </summary>
        public int agent { get; set; }

        public string content { get; set; }

        public TalkToSend()
        {
        }

        public TalkToSend(Talk talk)
        {
            idx = talk.Idx;
            day = talk.Day;
            agent = talk.Agent.AgentIdx;
            content = talk.Content;
        }

        public Talk ToTalk()
        {
            return new Talk(idx, day, Agent.GetAgent(agent), content);
        }
    }
}
