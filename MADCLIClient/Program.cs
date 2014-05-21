using System;
using System.Net;
using System.Text;
using System.IO;

namespace CLIClient
{
    class Program
    {
        static int Main(string[] args)
        {
            #region vars

            IPAddress serverAddress;
            int serverPort;

            string cliInput;
            string serverResponse;

            string secureKey;
            string username;
            string passwordMD5;

            #endregion


            Console.WriteLine("CLI-Client [MINIMAL VERSION]");

            #region setup

            while (true)
            {
                while (true)
                {
                    Console.Write("\nIP: ");
                    cliInput = Console.ReadLine();

                    try
                    {
                        serverAddress = IPAddress.Parse(cliInput);
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("INVALID INPUT!");
                    }
                }

                while (true)
                {
                    Console.Write("PORT: ");
                    cliInput = Console.ReadLine();

                    try
                    {
                        serverPort = Int32.Parse(cliInput);
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("INVALID INPUT!");
                    }
                }

                Console.Write("USERNAME: ");
                username = Console.ReadLine();

                Console.Write("PASSWORD: ");
                passwordMD5 = Console.ReadLine();

                Console.Write("SECUREPASS: ");
                secureKey = Console.ReadLine();

                Console.WriteLine("ServerDestination: " + serverAddress + ":" + serverPort);
                Console.WriteLine("Username: " + username);

                Console.WriteLine("Everything right? Y/N");
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key == ConsoleKey.Y)
                {
                    CLIClient client = new CLIClient(new IPEndPoint(serverAddress, serverPort), secureKey, username, passwordMD5);
                    client.Start();

                    break;
                }

                Console.Clear();
            }

            #endregion

            Console.WriteLine("Press any key to exit program ...");
            Console.ReadKey();

            return 0;
        }
    }
}
