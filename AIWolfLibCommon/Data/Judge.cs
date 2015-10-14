using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Judge class.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class Judge
    {
        [DataMember(Name = "day")]
        public int Day { get; }

        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        [DataMember(Name = "target")]
        public Agent Target { get; }

        [DataMember(Name = "result")]
        public Species Result { get; }

        public Judge(int day, Agent agent, Agent target, Species result)
        {
            Day = day;
            Agent = agent;
            Target = target;
            Result = result;
        }

        public override string ToString()
        {
            return Agent + "->" + Target + "@" + Day + ":" + Result;
        }
    }
}
