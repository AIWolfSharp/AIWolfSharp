using System.Collections.Generic;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Class to define extension mathod.
    /// <para>
    /// written by otsuki
    /// </para>
    /// </summary>
    public static class RequestExtensions
    {
        private static Dictionary<Request, bool> hasReturnMap = new Dictionary<Request, bool>();

        static RequestExtensions()
        {
            hasReturnMap.Add(Request.Name, true);
            hasReturnMap.Add(Request.Role, true);
            hasReturnMap.Add(Request.Talk, true);
            hasReturnMap.Add(Request.Whisper, true);
            hasReturnMap.Add(Request.Vote, true);
            hasReturnMap.Add(Request.Divine, true);
            hasReturnMap.Add(Request.Guard, true);
            hasReturnMap.Add(Request.Attack, true);
            hasReturnMap.Add(Request.Initialize, false);
            hasReturnMap.Add(Request.DailyInitialize, false);
            hasReturnMap.Add(Request.DailyFinish, false);
            //hasReturnMap.Add(Request.Update, true);
            hasReturnMap.Add(Request.Finish, false);
        }

        public static bool HasReturn(this Request request)
        {
            return hasReturnMap[request];
        }
    }
}
