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
    class JudgeToSend
    {
        public int Day { get; set; }
        public int Agent { get; set; }
        public int Target { get; set; }
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
