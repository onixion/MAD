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
        // cli variables
        public string version = "0.0.5.5";
        public string cursor = "=> ";
        public string windowTitle = "MAD - Network Monitoring";

        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor textColor = ConsoleColor.White;
        public ConsoleColor logoColor = ConsoleColor.Cyan;

        // input variables
        private string cliInput;
        private string inputCommand;
        private List<string[]> inputArgs;

        // command variables
        private List<CommandOptions> commandOptions = new List<CommandOptions>{};
        private Command command;
        private Type inputCommandType;
        private int statusCode;

        public MadCLI()
        {
            SetWindowTitle();
            InitCommands();
            Console.ForegroundColor = textColor;
        }

        public void SetWindowTitle()
        {
            Console.Title = windowTitle;
        }

        private void InitCommands()
        {
            // GENERAL COMMANDS
            commandOptions.Add(new CommandOptions("help", typeof(HelpCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("exit", typeof(ExitCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("close", typeof(ExitCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("versions", typeof(VersionsCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("info", typeof(InfoCommand), new Type[0], new object[0]));

            commandOptions.Add(new CommandOptions("clear", typeof(ClearCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("logo", typeof(LogoCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("cursor", typeof(CursorCommand), new Type[0], new object[0]));

            // JOBSYSTEM COMMANDS
            commandOptions.Add(new CommandOptions("jobsystem status", typeof(JobSystemStatusCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("job status", typeof(JobListCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("job remove", typeof(JobSystemRemoveCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("job start", typeof(JobSystemStartCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("job stop", typeof(JobSystemStopCommand), new Type[0], new object[0]));

            commandOptions.Add(new CommandOptions("job add ping", typeof(JobSystemAddPingCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("job add http", typeof(JobSystemAddHttpCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("job add port", typeof(JobSystemAddPortCommand), new Type[0], new object[0]));
        }

        public void Start()
        {
            //
            MadComponents.components.jobSystem.AddJob(new JobHttpOptions("HttpRequest01", JobOptions.JobType.HttpRequest, 4000, IPAddress.Parse("127.0.0.1"), 80));
            MadComponents.components.jobSystem.AddJob(new JobPingOptions("PingRequest01", JobOptions.JobType.PingRequest, 4000, IPAddress.Parse("127.0.0.1"), 300));
            MadComponents.components.jobSystem.AddJob(new JobPortOptions("PortRequest01", JobOptions.JobType.PortRequest, 4000, IPAddress.Parse("127.0.0.1"), 80));
            //

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

                        // create command object
                        command = (Command)inputCommandType.GetConstructor(GetObjectsTypeArray(inputCommand)).Invoke(GetCommandObjects(inputCommand));

                        // check if the arguments are valid
                        if (command.ValidArguments(inputArgs))
                        {
                            // set arguments
                            command.SetArguments(inputArgs);

                            // EXECUTE COMMAND AND GET STATUSCODE AFTER EXECUTION
                            statusCode = command.Execute();
                        }
                    }
                    else
                        ErrorMessage("Command \"" + inputCommand + "\" unknown!");
                }
            }   
        }

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

        public bool CommandExists(string command)
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

        private string GetCommand(string input)
        {
            string[] buffer = input.Split(new string[]{"-"},StringSplitOptions.RemoveEmptyEntries);

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

        private List<string[]> GetArgs(string input)
        {
            List<string[]> tempArgs = new List<string[]>();

            string[] temp = input.Split(new char[] {'-'}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp.Length; i++)
                temp[i] = temp[i].Trim();

            for (int i = 1; i < temp.Length; i++)
            {
                string[] temp2 = temp[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (temp2.Length == 1)
                    tempArgs.Add(new string[]{temp2[0], null});
                else
                    tempArgs.Add(new string[] { temp2[0], temp2[1] }); // more arguments than 1 do not get recordnized
            }

            return tempArgs;
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
