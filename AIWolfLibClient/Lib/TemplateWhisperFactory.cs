//
// TemplateWhisperFactory.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;

namespace AIWolf.Client.Lib
{
    /// <summary>
    /// 人狼の囁き用の発話を生成するクラス
    /// <para>
    /// Original Java code was written by kengo,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class TemplateWhisperFactory
    {
        public static string Attack(Agent target)
        {
            string[] split = { Topic.ATTACK.ToString(), (target != null) ? target.ToString() : "null" };
            return WordAttachment(split);
        }

        public static string Estimate(Agent target, Role role)
        {
            string[] split = { Topic.ESTIMATE.ToString(), (target != null) ? target.ToString() : "null", role.ToString() };
            return WordAttachment(split);
        }

        public static string Comingout(Agent target, Role role)
        {
            string[] split = { Topic.COMINGOUT.ToString(), (target != null) ? target.ToString() : "null", role.ToString() };
            return WordAttachment(split);
        }

        public static string Divined(Agent target, Species species)
        {
            string[] split = { Topic.DIVINED.ToString(), (target != null) ? target.ToString() : "null", species.ToString() };
            return WordAttachment(split);
        }

        public static string Inquested(Agent target, Species species)
        {
            string[] split = { Topic.INQUESTED.ToString(), (target != null) ? target.ToString() : "null", species.ToString() };
            return WordAttachment(split);
        }

        public static string Guarded(Agent target)
        {
            string[] split = { Topic.GUARDED.ToString(), (target != null) ? target.ToString() : "null" };
            return WordAttachment(split);
        }

        public static string Vote(Agent target)
        {
            string[] split = { Topic.VOTE.ToString(), (target != null) ? target.ToString() : "null" };
            return WordAttachment(split);
        }

        public static string Agree(TalkType talkType, int day, int id)
        {
            string[] split = { Topic.AGREE.ToString(), talkType.ToString(), "day" + day, "ID:" + id };
            return WordAttachment(split);
        }

        public static string Disagree(TalkType talkType, int day, int id)
        {
            string[] split = { Topic.DISAGREE.ToString(), talkType.ToString(), "day" + day, "ID:" + id };
            return WordAttachment(split);
        }

        public static string Over()
        {
            return Talk.OVER;
        }

        public static string Skip()
        {
            return Talk.SKIP;
        }

        private static string WordAttachment(string[] split)
        {
            var answer = "";
            for (var i = 0; i < split.Length; i++)
            {
                answer += split[i] + " ";
            }
            return answer.Trim();
        }
    }
}
