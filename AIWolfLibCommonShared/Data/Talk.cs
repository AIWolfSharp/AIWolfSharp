//
// Talk.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using System;
using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// AI Wolf Talk.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class Talk
    {
        public const string OVER = "Over";
        public const string SKIP = "Skip";

        /// <summary>
        /// Index number of sentence.
        /// </summary>
        [DataMember(Name = "idx")]
        public int Idx { get; }

        /// <summary>
        /// Told day.
        /// </summary>
        [DataMember(Name = "day")]
        public int Day { get; }

        /// <summary>
        /// Agent.
        /// </summary>
        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        [DataMember(Name = "content")]
        public string Content { get; }

        public Talk(int idx, int day, Agent agent, string content)
        {
            Idx = idx;
            Day = day;
            Agent = agent;
            Content = content;
        }

        [DataMember(Name = "skip")]
        public bool Skip
        {
            get { return Content.Equals(SKIP); }
        }

        [DataMember(Name = "over")]
        public bool Over
        {
            get { return Content.Equals(OVER); }
        }

        public override string ToString()
        {
            return String.Format("Day{0:D2}[{1:D3}]\t{2}\t{3}", Day, Idx, Agent, Content);
        }
    }
}
