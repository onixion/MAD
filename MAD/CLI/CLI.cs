using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class CLI : CLIFramework
    {
        public Version version = new Version(1, 4, 3000);

        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor inputColor = ConsoleColor.White;

        public CLI()
        {
            // GENERAL
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));
            commands.Add(new CommandOptions("test", typeof(TestCommand), null));

            // JOBSYSTEM
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), null));
            commands.Add(new CommandOptions("js status", typeof(JobStatusCommand), null));
            commands.Add(new CommandOptions("js add ping", typeof(JobSystemAddPingCommand), null));
            commands.Add(new CommandOptions("js add http", typeof(JobSystemAddHttpCommand), null));
            commands.Add(new CommandOptions("js add port", typeof(JobSystemAddPortCommand), null));
            commands.Add(new CommandOptions("js destroy", typeof(JobSystemRemoveCommand), null));
            commands.Add(new CommandOptions("js start", typeof(JobSystemStartCommand), null));
            commands.Add(new CommandOptions("js stop", typeof(JobSystemStopCommand), null));

            // CLISERVER
            commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), null));
            commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), null));
            commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), null));

            // NOTIFICATION

            // OTHER
        }

        public void Start()
        {
            string cliInput;
            WriteCursor();

            while (true)
            {
                cliInput = Console.ReadLine();

                if (cliInput != "")
                {
                    // check if input is valid
                    string response = AnalyseInput(cliInput, ref command);

                    if (response == "VALID_PARAMETER")
                    {
                        // input is valid -> execute command and write output to console
                        ConsoleWriter.WriteToConsole(command.Execute());
                        WriteCursor();
                    }
                    else
                    {
                        // something is wrong with the input (false arguments, missing arguments, ..)
                        ConsoleWriter.WriteToConsole(response);
                        WriteCursor();
                    }
                }
                else
                {
                    WriteCursor();
                }
            }
        }

        private void WriteCursor()
        {
            Console.ForegroundColor = cursorColor;
            Console.Write(cursor);
            Console.ForegroundColor = inputColor;
        }
    }
}
