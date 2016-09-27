//
// TalkToSend.cs
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
    /// AI Wolf Talk.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class TalkToSend
    {
        /// <summary>
        /// Index number of sentence.
        /// </summary>
        [DataMember(Name = "idx")]
        public int Idx { get; set; }

        /// <summary>
        /// Told day.
        /// </summary>
        [DataMember(Name = "day")]
        public int Day { get; set; }

        /// <summary>
        /// Agent.
        /// </summary>
        [DataMember(Name = "agent")]
        public int Agent { get; set; }

        [DataMember(Name = "content")]
        public string Content { get; set; }

        public TalkToSend()
        {
        }

        public TalkToSend(Talk talk)
        {
            Idx = talk.Idx;
            Day = talk.Day;
            Agent = talk.Agent.AgentIdx;
            Content = talk.Content;
        }

        public Talk ToTalk()
        {
            return new Talk(Idx, Day, Data.Agent.GetAgent(Agent), Content);
        }
    }
}
