using AIWolf.Common.Data;
using System;
using System.Collections.Generic;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Game information which send to each player.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class GameInfoToSend
    {
        public int Day { get; set; }
        public int Agent { get; set; }

        public JudgeToSend MediumResult { get; set; }
        public JudgeToSend DivineResult { get; set; }
        public int ExecutedAgent { get; set; }
        public int AttackedAgent { get; set; }
        public int GuardedAgent { get; set; }
        public List<VoteToSend> VoteList { get; set; }
        public List<VoteToSend> AttackVoteList { get; set; }

        public List<TalkToSend> TalkList { get; set; }
        public List<TalkToSend> WhisperList { get; set; }

        public Dictionary<int, string> StatusMap { get; set; }
        public Dictionary<int, string> RoleMap { get; set; }

        public GameInfoToSend()
        {
            ExecutedAgent = -1;
            AttackedAgent = -1;
            GuardedAgent = -1;
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
