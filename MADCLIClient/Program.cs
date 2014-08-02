using System;
using System.Net;
using System.Text;
using System.IO;

using MadNet;

namespace CLIClient
{
    class Program
    {
        static int Main(string[] args)
        {
            #region vars

            Version version = new Version(1, 0);

            IPAddress serverAddress;
            int serverPort;

            string cliInput;

            string username;
            string passwordMD5 = "";

            #endregion

            Console.WriteLine("MAD-CLI-Client (Version " + version + ")");

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

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        passwordMD5 += key.KeyChar;
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && passwordMD5.Length > 0)
                        {
                            passwordMD5 = passwordMD5.Substring(0, passwordMD5.Length - 1);
                        }
                        else
                        {
                            if (key.Key == ConsoleKey.Enter)
                            {
                                break;
                            }
                        }
                    }
                }

                passwordMD5 = MD5Hashing.GetHash(passwordMD5);

                Console.WriteLine("\n\nConnect to '" + serverAddress + ":" + serverPort + "' with the username '" + username + "'?");
                Console.WriteLine("Sure about that? Y/N");

                ConsoleKeyInfo key2 = Console.ReadKey();
                Console.WriteLine("\n\n");

                if (key2.Key == ConsoleKey.Y)
                {
                    CLIClient _client = new CLIClient(new IPEndPoint(serverAddress, serverPort));

                    try
                    {
                        _client.Connect();
                        _client.LoginToRemoteCLI(username, passwordMD5);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("EXCEPTION: " + e.Message);
                    }
                }

                Console.Clear();
            }

            #endregion

            Console.WriteLine("\nPress any key to exit program ...");
            Console.ReadKey();

            return 0;
        }
    }
}
