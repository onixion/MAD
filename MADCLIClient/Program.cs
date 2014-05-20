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

            string username;
            string password;
            string secureKey;

            string cryptoKey;

            #endregion


            Console.WriteLine("CLI-Client [MINIMAL VERSION]");
            Console.WriteLine("______SETUP_________________\n");

            #region setup

            #region cryptoKey loading

            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");

            Console.WriteLine("Loading crypto-key ...");

            if(File.Exists(Path.Combine("data","cryptoKey.key")))
            {
                Console.WriteLine("Crypto-key found!");

                using (FileStream file = new FileStream(Path.Combine("data", "cryptoKey.key"), FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader reader = new StreamReader(file))
                    cryptoKey = reader.ReadToEnd();

                Console.WriteLine("Crypto-key: " + cryptoKey);
            }
            else
            {
                Console.WriteLine("No crypto-key found. Please insert the crypto key here: ");
                cryptoKey = Console.ReadLine();

                using (FileStream file = new FileStream(Path.Combine("data", "cryptoKey.key"), FileMode.Create, FileAccess.Write, FileShare.Write))
                using (StreamWriter writer = new StreamWriter(file))
                    writer.WriteLine(cryptoKey);

                Console.WriteLine("Crypto-file created.");
            }

            #endregion

            Console.Write("\nIP: ");
            cliInput = Console.ReadLine();

            try
            {
                serverAddress = IPAddress.Parse(cliInput);
            }
            catch (Exception)
            {
                Console.WriteLine("INVALID INPUT!");
                return 0;
            }

            Console.Write("PORT: ");
            cliInput = Console.ReadLine();

            try
            {
                serverPort = Int32.Parse(cliInput);
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
            secureKey = Console.ReadLine();

            #endregion

            while (true)
            {
                // init client
                CLIClient clientCLI = new CLIClient(new IPEndPoint(serverAddress, serverPort), secureKey, username, password);

                // try connect
                if (!clientCLI.sConnect(clientCLI.serverSocket, clientCLI.serverEndPoint)) { break; }

                if (clientCLI.serverSocket.Connected)
                {
                    if (!clientCLI.Send(clientCLI.serverSocket, clientCLI.securePass)) { break; }

                    if (clientCLI.Receive(clientCLI.serverSocket) == "OK")
                    {
                        if (!clientCLI.Send(clientCLI.serverSocket, clientCLI.username)) { break; }

                        if (clientCLI.Receive(clientCLI.serverSocket) == "OK")
                        {
                            if (!clientCLI.Send(clientCLI.serverSocket, clientCLI.passwordMD5)) { break; }

                            if (clientCLI.Receive(clientCLI.serverSocket) == "ACCEPTED")
                            {
                                Console.WriteLine("LOGIN WAS SUCCESSFUL!");
                                Console.WriteLine("Waiting for server ...");

                                if (!clientCLI.Send(clientCLI.serverSocket, "GET_CLI")) { break; }

                                cliInput = clientCLI.Receive(clientCLI.serverSocket);

                                if(cliInput != null)
                                if (cliInput != "MODE_UNKNOWN")
                                {
                                    Console.Write(cliInput);

                                    while (true)
                                    {
                                        cliInput = Console.ReadLine();
                                        if (!clientCLI.Send(clientCLI.serverSocket, cliInput)) { break; }

                                        serverResponse = clientCLI.Receive(clientCLI.serverSocket);

                                        if (serverResponse == "DISCONNECTED" || serverResponse == null)
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

                }
                else
                {
                    Console.WriteLine("Could not connect to server ...");
                }


                Console.WriteLine("Disconnecting ...");
                clientCLI.sDisconnect(clientCLI.serverSocket, clientCLI.serverEndPoint);

                Console.WriteLine("Closing socket ...");
                clientCLI.serverSocket.Close();
            }

            Console.WriteLine("Press any key to exit program ...");
            Console.ReadKey();

            return 0;
        }
    }
}
