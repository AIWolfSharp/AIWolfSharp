using AIWolf.Common.Data;
using AIWolf.Common.Net;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace AIWolf.ClientStarter
{
    class ClientStarter
    {
        static AutoResetEvent are = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            string host = null;
            int port = -1;
            string clsName = null;
            string dllName = null;
            Role? roleRequest = null;
            string playerName = null;
            bool eternity = false;
            bool verbose = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith("-"))
                {
                    if (args[i].Equals("-p"))
                    {
                        i++;
                        port = int.Parse(args[i]);
                    }
                    else if (args[i].Equals("-e"))
                    {
                        eternity = true;
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
                        dllName = args[i];
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
                    else if (args[i].Equals("-n"))
                    {
                        i++;
                        playerName = args[i];
                    }
                    else if (args[i].Equals("-v"))
                    {
                        verbose = true;
                    }
                }
            }
            if (port < 0 || host == null || clsName == null)
            {
                Console.Error.WriteLine("Usage:" + typeof(ClientStarter).Name + " -h host -p port -c clientClass dllName (roleRequest) [-n name] [-e] [-v]");
                return;
            }

            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(dllName);
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("Can not find " + dllName);
                return;
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error in loading " + dllName);
                return;
            }

            IPlayer player;
            try
            {
                player = (IPlayer)Activator.CreateInstance(assembly.GetType(clsName));
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error in creating instance of " + clsName);
                return;
            }

            TcpipClient client = new TcpipClient(host, port, roleRequest);
            client.Completed += Client_Completed;
            if (playerName != null)
            {
                client.PlayerName = playerName;
            }

            int turn = 1;
            do
            {
                if (client.Connect(player))
                {
                    if (verbose)
                    {
                        Console.WriteLine("{0}:Player connected to server:{1}", turn, player);
                    }
                    are.WaitOne();
                    turn++;
                }
                else
                {
                    break;
                }
            }
            while (eternity);
        }

        private static void Client_Completed(object sender, EventArgs e)
        {
            are.Set();
        }
    }
}
