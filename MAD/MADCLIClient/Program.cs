using System;
using System.Net;
using System.Text;

namespace CLIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CLIClient client = new CLIClient(IPAddress.Parse("127.0.0.1"), 999, "admin", "bestpasswordeva", "123456");
            client.Connect();

            client.Send(client.socket, "123456");

            if (client.Receive(client.socket) == "OK")
            {
                client.Send(client.socket, "admin");

                if (client.Receive(client.socket) == "OK")
                {
                    client.Send(client.socket, "yolo");
                }
            }
            client.socket.Close();
            Console.ReadKey();
        }
    }
}
