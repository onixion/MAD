using System;
using System.Net;
using System.Text;

namespace CLIClient
{
    class Program
    {
        static int Main(string[] args)
        {
            #region vars

            string input;

            IPAddress serverAddress;
            int serverPort;
            string serverReply;

            string username;
            string password;
            string securePass;

            #endregion

            #region setup

            Console.WriteLine("CLI-Client [MINIMAL VERSION]");

            Console.Write("IP: ");
            input = Console.ReadLine();

            try
            {
                serverAddress = IPAddress.Parse(input);
            }
            catch (Exception)
            {
                Console.WriteLine("INVALID INPUT!");
                return 0;
            }

            Console.Write("PORT: ");
            input = Console.ReadLine();

            try
            {
                serverPort = Int32.Parse(input);
            }
            catch (Exception)
            {
                Console.WriteLine("INVALID INPUT!");
                return 0;
            }

            Console.Write("USERNAME: ");
            username = Console.ReadLine();

            Console.Write("PASSWORD: ");
            password = Console.ReadLine();

            Console.Write("SECUREPASS: ");
            securePass = Console.ReadLine();

            #endregion

            CLIClient client = new CLIClient(serverAddress,serverPort, username, password, securePass);
            client.Connect();

            string passwordMD5 = client.GetMD5Hash(password);

            client.Send(client.socket, securePass);

            if (client.Receive(client.socket) == "OK")
            {
                client.Send(client.socket, username);

                if (client.Receive(client.socket) == "OK")
                {
                    client.Send(client.socket, passwordMD5);

                    if (client.Receive(client.socket) == "ACCEPTED")
                    {
                        Console.WriteLine("LOGIN WAS SUCCESSFUL!");
                        Console.WriteLine("Waiting for server ...");

                        client.Send(client.socket, "GET_CLI");

                        input = client.Receive(client.socket);

                        if (input != "MODE_UNKNOWN")
                        {
                            Console.Write(input);

                            while (true)
                            {
                                input = Console.ReadLine();
                                client.Send(client.socket, input);

                                serverReply = client.Receive(client.socket);

                                if (serverReply == "DISCONNECTED")
                                    break;

                                Console.Write(serverReply);
                            }

                            Console.WriteLine("DISCONNECTED FROM SERVER!");
                        }
                        else
                        {
                            Console.WriteLine("MODE UNKNOWN!");
                            Console.ReadKey();
                            return 0;
                        }
                    }
                    else
                    {
                        Console.WriteLine("NO AUTHORISATION!");
                        Console.ReadKey();
                        return 0;
                    }


                }
                else
                {
                    Console.WriteLine("SERVER NOT RESPONDING!");
                    Console.ReadKey();
                    return 0;
                }
            }
            else
            {
                Console.WriteLine("SERVER NOT RESPONDING!");
                Console.ReadKey();
                return 0;
            }

            Console.WriteLine("Closing socket ...");
            client.socket.Close();

            Console.WriteLine("Press any key to close ...");
            Console.ReadKey();

            return 0;
        }
    }
}
