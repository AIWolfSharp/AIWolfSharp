using AIWolf.Common.Data;
using AIWolf.Common.Net;
using AIWolf.Server;
using AIWolf.Server.Net;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AIWolf.TcpipAgentTester
{
    /// <summary>
    /// TCP/IP版エージェントテスター
    /// </summary>
    class TcpipAgentTester
    {
        // クライアントが終了したことを通知する
        static AutoResetEvent are = new AutoResetEvent(false);

        static string host = "localhost";
        static int port = 10000;

        static void Usage()
        {
            Console.Error.WriteLine("Usage:" + typeof(TcpipAgentTester) + " -c clientClass dllName (-h host) (-p port) (-o)");
        }

        static void Main(string[] args)
        {
            string clsName = null;
            string dllName = null;
            bool clientOnly = false;

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
                        dllName = args[i];
                        i++;
                    }
                    else if (args[i].Equals("-o"))
                    {
                        clientOnly = true;
                    }
                }
            }

            if (port < 0 || host == null || clsName == null || dllName == null)
            {
                Usage();
                Environment.Exit(0);
            }

            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFrom(dllName);
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("Can not find {0}.", dllName);
                Environment.Exit(0);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error in loading {0}.", dllName);
                Environment.Exit(0);
            }
            Type playerType = assembly.GetType(clsName);
            if (playerType == null)
            {
                Console.Error.WriteLine("Can not get Type of {0} from {1}.", clsName, dllName);
                Environment.Exit(0);
            }

            if (clientOnly)
            {
                foreach (Role requestRole in Enum.GetValues(typeof(Role)))
                {
                    if (requestRole == Role.FREEMASON)
                    {
                        continue;
                    }
                    IPlayer player = null;
                    try
                    {
                        player = (IPlayer)Activator.CreateInstance(playerType);
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Error in creating instance of {0}.", args[1]);
                        Environment.Exit(0);
                    }
                    Console.WriteLine("Role:" + requestRole);
                    TcpipClient client = new TcpipClient("localhost", port, requestRole);
                    client.Completed += Client_Completed;
                    client.Connect(player);
                    are.WaitOne();
                }
            }
            else
            {
                Console.WriteLine("Start AIWolf Server port:{0} playerNum:{1}", port, 15);
                GameSetting gameSetting = GameSetting.GetDefaultGame(15);
                TcpipServer gameServer = new TcpipServer(port, 15, gameSetting);

                foreach (Role requestRole in Enum.GetValues(typeof(Role)))
                {
                    if (requestRole == Role.FREEMASON)
                    {
                        continue;
                    }
                    IPlayer player = null;
                    try
                    {
                        player = (IPlayer)Activator.CreateInstance(playerType);
                    }
                    catch (Exception)
                    {
                        Console.Error.WriteLine("Error in creating instance of {0}.", args[1]);
                        Environment.Exit(0);
                    }
                    TcpipClient client = new TcpipClient("localhost", port, requestRole);
                    Task task = Task.Run(() =>
                    {
                        gameServer.WaitForConnection();
                    });
                    client.Connect(player);
                    for (int i = 0; i < 14; i++)
                    {
                        client = new TcpipClient("localhost", port, null);
                        client.Connect(new RandomPlayer());
                    }
                    task.Wait();
                    for (int j = 0; j < 10; j++)
                    {
                        AIWolfGame game = new AIWolfGame(gameSetting, gameServer);
                        game.SetRand(new Random());
                        game.Start();
                    }
                    gameServer.Close();
                }
            }
        }

        private static void Client_Completed(object sender, EventArgs e)
        {
            are.Set();
        }
    }
}