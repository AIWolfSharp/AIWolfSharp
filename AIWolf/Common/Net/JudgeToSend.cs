using AIWolf.Common.Data;
using System;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// 投票情報
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class JudgeToSend
    {
        public int day { get; set; }
        public int agent { get; set; }
        public int target { get; set; }
        public string result { get; set; }

        public JudgeToSend()
        {
        }

        public JudgeToSend(Judge judge)
        {
            day = judge.Day;
            agent = judge.Agent.AgentIdx;
            target = judge.Target.AgentIdx;
            result = judge.Result.ToString();
            if (result == null)
            {
                throw new AIWolfRuntimeException("judge result = null");
            }
        }

        public Judge ToJudge()
        {
            Judge judge = new Judge(day, Data.Agent.GetAgent(agent), Data.Agent.GetAgent(target), (Species)Enum.Parse(typeof(Species), result));
            return judge;
        }
    }
}
