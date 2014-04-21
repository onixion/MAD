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

            IPAddress serverAddress;
            int serverPort;

            string input;

            string username;
            string password;
            string securePass;

            string serverRequest;
            string serverResponse;

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

            CLIClient clientCLI = new CLIClient(new IPEndPoint(serverAddress, serverPort), securePass, username, password);
            clientCLI.sConnect(clientCLI.serverSocket, clientCLI.serverEndPoint);

            if (clientCLI.serverSocket.Connected == true)
            {
                clientCLI.Send(clientCLI.serverSocket, clientCLI.securePass);

                //Console.WriteLine(clientCLI.Receive(clientCLI.serverSocket));

                if (clientCLI.Receive(clientCLI.serverSocket) == "OK")
                {
                    clientCLI.Send(clientCLI.serverSocket, clientCLI.username);

                    if (clientCLI.Receive(clientCLI.serverSocket) == "OK")
                    {
                        clientCLI.Send(clientCLI.serverSocket, clientCLI.passwordMD5);

                        if (clientCLI.Receive(clientCLI.serverSocket) == "ACCEPTED")
                        {
                            Console.WriteLine("LOGIN WAS SUCCESSFUL!");
                            Console.WriteLine("Waiting for server ...");

                            clientCLI.Send(clientCLI.serverSocket, "GET_CLI");

                            input = clientCLI.Receive(clientCLI.serverSocket);

                            if (input != "MODE_UNKNOWN")
                            {
                                Console.Write(input);

                                while (true)
                                {
                                    input = Console.ReadLine();
                                    clientCLI.Send(clientCLI.serverSocket, input);

                                    serverResponse = clientCLI.Receive(clientCLI.serverSocket);

                                    if (serverResponse == "DISCONNECTED")
                                        break;

                                    Console.Write(serverResponse);
                                }

                                Console.WriteLine("DISCONNECTED FROM SERVER!");
                            }
                            else
                            {
                                Console.WriteLine("MODE NOT KNOW BY SERVER!");
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
                clientCLI.serverSocket.Close();
            }
            else
            {
                Console.WriteLine("Could not connect to server ...");
            }

            Console.WriteLine("Press any key to close ...");
            Console.ReadKey();

            return 0;
        }
    }
}
