using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD
{
    public class MadCLI
    {
        public string version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public string cursor = "=> ";
        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor textColor = ConsoleColor.White;
        public string windowTitle = "MAD - Network Monitoring";

        public List<string> commands = new List<string>();
        public Command command;
        public int executeStatusCode;

        private string cliInput;

        public string commandInput;
        public List<string[]> args;

        //
        JobSystem system = new JobSystem();
        

        public MadCLI()
        {
            UpdateWindowTitle();
            Console.ForegroundColor = textColor;

            InitCommands();
        }

        /// <summary>
        /// Initial all available commands
        /// </summary>
        private void InitCommands()
        {
            commands.Add("help");
            commands.Add("clear");
            commands.Add("exit");
            commands.Add("close");
            commands.Add("header");
        }

        /// <summary>
        /// Update windowtitle
        /// </summary>
        public void UpdateWindowTitle()
        {
            Console.Title = windowTitle;
        }

        /// <summary>
        /// Start CLI
        /// </summary>
        public void Start()
        {
            PrintHeader();

            while (true)
            {
                PrintCursor();
                cliInput = Console.ReadLine();

                if (cliInput != "")
                {
                    commandInput = GetMainCommand(cliInput);

                    if (commandInput != null)
                    {
                        if (CommandExists(commandInput))
                        {
                            CreateCommand(commandInput);

                            args = GetArgs(cliInput);

                                if (command.ValidArguments(args))
                                {
                                    command.SetArguments(args);

                                    executeStatusCode = command.Execute();

                                    if (executeStatusCode != 0)
                                        ErrorMessage(ErrorText(executeStatusCode));
                                }
                                else
                                    ErrorMessage("Missing or wrong arguments!");
                        }
                        else
                            ErrorMessage("Command \"" + commandInput + "\" not found!");
                    }
                }
            }
            
        }

        /// <summary>
        /// Print cursor to cli
        /// </summary>
        public void PrintCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = textColor;
        }

        public string ErrorText(int statusCode)
        {
            switch (statusCode)
            { 
                default:
                    return "Errorcode: " + statusCode;
            }
        }

        /// <summary>
        /// Get main command
        /// </summary>
        public string GetMainCommand(string input)
        {
            string[] buffer = input.Split(new string[]{"-"},StringSplitOptions.RemoveEmptyEntries);

            if (buffer[0] == "")
                return null;

            return buffer[0];
        }

        /// <summary>
        /// Get Arguments
        /// </summary>
        public List<string[]> GetArgs(string inputCLI)
        {
            List<string[]> temp1 = new List<string[]>();
            string[] temp2 = cliInput.Split(new char[] {'-'});

            for (int i = 1; i < temp2.Length; i++)
                temp2[i] = temp2[i].Trim();

            for (int i = 1; i < temp2.Length; i++)
            {
                string[] temp3 = temp2[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                temp1.Add(temp3);
            }

            return temp1;
        }

        public string GetCommands(string inputCLI)
        {
            string[] temp = inputCLI.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            temp = temp[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            return temp[0];
        }

        /// <summary>
        /// Checks if a command exists
        /// </summary>
        public bool CommandExists(string input)
        {
            return commands.Contains(input);
        }

        /// <summary>
        /// Creates the command
        /// </summary>
        public void CreateCommand(string input)
        {
            switch (input)
            {
                case "help":
                    command = new HelpCommand();
                    break;
                case "clear":
                    command = new ClearCommand();
                    break;
                case "exit":
                    command = new ExitCommand();
                    break;
                case "close":
                    command = new ExitCommand();
                    break;
                case "header":
                    command = new HeaderCommand(this);
                    break;
                case "info":
                    command = new InfoCommand();
                    break;
                default:
                    break;
            }
        }

        public void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = textColor;
            Console.WriteLine(message);
        }

        /// <summary>
        /// Print the MAD-Logo on the CLI
        /// </summary>
        public void PrintHeader()
        {
            Console.WriteLine(@" __  __   _   ____ ");
            Console.WriteLine(@"|  \/  |/ _ \|  _  \");
            Console.WriteLine(@"| .  . / /_\ \ | | |");
            Console.WriteLine(@"| |\/| |  _  | | | |    NETWORK MONITORING");
            Console.WriteLine(@"| |  | | | | | |_/ |    VERSION " + version);
            Console.WriteLine(@"|_|  |_\_| |_/____/ __________________________ ");
            Console.WriteLine();
        }
    }
}
