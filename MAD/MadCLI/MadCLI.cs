using System;
using System.Reflection;
using System.Collections.Generic;

namespace MadCLI
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

        private string cliInput;

        public string mainCommand;
        public List<string[]> args;

        public MadCLI()
        {
            UpdateWindowTitle();
            Console.ForegroundColor = textColor;

            InitCommands();
        }

        private void InitCommands()
        {
            commands.Add("clear");
            commands.Add("help");
            commands.Add("header");
        }

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
                    mainCommand = GetMainCommand(cliInput);
                    args = GetCommandArgs(cliInput);

                    if (CommandExists(mainCommand))
                    {
                        if (CheckArguments(args))
                        {
                            CreateCommand(mainCommand);

                            if (command.ValidArguments(args))
                            {
                                command.args = args;
                                command.Execute();
                            }
                            else
                                ErrorMessage("Missing or wrong arguments!");
                        }
                        else
                            ErrorMessage("Argument NULL Exception!");
                    }
                    else
                        ErrorMessage("Command \"" + mainCommand + "\" not found!");
                }
            }
            
        }

        public void PrintCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = textColor;
        }

        /// <summary>
        /// Get main command
        /// </summary>
        public string GetMainCommand(string input)
        {
            string[] buffer = input.Split(new string[]{"-"},StringSplitOptions.None);
            buffer[0] = buffer[0].Trim();

            return buffer[0];
        }

        private bool CheckArguments(List<string[]> args)
        {
            foreach (string[] temp in args)
                if (temp.Length == 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Get Arguments
        /// </summary>
        public List<string[]> GetCommandArgs(string inputCLI)
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

        /// <summary>
        /// Checks if a command exists
        /// </summary>
        public bool CommandExists(string input)
        {
            return commands.Contains(input);
        }

        /// <summary>
        /// creates the command
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
                case "header":
                    command = new HeaderCommand(this);
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
            Console.WriteLine(@"___  ___ ___ ______");
            Console.WriteLine(@"|  \/  |/ _ \|  _  \");
            Console.WriteLine(@"| .  . / /_\ \ | | |");
            Console.WriteLine(@"| |\/| |  _  | | | |    NETWORK MONITORING");
            Console.WriteLine(@"| |  | | | | | |/ /     VERSION " + version);
            Console.WriteLine(@"\_|  |_\_| |_/___/___________________________ ");
            Console.WriteLine();
        }
    }
}
