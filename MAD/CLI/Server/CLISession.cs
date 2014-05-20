using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using SocketFramework;

namespace MAD.CLI
{
    public class CLISession : CLIFramework
    {
        // network vars
        private Socket clientSocket;
        private IPEndPoint clientEndPoint;

        // --------------------------------------------------------
        //          CLI Session
        // --------------------------------------------------------

        public CLISession(Socket clientSocket)
        {
            InitCLI();
            this.clientSocket = clientSocket;
            this.clientEndPoint = (IPEndPoint) clientSocket.RemoteEndPoint;

            // start session
            this.Start();
        }

        public override void Start()
        {
            //send cursor for the first time
            MadComponents.components.sfNode.Send(clientSocket, "\n" + cursor);

            while (true)
            {
                // cli waiting for input from socket
                cliInput = MadComponents.components.sfNode.Receive(clientSocket);

                // LOGGER HERE

                // check if client wants to disconnect
                if (cliInput == "DISCONNECT")
                {
                    // let the client know that he disconnected
                    MadComponents.components.sfNode.Send(clientSocket, "DISCONNECTED");
                    break;
                }

                // check if client lost connection
                if (cliInput != null)
                {
                    if (cliInput != "")
                    {
                        // get command
                        commandInput = GetCommand(cliInput);

                        // check if command are known
                        if (CommandExists(commandInput))
                        {
                            // get arguments from input
                            parameterInput = GetParamtersFromInput(cliInput);
                            // get command type
                            inputCommandType = GetCommandType(commandInput);

                            // create command object (pass the command none objects)
                            command = (Command)inputCommandType.GetConstructor(new Type[0]).Invoke(new object[0]);

                            // check if the arguments are valid (string = VALID_PARAMETER_YES)
                            parameterValid = command.ValidParameters(parameterInput);

                            if (parameterValid == "VALID_PARAMETER")
                            {
                                // set command parameters 
                                command.SetParameters(parameterInput);

                                // EXECUTE COMMAND AND SEND OUTPUT
                                MadComponents.components.sfNode.Send(clientSocket, command.Execute() + "\n" + cursor);
                            }
                            else
                            {
                                MadComponents.components.sfNode.Send(clientSocket, parameterValid + "\n" + cursor);
                            }
                        }
                        else
                        {
                            MadComponents.components.sfNode.Send(clientSocket, "Command '" + commandInput + "' unknown! Type 'help' for more information." + "\n" + cursor);
                        }
                    }
                    else
                    {
                        MadComponents.components.sfNode.Send(clientSocket, "\n" + cursor);
                    }
                }
                else
                {
                    break;
                }

                // check if the server has stopped
                if (MadComponents.components.cliServer.serverStopRequest == true)
                {
                    break;
                }
            }

            clientSocket.Close();

            Console.WriteLine(" Client (" + clientEndPoint.Address + ") disconnected.");
        }
    }
}
