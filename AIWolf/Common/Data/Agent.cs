using System;
using System.Collections.Generic;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Player Agent.
    /// Each players can identify other players as Agent.
    /// Each agent has unique index.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class Agent : IComparable<Agent>
    {
        private static Dictionary<int, Agent> agentIndexMap = new Dictionary<int, Agent>();

        static public Agent GetAgent(int idx)
        {
            if (idx < 0)
            {
                return null;
            }
            if (!agentIndexMap.ContainsKey(idx))
            {
                Agent agent = new Agent(idx);
                agentIndexMap.Add(idx, agent);
            }
            return agentIndexMap[idx];
        }

        // JSONデータ互換性のため小文字で始める
        public int agentIdx { get; }

        private Agent(int idx)
        {
            agentIdx = idx;
        }

        public override string ToString()
        {
            return string.Format("Agent[{0:00}]", agentIdx);
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + agentIdx;
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }
            Agent other = (Agent)obj;
            if (agentIdx != other.agentIdx)
            {
                return false;
            }
            return true;
        }

        public int CompareTo(Agent target)
        {
            if (target == null)
            {
                return 1;
            }
            return agentIdx - target.agentIdx;
        }
    }
}
