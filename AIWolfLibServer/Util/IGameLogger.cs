//
// IGameLogger.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿namespace AIWolf.Server.Util
{
    public interface IGameLogger
    {
        void Log(string log);
        void Flush();
        void Close();
    }
}
