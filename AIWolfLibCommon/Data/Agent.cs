using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
    [DataContract]
    public class Agent : IComparable<Agent>
    {
        static Dictionary<int, Agent> agentIndexMap = new Dictionary<int, Agent>();

        /// <summary>
        /// Get agent of idx.
        /// </summary>
        /// <param name="idx">agent's idx</param>
        /// <returns>agent</returns>
        public static Agent GetAgent(int idx)
        {
            if (idx < 0)
            {
                return null;
            }
            if (!agentIndexMap.ContainsKey(idx))
            {
                agentIndexMap[idx] = new Agent(idx);
            }
            return agentIndexMap[idx];
        }

        [DataMember(Name = "agentIdx")]
        public int AgentIdx { get; }

        /// <summary>
        /// Create new agent.
        /// </summary>
        /// <param name="idx"></param>
        private Agent(int idx)
        {
            AgentIdx = idx;
        }

        public override string ToString()
        {
            return string.Format("Agent[{0:00}]", AgentIdx);
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + AgentIdx;
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
            if (AgentIdx != other.AgentIdx)
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
            return AgentIdx - target.AgentIdx;
        }
    }
}
