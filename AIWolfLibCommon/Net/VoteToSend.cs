//
// VoteToSend.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using System.Runtime.Serialization;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// 投票情報
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class VoteToSend
    {
        [DataMember(Name = "day")]
        public int Day { get; set; }

        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        [DataMember(Name = "target")]
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
