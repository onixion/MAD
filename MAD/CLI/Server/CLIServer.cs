using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SocketFramework;

namespace MAD
{
    public class CLIServer : SocketServer
    {
        private string cryptoKey = "asdf";
        private string secureKey = "1234";

        public CLIServer(int port)
        {
        }

        public override void HandleClient(Socket socket)
        {
            // WORKING ON THIS!

            /*
            // get ip-endpoint from client
            IPEndPoint client = (IPEndPoint)socket.RemoteEndPoint;

            // log request to console
            Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") connected.");

            // reveive sercurePass
            string receivedSecurePass = Receive(socket);
            Send(socket, "OK");

            // reveive username
            string username = Receive(socket);
            Send(socket, "OK");

            // reveive passwordMD5
            string passwordMD5 = Receive(socket);

            if (this.secureKey == receivedSecurePass)
            {
                if (CheckUsernameAndPassword(username, passwordMD5))
                {
                    // client accepted
                    Send(socket, "ACCEPTED");
                    Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") login as '" + username + "'.");

                    /* After the client login to the cli server,
                     * he need to send the mode he want to enter.
                     * For the default cli, he needs to send GET_CLI.
                     * 

                    string mode = Receive(socket);

                    switch (mode)
                    {
                        case "GET_CLI":
                            CLIServerInternal cli = new CLIServerInternal(socket);
                            cli.Start();
                            break;
                        // other modes ...

                        default:
                            Send(socket, "MODE_UNKNOWN");
                            break;
                    }
                }
                else
                {
                    Send(socket, "DENIED");
                    Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") failed to login. Username or password wrong.");
                }
            }
            else
            {
                Send(socket, "DENIED");
                Console.WriteLine(GetTimeStamp() + " Client (" + client.Address + ") failed to login. SecurePass wrong.");
            }
             * */
        }
    }
}
