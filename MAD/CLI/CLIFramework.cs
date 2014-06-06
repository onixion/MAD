using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MAD.CLI
{
    public abstract class CLIFramework
    {
        #region member

        // cli framework version
        public Version versionFramework = new Version(1,3);

        // cli vars
        protected string cursor = "=> ";

        // all commands useable for this cli
        protected List<CommandOptions> commands = new List<CommandOptions>();

        protected Command command;
        protected Type inputCommandType;

        #region all available commands
        /*
        private static List<CommandOptions> availableCommands = new List<CommandOptions>()
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
        */
        #endregion

        #endregion

        #region CLI main methodes

        /*
         * authLevel = 0 -> all commands
         * authLevel = 1 -> FileSystem, CLIServer
         * authLevel = 2 -> FileSystem
         * ...
         */
        protected void _InitCLI(int authLevel)
        {
            switch(authLevel)
            {
                case 0:

                case 1:

                case 2:

                case 3:

                case 4:
                
                case 100: // commands for everyone
                    commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[]{commands}));
                    commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), new object[0]));
                    break;
            }
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

            // get command
            commandInput = GetCommandName(cliInput);

            // check if command is known
            if (CommandExists(commandInput))
            {
                // get command 
                CommandOptions commandOptions = GetCommandOptions(commandInput);
                // get parameter and arguments from input
                ParameterInput parameterInput = GetParamtersFromInput(cliInput);

                // get command type
                Type commandType = commandOptions.commandType;
                // get parameter objects to pass to constructor of the command
                object[] commandParameters = commandOptions.commandParameterObjects;

                ConstructorInfo cInfo = commandType.GetConstructor(new Type[1] { typeof(object[]) });
                command = (Command)cInfo.Invoke(new object[]{commandParameters});

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
                return "<color><red>Command '" + commandInput + "' unknown! Type 'help' for more information.";
            }
        }

        #endregion

        #region CLI format methodes

        protected string GetCommandName(string input)
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
            foreach (CommandOptions temp in commands)
            {
                if (temp.command == command)
                {
                    return true;
                }
            }

            return false;
        }

        protected CommandOptions GetCommandOptions(string command)
        {
            foreach (CommandOptions temp in commands)
            {
                if (temp.command == command)
                {
                    return temp;
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
