using AIWolf.Common.Data;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// 投票情報
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class VoteToSend
    {
        public int Day { get; set; }
        public int Agent { get; set; }
        public int Target { get; set; }

        public VoteToSend()
        {
        }

        public VoteToSend(Vote vote)
        {
            Day = vote.Day;
            Agent = vote.Agent.AgentIdx;
            Target = vote.Target.AgentIdx;
        }

        public Vote ToVote()
        {
            return new Vote(Day, Data.Agent.GetAgent(Agent), Data.Agent.GetAgent(Target));
        }
    }
}
