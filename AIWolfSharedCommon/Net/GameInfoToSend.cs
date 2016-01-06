using AIWolf.Common.Data;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information which send to each player.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class GameInfoToSend
    {
        [DataMember(Name = "day")]
        public int Day { get; set; }

        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        [DataMember(Name = "mediumResult")]
        public JudgeToSend MediumResult { get; set; }

        [DataMember(Name = "divineResult")]
        public JudgeToSend DivineResult { get; set; }

        [DataMember(Name = "executedAgent")]
        public int ExecutedAgent { get; set; } = -1;

        [DataMember(Name = "attackedAgent")]
        public int AttackedAgent { get; set; } = -1;

        [DataMember(Name = "guardedAgent")]
        public int GuardedAgent { get; set; } = -1;

        [DataMember(Name = "voteList")]
        public List<VoteToSend> VoteList { get; set; }

        [DataMember(Name = "attackVoteList")]
        public List<VoteToSend> AttackVoteList { get; set; }

        [DataMember(Name = "talkList")]
        public List<TalkToSend> TalkList { get; set; }

        [DataMember(Name = "whisperList")]
        public List<TalkToSend> WhisperList { get; set; }

        [DataMember(Name = "statusMap")]
        public Dictionary<int, string> StatusMap { get; set; }

        [DataMember(Name = "roleMap")]
        public Dictionary<int, string> RoleMap { get; set; }

        public GameInfoToSend()
        {
            VoteList = new List<VoteToSend>();
            AttackVoteList = new List<VoteToSend>();
            StatusMap = new Dictionary<int, string>();
            RoleMap = new Dictionary<int, string>();
            TalkList = new List<TalkToSend>();
            WhisperList = new List<TalkToSend>();
        }

        public GameInfo ToGameInfo()
        {
            GameInfo gi = new GameInfo();
            gi.Day = Day;
            gi.Agent = Data.Agent.GetAgent(Agent);

            if (MediumResult != null)
            {
                gi.MediumResult = MediumResult.ToJudge();
            }
            if (DivineResult != null)
            {
                gi.DivineResult = DivineResult.ToJudge();
            }
            gi.ExecutedAgent = Data.Agent.GetAgent(ExecutedAgent);
            gi.AttackedAgent = Data.Agent.GetAgent(AttackedAgent);
            gi.GuardedAgent = Data.Agent.GetAgent(GuardedAgent);

            gi.VoteList = new List<Vote>();
            foreach (VoteToSend vote in VoteList)
            {
                gi.VoteList.Add(vote.ToVote());
            }
            gi.AttackVoteList = new List<Vote>();
            foreach (VoteToSend vote in AttackVoteList)
            {
                gi.AttackVoteList.Add(vote.ToVote());
            }

            gi.TalkList = new List<Talk>();
            foreach (TalkToSend talk in TalkList)
            {
                gi.TalkList.Add(talk.ToTalk());
            }
            gi.WhisperList = new List<Talk>();
            foreach (TalkToSend whisper in WhisperList)
            {
                gi.WhisperList.Add(whisper.ToTalk());
            }

            gi.StatusMap = new Dictionary<Agent, Status>();
            foreach (int agent in StatusMap.Keys)
            {
                gi.StatusMap.Add(Data.Agent.GetAgent(agent), (Status)Enum.Parse(typeof(Status), StatusMap[agent]));
            }
            gi.RoleMap = new Dictionary<Agent, Role>();
            foreach (int agent in RoleMap.Keys)
            {
                gi.RoleMap.Add(Data.Agent.GetAgent(agent), (Role)Enum.Parse(typeof(Role), RoleMap[agent]));
            }

            return gi;
        }
    }
}
