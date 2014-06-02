using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class CLI : CLIFramework
    {
        public Version version = new Version(1, 2, 3001);

        public CLI()
        { 
            _InitCLI();
        }

        public void Start()
        {
            string cliInput;
            Console.Write(cursor);

            while (true)
            {
                cliInput = Console.ReadLine();

                // check if input is valid
                string response = AnalyseInput(cliInput, ref command);

                if (response == "VALID_PARAMETER")
                {
                    // input is valid -> execute command and write command output to console
                    ConsoleWriter.WriteToConsole(command.Execute());
                    Console.Write("\n" + cursor);
                }
                else if (response == "")
                {
                    // input is empty
                    Console.Write(cursor);
                }
                else
                { 
                    // something is wrong with the input (false arguments, missing arguments, and so on)
                    ConsoleWriter.WriteToConsole(response);
                    Console.Write("\n" + cursor);
                }
            }
        }
    }
}
