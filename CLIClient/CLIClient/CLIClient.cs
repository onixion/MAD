using System;
using System.Net;
using System.Net.Sockets;

namespace CLIClient
{
    public class CLIClient : TCPClient
    {
        private string cliInput;

        public CLIClient(IPAddress serverAddress, int serverPort)
        {
            this.serverAddress = serverAddress;
            this.serverPort = serverPort;
            this.serverEndPoint = new IPEndPoint(serverAddress, serverPort);
        }

        public void Run()
        {
            while (true)
            {
                Console.Write("> ");
                cliInput = Console.ReadLine();

                switch (cliInput)
                { 
                    case "set ip":
                        Console.Write("Set server-ip: ");
                        cliInput = Console.ReadLine();
                        this.serverAddress = IPAddress.Parse("cliInput");
                        break;

                    case "set port":
                        Console.Write("Set server-port: ");
                        cliInput = Console.ReadLine();
                        this.serverPort = Int32.Parse(cliInput);
                        break;
                    
                    case "connect":
                        Console.WriteLine("Connecting to server ...");
                        this.Connect("", "");
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
