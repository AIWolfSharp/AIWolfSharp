using AIWolf.Common.Data;

namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Factory to create template talk contents.
    /// <para>
    /// Original Java code was written by kengo,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class TemplateTalkFactory
    {
        /// <summary>
        /// Talk one's estimation.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string Estimate(Agent target, Role role)
        {
            string[] split = { Topic.ESTIMATE.ToString(), (target != null) ? target.ToString() : "null", role.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Comingout someone's role.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string Comingout(Agent target, Role role)
        {
            string[] split = { Topic.COMINGOUT.ToString(), (target != null) ? target.ToString() : "null", role.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Report result of divine.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="species"></param>
        /// <returns></returns>
        public static string Divined(Agent target, Species species)
        {
            string[] split = { Topic.DIVINED.ToString(), (target != null) ? target.ToString() : "null", species.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Report result of inquest.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="species"></param>
        /// <returns></returns>
        public static string Inquested(Agent target, Species species)
        {
            string[] split = { Topic.INQUESTED.ToString(), (target != null) ? target.ToString() : "null", species.ToString() };
            return WordAttachment(split);
        }

        /// <summary>
        /// Report guarded agent.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Guarded(Agent target)
        {
            string[] split = { Topic.GUARDED.ToString(), (target != null) ? target.ToString() : "null" };
            return WordAttachment(split);
        }

        /// <summary>
        /// Declare vote target.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
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
