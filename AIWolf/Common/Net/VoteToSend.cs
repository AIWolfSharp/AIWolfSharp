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
    public class VoteToSend
    {
        // JSONデータの互換性のため小文字ではじめる
        public int day { get; set; }
        public int agent { get; set; }
        public int target { get; set; }

        public VoteToSend()
        {
        }

        public VoteToSend(Vote vote)
        {
            day = vote.Day;
            agent = vote.Agent.agentIdx;
            target = vote.Target.agentIdx;
        }

        public Vote ToVote()
        {
            Vote vote = new Vote(day, Agent.GetAgent(agent), Agent.GetAgent(target));
            return vote;
        }
    }
}
