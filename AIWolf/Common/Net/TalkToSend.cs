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
        /// <summary>
        /// index number of sentence
        /// </summary>
        int idx { get; set; }
        /// <summary>
        /// told day
        /// </summary>
        int day { get; set; }
        /// <summary>
        /// agent
        /// </summary>
        int agent { get; set; }
        string content { get; set; }

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
            return new Talk(idx, day, Data.Agent.GetAgent(agent), content);
        }
    }
}
