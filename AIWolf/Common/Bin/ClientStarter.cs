using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;

namespace AIWolf.Common.Bin
{
    class ClientStarter
    {
        public static void Main(string[] args)
        {
            string host = null;
            int port = -1;
            string clsName = null;
            Role? roleRequest = null; // Nullable

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i].Equals("-p"))
                    {
                        i++;
                        port = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-h"))
                    {
                        i++;
                        host = args[i];
                    }
                    else if (args[i].Equals("-c"))
                    {
                        i++;
                        clsName = args[i];
                        i++;
                        try
                        {
                            if (i > args.Length - 1 || args[i].StartsWith("-")) // Roleでない
                            {
                                i--;
                                roleRequest = null;
                                continue;
                            }
                            roleRequest = (Role)Enum.Parse(typeof(Role), args[i]);
                        }
                        catch (ArgumentException)
                        {
                            Console.Error.WriteLine("No such role as " + args[i]);
                            return;
                        }
                    }
                }
            }
            if (port < 0 || host == null || clsName == null)
            {
                Console.Error.WriteLine("Usage:" + typeof(ClientStarter) + " -h host -p port -c clientClass");
                return;
            }
            IPlayer player = (IPlayer)Activator.CreateInstance(Type.GetType(clsName));
            TcpipClient client = new TcpipClient(host, port, roleRequest);
            if (client.Connect(player))
            {
                Console.WriteLine("Player connected to server:" + player);
            }
        }
    }
}
