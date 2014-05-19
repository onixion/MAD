using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SocketFramework;
using System.Text;

namespace MAD.CLI
{
    public class CLIServerInternal : CLIFramework
    {
        // network vars
        private Socket clientSocket;
        private IPEndPoint clientEndPoint;

        // --------------------------------------------------------
        //          CLI ServerINTERNAL
        // --------------------------------------------------------

        public CLIServerInternal(Socket clientSocket)
        {
            /*InitCommands();
            this.clientSocket = clientSocket;
            this.clientEndPoint = (IPEndPoint) clientSocket.RemoteEndPoint;*/
        }

        public override void Start()
        {
            // WORKING ON THIS!

            /*
            //send cursor for the first time
            Send(clientSocket, "\n" + cursor);

            while (true)
            {
                // cli waiting for input from socket
                cliInput = Receive(clientSocket);

                // LOGGER HERE

                // check if client wants to disconnect
                if (cliInput == "DISCONNECT")
                {
                    // let the client know that he disconnected
                    Send(clientSocket, "DISCONNECTED");
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
                                Send(clientSocket, command.Execute() + "\n" + cursor);
                            }
                            else
                                Send(clientSocket, parameterValid + "\n" + cursor);
                        }
                        else
                            Send(clientSocket,"Command '" + commandInput + "' unknown! Type 'help' for more information." + "\n" + cursor);
                    }
                    else
                        Send(clientSocket,"\n" + cursor);
                }
                else
                    break;

                // check if the server has stopped
                if (MadComponents.components.cliServer.serverStopRequest == true)
                    break;
            }

            clientSocket.Close();

            Console.WriteLine(" Client (" + clientEndPoint.Address + ") disconnected.");
             * */
        }
    }
}
