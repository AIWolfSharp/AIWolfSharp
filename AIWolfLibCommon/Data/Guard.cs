using System.Runtime.Serialization;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Guard class.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    [DataContract]
    public class Guard
    {
        [DataMember(Name = "day")]
        public int Day { get; }

        [DataMember(Name = "agent")]
        public Agent Agent { get; }

        [DataMember(Name = "target")]
        public Agent Target { get; }

        public Guard(int day, Agent agent, Agent target)
        {
            Day = day;
            Agent = agent;
            Target = target;
        }

        public override string ToString()
        {
            return Agent + " guarded " + Target + "@" + Day;
        }
    }
}