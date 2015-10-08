namespace AIWolf.Common.Data
{
    /// <summary>
    /// Guard class.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class Guard
    {
        public int Day { get; }
        public Agent Agent { get; }
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