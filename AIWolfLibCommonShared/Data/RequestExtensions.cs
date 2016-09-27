//
// RequestExtensions.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using System.Collections.Generic;

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
            hasReturnMap[Request.NAME] = true;
            hasReturnMap[Request.ROLE] = true;
            hasReturnMap[Request.TALK] = true;
            hasReturnMap[Request.WHISPER] = true;
            hasReturnMap[Request.VOTE] = true;
            hasReturnMap[Request.DIVINE] = true;
            hasReturnMap[Request.GUARD] = true;
            hasReturnMap[Request.ATTACK] = true;
            hasReturnMap[Request.INITIALIZE] = false;
            hasReturnMap[Request.DAILY_INITIALIZE] = false;
            hasReturnMap[Request.DAILY_FINISH] = false;
            //hasReturnMap[Request.UPDATE] = false;
            hasReturnMap[Request.FINISH] = false;
            hasReturnMap[Request.DUMMY] = false;
        }

        public static bool HasReturn(this Request request)
        {
            return hasReturnMap[request];
        }
    }
}
