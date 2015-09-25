using AIWolf.Common.Data;
using System;
using System.Collections.Generic;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information which send to each player
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class GameInfoToSend
    {
        // JSONデータの互換性のため小文字ではじめる
        public int day { get; set; }
        public int agent { get; set; }

        public JudgeToSend mediumResult { get; set; }
        public JudgeToSend divineResult { get; set; }
        public int executedAgent { get; set; }
        public int attackedAgent { get; set; }
        public int guardedAgent { get; set; }
        public List<VoteToSend> voteList { get; set; }
        public List<VoteToSend> attackVoteList { get; set; }

        public List<TalkToSend> talkList { get; set; }
        public List<TalkToSend> whisperList { get; set; }

        public Dictionary<string, string> statusMap { get; set; }
        public Dictionary<string, string> roleMap { get; set; }

        public GameInfoToSend()
        {
            executedAgent = -1;
            attackedAgent = -1;
            guardedAgent = -1;
            voteList = new List<VoteToSend>();
            attackVoteList = new List<VoteToSend>();
            statusMap = new Dictionary<string, string>();
            roleMap = new Dictionary<string, string>();
            talkList = new List<TalkToSend>();
            whisperList = new List<TalkToSend>();
        }

        public GameInfo ToGameInfo()
        {
            GameInfo gi = new GameInfo();
            gi.Day = day;
            gi.Agent = Agent.GetAgent(agent);

            if (mediumResult != null)
            {
                gi.MediumResult = mediumResult.ToJudge();
            }
            if (divineResult != null)
            {
                gi.DivineResult = divineResult.ToJudge();
            }
            gi.ExecutedAgent = Agent.GetAgent(executedAgent);
            gi.AttackedAgent = Agent.GetAgent(attackedAgent);
            gi.GuardedAgent = Agent.GetAgent(guardedAgent);

            gi.VoteList = new List<Vote>();
            foreach (VoteToSend vote in voteList)
            {
                gi.VoteList.Add(vote.ToVote());
            }
            gi.AttackVoteList = new List<Vote>();
            foreach (VoteToSend vote in attackVoteList)
            {
                gi.AttackVoteList.Add(vote.ToVote());
            }

            gi.TalkList = new List<Talk>();
            foreach (TalkToSend talk in talkList)
            {
                gi.TalkList.Add(talk.ToTalk());
            }
            gi.WhisperList = new List<Talk>();
            foreach (TalkToSend whisper in whisperList)
            {
                gi.WhisperList.Add(whisper.ToTalk());
            }

            gi.StatusMap = new Dictionary<Agent, Status>();
            foreach (string agent in statusMap.Keys)
            {
                gi.StatusMap.Add(Agent.GetAgent(int.Parse(agent)), (Status)Enum.Parse(typeof(Status), statusMap[agent]));
            }
            gi.RoleMap = new Dictionary<Agent, Role>();
            foreach (string agent in roleMap.Keys)
            {
                gi.RoleMap.Add(Agent.GetAgent(int.Parse(agent)), (Role)Enum.Parse(typeof(Role), roleMap[agent]));
            }

            return gi;
        }
    }
}
