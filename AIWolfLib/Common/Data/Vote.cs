namespace AIWolf.Common.Data
{
    /// <summary>
    /// 投票情報
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class Vote
    {
        public int Day { get; }
        public Agent Agent { get; }
        public Agent Target { get; }

        public Vote(int day, Agent agent, Agent target)
        {
            Day = day;
            Agent = agent;
            Target = target;
        }

        public override string ToString()
        {
            return Agent + " voted " + Target + "@" + Day;
        }
    }
}
