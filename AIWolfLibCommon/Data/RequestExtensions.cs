using System.Collections.Generic;

namespace AIWolf.Common.Data
{
    /// <summary>
    /// Class to define extension method of enum Request.
    /// <para>
    /// Written by otsuki.
    /// </para>
    /// </summary>
    static class RequestExtensions
    {
        static Dictionary<Request, bool> hasReturnMap = new Dictionary<Request, bool>();

        static RequestExtensions()
        {
            hasReturnMap[Request.Name] = true;
            hasReturnMap[Request.Role] = true;
            hasReturnMap[Request.Talk] = true;
            hasReturnMap[Request.Whisper] = true;
            hasReturnMap[Request.Vote] = true;
            hasReturnMap[Request.Divine] = true;
            hasReturnMap[Request.Guard] = true;
            hasReturnMap[Request.Attack] = true;
            hasReturnMap[Request.Initialize] = false;
            hasReturnMap[Request.DailyInitialize] = false;
            hasReturnMap[Request.DailyFinish] = false;
            //hasReturnMap[Request.Update] = false;
            hasReturnMap[Request.Finish] = false;
        }

        public static bool HasReturn(this Request request)
        {
            return hasReturnMap[request];
        }
    }
}
