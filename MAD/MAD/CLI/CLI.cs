using System;
using System.Reflection;
using System.Collections.Generic;

//
using System.Net;
//

namespace MAD
{
    public class MadCLI
    {
        // cli vars
        public string version = "0.0.6.5";
        public string cursor = "=> ";
        public string windowTitle = "MAD - Network Monitoring";

        // cli color vars
        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor textColor = ConsoleColor.White;
        public ConsoleColor logoColor = ConsoleColor.Cyan;

        // cli input vars
        private string cliInput;
        private string inputCommand;
        private List<object[]> inputArgs;

        // cli command vars
        private List<CommandOptions> commandOptions;
        private Command command;
        private Type inputCommandType;

        public MadCLI()
        {
            InitCommands();
        }

        public void SetWindowTitle()
        {
            Console.Title = windowTitle;
        }

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

        public void Start()
        {
            SetWindowTitle();
            Console.ForegroundColor = textColor;

            PrintLogo();
            while (true)
            {
                PrintCursor();
                cliInput = Console.ReadLine();

                // get command
                inputCommand = GetCommand(cliInput);

                if (inputCommand != null)
                {
                    // check if command are known
                    if (CommandExists(inputCommand))
                    {
                        // get arguments from input
                        inputArgs = GetArgs(cliInput);
                        // get command type
                        inputCommandType = GetCommandType(inputCommand);

                        // create command object (pass the command none objects)
                        command = (Command)inputCommandType.GetConstructor(new Type[0]).Invoke(new object[0]);

                        // check if the arguments are valid
                        if (command.ValidArguments(inputArgs))
                        {
                            // set arguments
                            command.SetArguments(inputArgs);

                            // EXECUTE COMMAND
                            command.Execute();
                        }
                    }
                    else
                        ErrorMessage("Command \"" + inputCommand + "\" unknown!");
                }
            }   
        }

        //

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

        public bool CommandExists(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return true;
            return false;
        }

        private List<object[]> GetArgs(string input)
        {
            List<object[]> tempArgs = new List<object[]>();

            string[] temp = input.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp.Length; i++)
                temp[i] = temp[i].Trim();

            for (int i = 1; i < temp.Length; i++)
            {
                string[] temp2 = temp[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (temp2.Length == 1)
                    tempArgs.Add(new string[] { temp2[0], null }); // argument value is empty
                else
                    tempArgs.Add(new string[] { temp2[0], temp2[1] }); // more arguments than 1 do not get recordnized
            }

            return tempArgs;
        }

        private Type GetCommandType(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return temp.commandType;
            return null;
        }

        /*
        private Type[] GetObjectsTypeArray(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return temp.commandObjectsTypes;
            return null;
        }

        private object[] GetCommandObjects(string command)
        {
            foreach (CommandOptions temp in commandOptions)
                if (temp.command == command)
                    return temp.commandObjects;
            return null;
        }
         * */

        public void PrintLogo()
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

        private void PrintCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = textColor;
        }

        private void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
        }
    }
}
