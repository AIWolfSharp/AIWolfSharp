namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Class to define extension method.
    /// <para>
    /// written by otsuki
    /// </para>
    /// </summary>
    public static class TalkTypeExtensions
    {
        public static bool IsWhisper(this TalkType talkType)
        {
            if (talkType == TalkType.WHISPER)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
