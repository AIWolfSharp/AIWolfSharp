using System;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// AI Wolf Talk
    /// <para>
    /// Original Java code was written by tori,
    /// and translated into C# by otsuki.
    /// </para>
    /// </summary>
    public class Talk
    {
        public int Idx { get; }
        public int Day { get; }
        public Agent Agent { get; }
        public string Content { get; }

        public Talk(int idx, int day, Agent agent, string content)
        {
            Idx = idx;
            Day = day;
            Agent = agent;
            Content = content;
        }

        public override string ToString()
        {
            return String.Format("Day{0:D2}[{1:D3}]\t{2}\t{3}", Day, Idx, Agent, Content);
        }

        public const string OVER = "Over";
        public const string SKIP = "Skip";

        public bool IsSkip()
        {
            return Content.Equals(SKIP);
        }

        public bool IsOver()
        {
            return Content.Equals(OVER);
        }
    }
}
