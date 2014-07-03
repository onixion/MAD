using System;
using System.Collections.Generic;
using System.IO;

using MAD.JobSystemCore;
using MAD.CLIServerCore;
using MAD.CLIIO;

namespace MAD.CLICore
{
    public class CLI : CLIFramework
    {
        #region members

        private CLIInput _input = new CLIInput();

        #endregion

        #region constructor

        public CLI(string dataPath, JobSystem js, CLIServer cliServer)
            :base()
        {
            // Set the objects needed for the definition of the commands.
            SetWorkObjects(js, cliServer);

            // Add commands to this cli.
            AddToCommands(CommandGroup.Gereral, CommandGroup.JobSystem, CommandGroup.CLIServer);
        }

        #endregion

        #region methodes

        public void Start()
        {
            ConsoleIO.WriteToConsole(GetBanner());

            while (true)
            {
                Command _command = null;

                _input.WriteCursor();

                //string _cliInput = Console.ReadLine();
                string _cliInput = _input.ReadInput();

                if (_cliInput != "")
                {
                    string response = AnalyseInput(_cliInput, ref _command);

                    // Check if the parameter and arguments are valid.
                    if (response == "VALID_PARAMETER")
                    {
                        // Execute command and get response from command.
                        response = _command.Execute();

                        // When command response with 'EXIT_CLI' the CLI closes.
                        if (response == "EXIT_CLI")
                            break;

                        // Write command ouput to console.
                        ConsoleIO.WriteToConsole(response);
                    }
                    else
                    {
                        // Something must be wrong with the input (parameter does not exist, to many arguments, ..).
                        ConsoleIO.WriteToConsole(response);
                    }
                }
            }
        }

        private string GetBanner()
        {
            string _buffer = "";

            _buffer += @"<color><cyan> ___  ___  ___ ______ " + "\n";
            _buffer += @"<color><cyan> |  \/  | / _ \|  _  \" + "\n";
            _buffer += @"<color><cyan> |      |/ /_\ \ | | |" + "\n";
            _buffer += @"<color><cyan> | |\/| ||  _  | | | | <color><yellow>VERSION <color><white>" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + "\n";
            _buffer += @"<color><cyan> | |  | || | | | |_/ | <color><yellow>TIME: <color><white>" + DateTime.Now.ToString("HH:mm:ss") + " <color><yellow>DATE: <color><white>" + DateTime.Now.ToString("dd.MM.yyyy") + "\n";
            _buffer += @"<color><cyan> \_|  |_/\_| |_/_____/_________________________________" + "\n";

            return _buffer;
        }

        #endregion
    }
}
