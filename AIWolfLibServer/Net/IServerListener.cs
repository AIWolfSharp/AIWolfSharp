//
// IServerListener.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿using AIWolf.Common.Data;
using System;
using System.Net.Sockets;

namespace AIWolf.Server.Net
{
    public interface IServerListener
    {
        void Connected(TcpClient client, Agent agent, String name);
        void Unconnected(TcpClient client, Agent agent, String name);
    }
}
