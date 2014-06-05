using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.CLI
{
    public abstract class CLIFramework
    {
        #region member

        // cli framework version
        public Version versionFramework = new Version(1,3);

        // cli vars
        protected string cursor = "=> ";

        // cli command vars
        protected List<CommandOptions> commandOptions;
        protected Command command;
        protected Type inputCommandType;

        #endregion

        #region CLI main methodes

        protected void _InitCLI()
        {
            commandOptions = new List<CommandOptions>()
            {
                // GENERAL COMMANDS
                new CommandOptions("help", typeof(HelpCommand)),
                new CommandOptions("info", typeof(InfoCommand)),
                new CommandOptions("colortest", typeof(ColorTest)),
                
                // JOBSYSTEM COMMANDS
                new CommandOptions("jobsystem", typeof(JobSystemStatusCommand)),
                new CommandOptions("job status", typeof(JobStatusCommand)),
                new CommandOptions("job remove", typeof(JobSystemRemoveCommand)),
                new CommandOptions("job start", typeof(JobSystemStartCommand)),
                new CommandOptions("job stop", typeof(JobSystemStopCommand)),
                new CommandOptions("job add ping", typeof(JobSystemAddPingCommand)),
                new CommandOptions("job add http", typeof(JobSystemAddHttpCommand)),
                new CommandOptions("job add port", typeof(JobSystemAddPortCommand)),

                // CLI SERVER COMMANDS
                new CommandOptions("cliserver", typeof(CLIServerInfo)),
                new CommandOptions("cliserver start", typeof(CLIServerStart)),
                new CommandOptions("cliserver stop", typeof(CLIServerStop))
            };
        }

        /*
         * This function checks if the parms are valid and sets the command
         * object.
         * 
         * It returns the string "VALID_PARAMETER" when the parameter are valid.
         * If the parameter are not valid it returns the error text. */
        public string AnalyseInput(string cliInput, ref Command command)
        {
            string commandInput;

            if (cliInput != "")
            {
                // get command
                commandInput = GetCommand(cliInput);

                // check if command is known
                if (CommandExists(commandInput))
                {
                    // get command type
                    Type inputCommandType = GetCommandType(commandInput);
                    // get parameter and arguments from input
                    ParameterInput parameterInput = GetParamtersFromInput(cliInput);

                    // create command object (pass the command none objects)
                    command = (Command)inputCommandType.GetConstructor(new Type[0]).Invoke(new object[0]);

                    // check if the arguments are valid (parameter valid = "VALID_PARAMETER")
                    string parameterValid = command.ValidParameters(parameterInput);

                    // set parameters if the parameter are valid
                    if (parameterValid == "VALID_PARAMETER")
                    {
                        command.SetParameters(parameterInput);
                    }

                    return parameterValid;
                }
                else
                {
                    return "<color><red>Command '" + commandInput + "' unknown! Type 'help' for more information.\n";
                }
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region CLI format methodes

        protected string GetCommand(string input)
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

        protected ParameterInput GetParamtersFromInput(string input)
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

        #region CLI internal methodes

        protected bool CommandExists(string command)
        {
            foreach (CommandOptions temp in commandOptions)
            {
                if (temp.command == command)
                {
                    return true;
                }
            }

            return false;
        }

        protected Type GetCommandType(string command)
        {
            foreach (CommandOptions temp in commandOptions)
            {
                if (temp.command == command)
                {
                    return temp.commandType;
                }
            }

            return null;
        }

        #endregion

        #region CLI TimeStamp

        protected string GetTimeStamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        #endregion
    }
}
