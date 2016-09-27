//
// TalkTypeExtensions.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿namespace AIWolf.Client.Lib
{
    /// <summary>
    /// Class to define extension method.
    /// <para>
    /// Written by otsuki.
    /// </para>
    /// </summary>
    static class TalkTypeExtensions
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
