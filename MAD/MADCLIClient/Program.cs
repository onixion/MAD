using System;
using System.Net;

namespace CLIClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CLIConnect connect = new CLIConnect(IPAddress.Parse("127.0.0.1"), 9999, "admin", "bestpasswordeva", "123456");
            connect.Connect();

            Console.ReadKey();
        }
    }
}
