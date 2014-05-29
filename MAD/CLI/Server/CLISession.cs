using System;
using System.Reflection;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MAD.CLI.Server
{
    public class CLISession : CLIFramework
    {
        #region members

        private static int _sessionsCount = 0;
        public int sessionID;
        private object _sessionInitLock = new object();

        private TcpClient client;
        private IPEndPoint clientEndPoint;

        private CLIUser user;

        #endregion

        public CLISession(TcpClient client, CLIUser user)
        {
            this.client = client;
            this.clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;

            this.user = user;

            // init CLISession
            _InitSession();

            // init CLI
            _InitCLI();

            // start session
            this.Start();
        }

        #region methodes

        private void _InitSession()
        {
            lock (_sessionInitLock)
            {
                sessionID = _sessionsCount;
                _sessionsCount++;
            }
        }

        private void Start()
        {
            NetworkStream stream = client.GetStream();

            // first time send cursor
            NetCommunication.SendString(stream, cursor, true);

            while (true)
            {
                cliInput = NetCommunication.ReceiveString(stream);
                Console.WriteLine(cliInput);

                if (cliInput != "")
                {
                    if (cliInput == "exit" || cliInput == "close")
                    {
                        break;
                    }

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

                        // check if the arguments are valid (string = VALID_PARAMETER)
                        parameterValid = command.ValidParameters(parameterInput);

                        if (parameterValid == "VALID_PARAMETER")
                        {
                            // set command parameters 
                            command.SetParameters(parameterInput);

                            // EXECUTE COMMAND AND SEND OUTPUT
                            NetCommunication.SendString(stream, command.Execute() + "\n<color><gray>" + cursor, true);
                        }
                        else
                        {
                            NetCommunication.SendString(stream, "<color><red>" + parameterValid + "\n<color><gray>" + cursor, true);
                        }
                    }
                    else
                    {
                        NetCommunication.SendString(stream, "<color><red>Command '" + commandInput + "' unknown! Type 'help' for more information.\n<color><gray>" + cursor, true);
                    }
                }
                else
                {
                    NetCommunication.SendString(stream, "<color><gray>" + cursor, true);
                }
            }
        }

        #endregion
    }
}
