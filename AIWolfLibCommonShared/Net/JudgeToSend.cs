//
// JudgeToSend.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using System;
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
    public class JudgeToSend
    {
        [DataMember(Name = "day")]
        public int Day { get; set; }

        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        [DataMember(Name = "target")]
        public int Target { get; set; }

        [DataMember(Name = "result")]
        public string Result { get; set; }

        public JudgeToSend()
        {
        }

        public JudgeToSend(Judge judge)
        {
            Day = judge.Day;
            Agent = judge.Agent.AgentIdx;
            Target = judge.Target.AgentIdx;
            Result = judge.Result.ToString();
            if (Result == null)
            {
                throw new AIWolfRuntimeException("judge result = null");
            }
        }

        public Judge ToJudge()
        {
            return new Judge(Day, Data.Agent.GetAgent(Agent), Data.Agent.GetAgent(Target), (Species)Enum.Parse(typeof(Species), Result));
        }
    }
}
