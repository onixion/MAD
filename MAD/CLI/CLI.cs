﻿using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD
{
    public class MadCLI
    {
        // cli vars
        public string version = "0.0.7.0";
        public string cursor = "=> ";

        // cli color vars
        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor textColor = ConsoleColor.White;
        public ConsoleColor logoColor = ConsoleColor.Cyan;

        // cli input vars
        private string cliInput;
        private string commandInput;
        private ParameterInput parameterInput;

        // cli command vars
        private List<CommandOptions> commandOptions;
        private Command command;
        private Type inputCommandType;

        // --------------------------------------------------------
        //          CLI Framework
        // --------------------------------------------------------

        public MadCLI()
        {
            InitCommands();
        }

        /// <summary>
        /// Initilize commands.
        /// </summary>
        private void InitCommands()
        {
            commandOptions = new List<CommandOptions>()
            {
                // GENERAL COMMANDS
                new CommandOptions("help", typeof(HelpCommand)),
                new CommandOptions("exit", typeof(ExitCommand)),
                new CommandOptions("close", typeof(ExitCommand)),
                new CommandOptions("versions", typeof(VersionsCommand)),
                new CommandOptions("info", typeof(InfoCommand)),
                new CommandOptions("clear", typeof(ClearCommand)),
                new CommandOptions("logo", typeof(LogoCommand)),
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

        /// <summary>
        /// Start the cli.
        /// </summary>

        public void Start()
        {
            PrintLogo();

            while (true)
            {
                PrintCursor();

                // cli waiting for input
                cliInput = Console.ReadLine();

                // get command
                commandInput = GetCommand(cliInput);

                if (commandInput != null)
                {
                    // check if command are known
                    if (CommandExists(commandInput))
                    {
                        // get arguments from input
                        parameterInput = GetParamtersFromInput(cliInput);
                        // get command type
                        inputCommandType = GetCommandType(commandInput);

                        // create command object (pass the command none objects)
                        command = (Command)inputCommandType.GetConstructor(new Type[0]).Invoke(new object[0]);

                        // check if the arguments are valid
                        if (command.ValidArguments(parameterInput))
                        {
                            // set command parameters 
                            command.SetParameters(parameterInput);

                            // EXECUTE COMMAND
                            command.Execute();
                        }
                    }
                    else
                        ErrorMessage("Command '" + commandInput + "' unknown! Type 'help' for more information.");
                }
            }   
        }

        /// <summary>
        /// Print logo for the cli.
        /// </summary>
        private void PrintLogo()
        {
            Console.ForegroundColor = logoColor;
            Console.WriteLine();
            Console.WriteLine(@" ███╗   ███2 █████╗ ██████╗");
            Console.WriteLine(@" ████╗ ████║██╔══██╗██╔══██╗");
            Console.WriteLine(@" ██╔████╔██║███████║██║  ██║");
            Console.WriteLine(@" ██║╚██╔╝██║██╔══██║██║  ██╠═════════════════════╗");
            Console.WriteLine(@" ██║ ╚═╝ ██║██║  ██║██████╔╝ CLI VERSION " + version + " ║");
            Console.WriteLine(@" ╚═╝     ╚═╝╚═╝  ╚═╝╚═════╩══════════════════════╝");
            Console.WriteLine();
            Console.ForegroundColor = textColor;
        }

        /// <summary>
        /// Print cursor for the cli.
        /// </summary>
        private void PrintCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = textColor;
        }

        /// <summary>
        /// Get command from input.
        /// </summary>
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

        /// <summary>
        /// Check if command is known by the cli.
        /// </summary>
        private bool CommandExists(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return true;

            return false;
        }

        /// <summary>
        /// Get Parameters from input (e.g. -a MyArgument)
        /// </summary>
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
                    parameterTemp.parameters.Add(new Parameter(temp2[0], null)); // parameter argument is null
                }
                else
                {
                    parameterTemp.parameters.Add(new Parameter(temp2[0], temp2[1])); // more than one argument do NOT get recordnized by the CLI!!!
                }
            }

            return parameterTemp;
        }

        /// <summary>
        /// Get command type.
        /// </summary>
        /// <param name="command"></param>
        private Type GetCommandType(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return temp.commandType;
            return null;
        }

        /// <summary>
        /// Print error message to cli.
        /// </summary>
        private void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.WriteLine(message);
            Console.ForegroundColor = textColor;
        }
    }
}