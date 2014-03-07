using System;
using System.Net.Sockets;
using System.Net;

namespace PortScannerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // scans all ports from 0 to 81
            int startPort = 0;
            int endPort = 81;

            IPAddress targetIP = IPAddress.Parse("127.0.0.1");
            IPEndPoint target;

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            for (int i = startPort; i < endPort; i++)
            {
                target = new IPEndPoint(targetIP, i);

                try
                {
                    socket.Connect(target);
                    Console.WriteLine("PORT " + i + " open.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("PORT " + i + " closed.");
                }
            }

            socket.Close();

            Console.WriteLine();
            Console.WriteLine("Press any key to close program ... ");
            Console.ReadKey();
        }
    }
}
