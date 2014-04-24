using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SocketFramework;
using System.Text;
namespace MAD
{
    public class CLI : SocketFramework.SocketFramework
    {
        // network vars
        private Socket clientSocket;
        private IPEndPoint clientEndPoint;

        // cli vars
        public string cursor = "=> ";

        // cli input vars
        private string cliInput;
        private string commandInput;
        private ParameterInput parameterInput;
        private string parameterValid;

        // cli command vars
        private List<CommandOptions> commandOptions;
        private Command command;
        private Type inputCommandType;

        // --------------------------------------------------------
        //          CLI Framework
        // --------------------------------------------------------

        public CLI(Socket clientSocket)
        {
            InitCommands();
            this.clientSocket = clientSocket;
            this.clientEndPoint = (IPEndPoint) clientSocket.RemoteEndPoint;
        }

        private void InitCommands()
        {
            commandOptions = new List<CommandOptions>()
            {
                // GENERAL COMMANDS
                new CommandOptions("help", typeof(HelpCommand)),
                new CommandOptions("versions", typeof(VersionCommand)),
                new CommandOptions("info", typeof(InfoCommand)),
                new CommandOptions("cursor", typeof(CursorCommand)),

                // JOBSYSTEM COMMANDS
                new CommandOptions("jobsystem status", typeof(JobSystemStatusCommand)),
                new CommandOptions("job status", typeof(JobListCommand)),
                new CommandOptions("job remove", typeof(JobSystemRemoveCommand)),
                new CommandOptions("job start", typeof(JobSystemStartCommand)),
                new CommandOptions("job stop", typeof(JobSystemStopCommand)),
                new CommandOptions("job add ping", typeof(JobSystemAddPingCommand)),
                new CommandOptions("job add http", typeof(JobSystemAddHttpCommand)),
                new CommandOptions("job add port", typeof(JobSystemAddPortCommand))
            };
        }

        public void Start()
        {
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
        }

        #region CLI format methodes

        private string GetCommand(string input)
        {
            string[] buffer = input.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            if (buffer.Length != 0)
            {
                if (buffer[0] == "")
                    return null;

                buffer[0] = buffer[0].Trim();

                return buffer[0];
            }
            else
                return null;
        }

        private ParameterInput GetParamtersFromInput(string input)
        {
            ParameterInput parameterTemp = new ParameterInput();

            string[] temp = input.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp.Length; i++)
                temp[i] = temp[i].Trim();

            for (int i = 1; i < temp.Length; i++)
            {
                string[] temp2 = temp[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (temp2.Length == 1)
                {
                    // parameter argument is null
                    parameterTemp.parameters.Add(new Parameter(temp2[0], null)); 
                }
                else
                {
                    // more than one argument do NOT get recordnized by the CLI!!!
                    parameterTemp.parameters.Add(new Parameter(temp2[0], temp2[1])); 
                }
            }

            return parameterTemp;
        }

        #endregion

        #region CLI logic-methodes

        private bool CommandExists(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return true;

            return false;
        }

        private Type GetCommandType(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return temp.commandType;
            return null;
        }

        #endregion
    }
}
