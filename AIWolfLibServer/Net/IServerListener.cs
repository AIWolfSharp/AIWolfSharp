using AIWolf.Common.Data;
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
