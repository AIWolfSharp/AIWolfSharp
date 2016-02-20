using AIWolf.Common.Data;
using System;
using System.Text.RegularExpressions;

namespace AIWolf.Client.Lib
{
    /// <summary>
    /// 発話をパースしたもの
    /// </summary>
    public class Utterance
    {
        // 原文
        public string Text { get; }

        // 文章のトピック
        public Topic? Topic { get; }

        // 文章上の対象プレイヤー
        public Agent Target { get; }

        // カミングアウト結果や占い結果等
        private State? state;

        // TopicがAGREE,DISAGREEの時の対象発話のログの種類（囁きかどうか）
        public TalkType? TalkType { get; }

        // TopicがAGREE,DISAGREEの時の対象発話の日にち
        public int TalkDay { get; }

        // TopicがAGREE,DISAGREEの時の対象発話のID
        public int TalkID { get; }

        // TopicがESTIMATE,COMINGOUTの場合のRole．それ以外のTopicではnull
        public Role? Role
        {
            get
            {
                if (state != null)
                {
                    return state.ToRole();
                }
                else
                {
                    return null;
                }
            }
        }

        // TopicがDIVINED,INQUESTEDの場合のSpecies．それ以外のTopicではnull
        public Species? Result
        {
            get
            {
                if (state != null)
                {
                    return state.ToSpecies();
                }
                else
                {
                    return null;
                }
            }
        }

        public Utterance(string input)
        {
            TalkDay = -1;
            TalkID = -1;
            Text = input;

            // 原文を単語に分割
            string[] split = input.Split();

            // Topicの取得．Topicに存在しないならばnullが入る
            Topic = GetTopic(split[0]);

            var agentId = -1;

            if (split.Length >= 2 && split[1].StartsWith("Agent"))
            {
                agentId = GetInt(split[1]);
            }

            switch (Topic)
            {
                // 話すこと無し
                case Lib.Topic.SKIP:
                case Lib.Topic.OVER:
                    break;

                // 同意系
                case Lib.Topic.AGREE:
                case Lib.Topic.DISAGREE:
                    // Talk day4 ID38 みたいな形でくるので数字だけ取得
                    TalkType = ParseTalkType(split[1]);
                    TalkDay = GetInt(split[2]);
                    TalkID = GetInt(split[3]);
                    break;

                // "Topic Agent Role"
                case Lib.Topic.ESTIMATE:
                case Lib.Topic.COMINGOUT:
                    Target = Agent.GetAgent(agentId);
                    state = ParseState(split[2]);
                    break;

                // RESULT系
                case Lib.Topic.DIVINED:
                case Lib.Topic.INQUESTED:
                    Target = Agent.GetAgent(agentId);
                    state = ParseState(split[2]);
                    break;

                case Lib.Topic.GUARDED:
                    Target = Agent.GetAgent(agentId);
                    break;

                // 投票系
                case Lib.Topic.ATTACK:
                case Lib.Topic.VOTE:
                    Target = Agent.GetAgent(agentId);
                    break;

                default:
                    return;
            }
            return;
        }

        int GetInt(string text)
        {
            var m = new Regex(@"-?[\d]+").Match(text);
            if (m.Success)
            {
                return int.Parse(m.Value);
            }
            return -1;
        }

        /// <summary>
        /// 引数の文字列がTopicに存在するものならTopicを返す
        /// <para>
        /// 元々はTopicクラスのstaticメソッド
        /// </para>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Topic? GetTopic(string s)
        {
            foreach (Topic topic in Enum.GetValues(typeof(Topic)))
            {
                if (topic.ToString().Equals(s, StringComparison.CurrentCultureIgnoreCase))
                {
                    return topic;
                }
            }
            return null;
        }

        /// <summary>
        /// 元々はTemplateTalkFactoryクラスのstaticメソッド
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TalkType? ParseTalkType(string input)
        {
            if (input.Equals("talk", StringComparison.CurrentCultureIgnoreCase))
            {
                return Lib.TalkType.TALK;
            }
            else if (input.Equals("whisper", StringComparison.CurrentCultureIgnoreCase))
            {
                return Lib.TalkType.WHISPER;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 元々はStateクラスのstaticメソッド
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public State? ParseState(string input)
        {
            foreach (State noun in Enum.GetValues(typeof(State)))
            {
                if (noun.ToString().Equals(input, StringComparison.CurrentCultureIgnoreCase))
                {
                    return noun;
                }
            }
            return null;
        }
    }
}
