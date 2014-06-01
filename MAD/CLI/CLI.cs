using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class CLI : CLIFramework
    {
        public Version version = new Version(1, 0, 7000);

        public CLI()
        { 
            // init CLI
            _InitCLI();
        }

        public void Start()
        {
            string cliInput;

            while (true)
            {
                Console.Write(cursor);
                cliInput = Console.ReadLine();

                string response = AnalyseInput(cliInput, ref command);

                if (response == "VALID_PARAMETER")
                {
                    ConsoleWriter.WriteToConsole(command.Execute());
                }
                else
                {
                    ConsoleWriter.WriteToConsole(response);
                }
            }
        }
    }
}
