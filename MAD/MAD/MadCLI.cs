using System;
using System.Collections.Generic;

namespace MAD
{
    class MadCLI
    {
        public Command command;

        public List<string[]> commandArguments;

        public string cursor = "=> ";
        private string cliInput;
        private string[] cliInputArray;

        public string windowTitle = "MAD - Network Monitoring";

        public MadCLI()
        { 

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
            while (true)
            {
                Console.Write(cursor);
                cliInput = Console.ReadLine();
                cliInputArray = cliInput.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (CommandExists(cliInputArray[0]))
                {
                    commandArguments = GetCommandArgs(cliInput);

                    if (command.ValidArguments(commandArguments))
                    {
                        string[] buffer = commandArguments[0];
                        CommandCreate(buffer[0]);
                        command.Execute();
                    }
                    else
                        Console.WriteLine("Wrong syntax or missing arguments!");
                }
                else
                    Console.WriteLine("Command not found!");
            }
        }

        /// <summary>
        /// Get Arguments
        /// </summary>
        /// <param name="inputCLI"></param>
        /// <returns>Returns all arguments in a list with string[]</returns>

        public List<string[]> GetCommandArgs(string inputCLI)
        {
            List<string[]> temp = new List<string[]>();
            string[] temp2 = cliInput.Split('-');

            for (int i = 0; i < temp2.Length; i++)
            {
                string[] temp3 = temp2[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                temp.Add(temp3);
            }

            return temp;
        }

        /// <summary>
        /// Checks if a command exists and creates it
        /// </summary>

        public bool CommandExists(string input)
        {
            switch (input)
            {
                case "test":
                    return true;

                case "help":
                    return true;

                case "":
                    break;

                default:
                    break;
            }

            return false;
        }

        public void CommandCreate(string input)
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

        /// <summary>
        /// Print the MAD-Logo on the CLI
        /// </summary>

        public void PrintHeader()
        { 
        
        }
    }
}
