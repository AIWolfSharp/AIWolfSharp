//
// Vote.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// 投票情報
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class Vote
    {
        [DataMember(Name = "day")]
        public int Day { get; }

        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        [DataMember(Name = "target")]
        public Agent Target { get; }

        public Vote(int day, Agent agent, Agent target)
        {
            Day = day;
            Agent = agent;
            Target = target;
        }

        public override string ToString()
        {
            return Agent + "voted " + Target + "@" + Day;
        }
    }
}
