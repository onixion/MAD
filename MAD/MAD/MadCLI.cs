using System;
using System.Net;
using System.Reflection;
using System.Collections.Generic;

namespace MAD
{
    public class MadCLI
    {
        public string version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public string cliVersion = "0.0.4.0";
        public string cursor = "=> ";
        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor textColor = ConsoleColor.White;
        public string windowTitle = "MAD - Network Monitoring";

        private string cliInput;
        public string inputCommand;
        public List<string[]> inputArgs;

        public List<CommandOptions> commandOptions = new List<CommandOptions>{};
        public Command command;
        public Type inputCommandType;

        public int statusCode;

        //
        JobSystem js = new JobSystem();
        //

        public MadCLI()
        {
            UpdateWindowTitle();
            InitCommands();
            Console.ForegroundColor = textColor;
        }

        private void InitCommands()
        {
            // GENERAL COMMANDS
            commandOptions.Add(new CommandOptions("help", typeof(HelpCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("clear", typeof(ClearCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("logo", typeof(LogoCommand), new Type[]{typeof(MadCLI)}, new object[]{this}));
            commandOptions.Add(new CommandOptions("exit", typeof(ExitCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("close", typeof(ExitCommand), new Type[0], new object[0]));
            commandOptions.Add(new CommandOptions("cursor", typeof(CursorCommand), new Type[]{typeof(MadCLI)}, new object[]{this}));

            // JOBSYSTEM COMMANDS
            commandOptions.Add(new CommandOptions("job status", typeof(JobSystemListCommand), new Type[] { typeof(JobSystem) }, new object[] { js }));
            commandOptions.Add(new CommandOptions("job add", typeof(JobSystemAddCommand), new Type[] { typeof(JobSystem) }, new object[] { js }));
            commandOptions.Add(new CommandOptions("job remove", typeof(JobSystemRemoveCommand), new Type[] { typeof(JobSystem) }, new object[] { js }));
            commandOptions.Add(new CommandOptions("job start", typeof(JobSystemStartCommand), new Type[] { typeof(JobSystem) }, new object[] { js }));
            commandOptions.Add(new CommandOptions("job stop", typeof(JobSystemStopCommand), new Type[] { typeof(JobSystem) }, new object[] { js }));
        }

        public void UpdateWindowTitle()
        {
            Console.Title = windowTitle;
        }

        public void Start()
        {
            PrintWelcome();
            PrintLogo();

            while (true)
            {
                PrintCursor();
                cliInput = Console.ReadLine();

                inputCommand = GetCommand(cliInput);

                if (inputCommand != null)
                {
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

                            // check statuscode
                            if (statusCode != 0)
                                ErrorMessage(GetErrorText(statusCode));
                        }
                        else
                            ErrorMessage(GetErrorText(1));
                    }
                    else
                        ErrorMessage("Command \"" + inputCommand + "\" unknown!");
                }
            }   
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

        public void PrintCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = textColor;
        }

        private string GetErrorText(int statusCode)
        {
            switch (statusCode)
            { 
                case 1:
                    return "Missing or wrong arguments!";
                case 2:
                    return "Wrong argument type!";
                case 3:
                    return "Some arguments are null!";
                case 30:
                    return "Job do not exist!";
                default:
                    return "Errorcode: " + statusCode;
            }
        }

        public string GetCommand(string input)
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

        public List<string[]> GetArgs(string input)
        {
            List<string[]> temp1 = new List<string[]>();
            string[] temp2 = input.Split(new char[] {'-'}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp2.Length; i++)
                temp2[i] = temp2[i].Trim();

            for (int i = 1; i < temp2.Length; i++)
            {
                string[] temp3 = temp2[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                temp1.Add(temp3);
            }

            return temp1;
        }

        public void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
        }

        public void PrintLogo()
        {
            Console.WriteLine(@" __  __ 2 _   ____ ");
            Console.WriteLine(@"|  \/  |/ _ \|  _  \");
            Console.WriteLine(@"| .  . / /_\ \ | | |    MONITORING LIKE A BOZZ!");
            Console.WriteLine(@"| |\/| |  _  | | | |");
            Console.WriteLine(@"| |  | | | | | |_/ |    MadCLI-VERSION " + cliVersion);
            Console.WriteLine(@"|_|  |_\_| |_/____/ __________________________ ");
            Console.WriteLine();
        }

        public void PrintWelcome()
        {
            Console.WriteLine("Welcome to the MAD-CommandLineInterface!");
            Console.WriteLine();
            Console.Write("This CLI is still not finished yet. Many important parts of the CLI are missing or not implemented yet. ");
            Console.Write("So please stay calm and relaxed if it should crash ...");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write(" > Press any key to continue ... ");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
