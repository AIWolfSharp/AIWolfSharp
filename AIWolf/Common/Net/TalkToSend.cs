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
        public int Idx { get; set; }

        /// <summary>
        /// told day
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// agent
        /// </summary>
        public int Agent { get; set; }

        public string Content { get; set; }

        public TalkToSend()
        {
        }

        public TalkToSend(Talk talk)
        {
            Idx = talk.Idx;
            Day = talk.Day;
            Agent = talk.Agent.AgentIdx;
            Content = talk.Content;
        }

        public Talk ToTalk()
        {
            return new Talk(Idx, Day, Data.Agent.GetAgent(Agent), Content);
        }
    }
}
