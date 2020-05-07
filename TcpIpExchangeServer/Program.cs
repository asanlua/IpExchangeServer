using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TcpIpExchangeServer
{
    class Program
    {
        private const int defaultPort = 51111;

        static void Main(string[] args)
        {
            CommandLineArguments cla = CommandLineArguments.Parse(args);

            if (cla == null)
            {
                CommandLineArguments.ShowUsage();
            }

            if (cla.LocalPort != -1)
            {
                Console.WriteLine("Using local port: {0}", cla.LocalPort);
            }
            else
            {
                cla.LocalPort = defaultPort;
            }

            TcpIpExchangeServer server = new TcpIpExchangeServer(cla.LocalPort);
            Thread serverThread = new Thread(server.StartServer);
            serverThread.Start();
        }

        class CommandLineArguments
        {
            internal int LocalPort = -1;

            internal bool Verbose = false;

            static internal CommandLineArguments Parse(string[] args)
            {
                CommandLineArguments result = new CommandLineArguments();

                int i = 0;

                while (i < args.Length)
                {
                    string arg = args[i++];

                    switch (arg)
                    {
                        case "--verbose":
                            result.Verbose = true;
                            break;
                        case "--localport":
                            if (args.Length == i) return null;
                            result.LocalPort = int.Parse(args[i++]);
                            break;
                        case "help":
                            return null;
                    }
                }

                return result;
            }

            static internal void ShowUsage()
            {
                Console.WriteLine("TcpIpExchangeServer --localPort <localport> --verbose");
            }
        }
    }
}