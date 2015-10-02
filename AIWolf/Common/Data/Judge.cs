namespace AIWolf.Common.Data
{
    /// <summary>
    /// Judge class.
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    class Judge
    {
        public int Day { get; }
        public Agent Agent { get; }
        public Agent Target { get; }
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
