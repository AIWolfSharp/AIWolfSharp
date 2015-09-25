using AIWolf.Common.Data;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// vote information
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class VoteToSend
    {
        public int day { get; set; }
        public int agent { get; set; }
        public int target { get; set; }

        public VoteToSend()
        {
        }

        public VoteToSend(Vote vote)
        {
            day = vote.Day;
            agent = vote.Agent.AgentIdx;
            target = vote.Target.AgentIdx;
        }

        public Vote ToVote()
        {
            Vote vote = new Vote(day, Data.Agent.GetAgent(agent), Data.Agent.GetAgent(target));
            return vote;
        }
    }
}
