using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class CLI : CLIFramework
    {
        public Version version = new Version(1, 3, 2000);

        public CLI()
        { 
            _InitCLI(0);
        }

        public void Start()
        {
            string cliInput;
            Console.Write(cursor);

            while (true)
            {
                cliInput = Console.ReadLine();

                if (cliInput != "")
                {
                    // check if input is valid
                    string response = AnalyseInput(cliInput, ref command);

                    if (response == "VALID_PARAMETER")
                    {
                        // input is valid -> execute command and write output of the command to console
                        ConsoleWriter.WriteToConsole(command.Execute());
                        Console.Write(cursor);
                    }
                    else
                    {
                        // something is wrong with the input (false arguments, missing arguments, ..)
                        ConsoleWriter.WriteToConsole(response);
                        Console.Write(cursor);
                    }
                }
                else
                {
                    Console.Write(cursor);
                }
            }
        }
    }
}
