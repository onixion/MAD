using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD
{
    public class MadCLI
    {
        public string version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public string cliVersion = "0.0.0.2";
        public string cursor = "=> ";
        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor textColor = ConsoleColor.White;
        public string windowTitle = "MAD - Network Monitoring";

        private string cliInput;
        public List<string> commands = new List<string> { "help", "clear", "exit", "close", "logo", "info", "cursor" };
        public Command command;
        public int executeStatusCode;

        public string commandInput;
        public List<string[]> args;

        public MadCLI()
        {
            UpdateWindowTitle();
            Console.ForegroundColor = textColor;
        }

        public void UpdateWindowTitle()
        {
            Console.Title = windowTitle;
        }

        public void Start()
        {
            PrintLogo();

            while (true)
            {
                PrintCursor();
                cliInput = Console.ReadLine();

                if (cliInput != "")
                {
                    commandInput = GetCommand(cliInput);

                    if (commandInput != null)
                    {
                        if (CommandExists(commandInput))
                        {
                            CreateCommand(commandInput);

                            args = GetArgs(cliInput);

                                if (command.ValidArguments(args))
                                {
                                    command.SetArguments(args);
                                    
                                    // EXECUTE COMMAND
                                    executeStatusCode = command.Execute();

                                    if (executeStatusCode != 0)
                                        ErrorMessage(GetErrorText(executeStatusCode));
                                }
                                else
                                    ErrorMessage("Wrong or missing arguments!");
                        }
                        else
                            ErrorMessage("Command \"" + commandInput + "\" not found!");
                    }
                }
            }
            
        }

        public void PrintCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = textColor;
        }

        public string GetErrorText(int statusCode)
        {
            switch (statusCode)
            { 
                case 1:
                    return "Missing or wrong arguments!";
                default:
                    return "Errorcode: " + statusCode;
            }
        }

        public string GetCommand(string input)
        {
            string[] buffer = input.Split(new string[]{"-"},StringSplitOptions.RemoveEmptyEntries);

            if (buffer[0] == "")
                return null;

            buffer[0] = buffer[0].Trim();

            return buffer[0];
        }

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

        public bool CommandExists(string input)
        {
            return commands.Contains(input);
        }

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
                case "logo":
                    command = new LogoCommand(this);
                    break;
                case "info":
                    command = new InfoCommand(this);
                    break;
                case "cursor":
                    command = new CursorCommand(this);
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

        public void PrintLogo()
        {
            Console.WriteLine(@" __  __   _   ____ ");
            Console.WriteLine(@"|  \/  |/ _ \|  _  \");
            Console.WriteLine(@"| .  . / /_\ \ | | |    MONITORING LIKE A BOZZ!");
            Console.WriteLine(@"| |\/| |  _  | | | |");
            Console.WriteLine(@"| |  | | | | | |_/ |    MadCLI-VERSION " + cliVersion);
            Console.WriteLine(@"|_|  |_\_| |_/____/ __________________________ ");
            Console.WriteLine();
        }
    }
}
