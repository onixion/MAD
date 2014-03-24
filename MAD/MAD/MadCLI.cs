using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD
{
    class MadCLI
    {
        public string version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public string cursor = "=> ";
        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor textColor = ConsoleColor.White;
        public string windowTitle = "MAD - Network Monitoring";

        public Command command;
        public List<string> commands = new List<string>();

        private string cliInput;

        public string mainCommand;
        public List<string[]> args;

        public MadCLI()
        {
            InitCLI();

            commands.Add("help");
            commands.Add("test");
        }

        public void InitCLI()
        {
            UpdateWindowTitle();
            Console.ForegroundColor = textColor;
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

                GetCommandArgs(cliInput);

                if (CommandExists(mainCommand))
                {
                    CreateCommand(mainCommand);

                    if (command.ValidArguments(args))
                    {
                        command.args = args;
                        command.Execute();
                    }
                }
                else
                    ErrorMessage("Command \"" + mainCommand + "\" not found!");
            }
            
        }

        public void PrintCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = textColor;
        }

        /// <summary>
        /// Get Arguments
        /// </summary>
        /// <param name="inputCLI"></param>
        /// <returns>Returns all arguments in a list with string[]</returns>
        public void GetCommandArgs(string inputCLI)
        {
            List<string[]> temp1 = new List<string[]>();
            string[] temp2 = cliInput.Split(new char[] {'-'});

            for (int i = 0; i < temp2.Length; i++)
                temp2[i] = temp2[i].Trim();

            mainCommand = temp2[0];

            for (int i = 1; i < temp2.Length; i++)
            {
                string[] temp3 = temp2[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                temp1.Add(temp3);
            }

            args = temp1;
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
                case "test":
                    command = new TestCommand();
                    break;

                case "help":
                    command = new HelpCommand();
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
