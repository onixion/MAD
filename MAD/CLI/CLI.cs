using System;

using MAD.jobSys;
using MAD.cli.server;

namespace MAD.cli
{
    public class CLI : CLIFramework
    {
        private Version _version = new Version(1, 6);
        public Version version { get { return _version; } }

        private string cursor = "=> ";
        public ConsoleColor cursorColor = ConsoleColor.Cyan;
        public ConsoleColor inputColor = ConsoleColor.White;

        private string _dataPath;
        private JobSystem _js;
        private CLIServer _cliServer;

        public CLI(string dataPath, JobSystem js, CLIServer cliServer)
            :base()
        {
            _dataPath = dataPath;
            _js = js;
            _cliServer = cliServer;

            InitCommands();
        }

        private void InitCommands()
        {
            // GENERAL COMMANDS
            commands.Add(new CommandOptions("exit", typeof(ExitCommand), null));
            commands.Add(new CommandOptions("help", typeof(HelpCommand), new object[] { commands }));
            commands.Add(new CommandOptions("colortest", typeof(ColorTestCommand), null));
            commands.Add(new CommandOptions("info", typeof(InfoCommand), null));
            //commands.Add(new CommandOptions("test", typeof(TestCommand), null));

            // JOBSYSTEM COMMANDS
            commands.Add(new CommandOptions("js", typeof(JobSystemStatusCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js status", typeof(JobStatusCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add ping", typeof(JobSystemAddPingCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add http", typeof(JobSystemAddHttpCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js add port", typeof(JobSystemAddPortCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js destroy", typeof(JobSystemRemoveCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js start", typeof(JobSystemStartCommand), new object[] { _js }));
            commands.Add(new CommandOptions("js stop", typeof(JobSystemStopCommand), new object[] { _js }));

            // CLISERVER COMMANDS
            commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), null));
            commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), null));
            commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), null));

            // NOTIFICATION COMMANDS

            // OTHER
        }

        public void Start()
        {
            CommandIO.WriteToConsole(GetBanner());
            WriteCursor();

            Command _command = null;

            while (true)
            {
                string _cliInput = Console.ReadLine();

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

                        CommandIO.WriteToConsole(response);
                        WriteCursor();
                    }
                    else
                    {
                        // Something must be wrong with the input (parameter does not exist, to many arguments, ..).
                        CommandIO.WriteToConsole(response);
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

        private string GetBanner()
        {
            string _buffer = "";

            _buffer += "<color><cyan>";
            _buffer += @" ___  ___  ___ ______ " + "\n";
            _buffer += @" |  \/  | / _ \|  _  \" + "\n";
            _buffer += @" | .  . |/ /_\ \ | | |" + "\n";
            _buffer += @" | |\/| ||  _  | | | |" + "\n";
            _buffer += @" | |  | || | | | |_/ |" + "\n";
            _buffer += @" \_|  |_/\_| |_/_____/" + "\n";

            return _buffer;
        }
    }
}
