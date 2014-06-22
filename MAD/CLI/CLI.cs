using System;
using System.Collections.Generic;
using System.IO;

using MAD.jobSys;

namespace MAD.cli
{
    public class CLI : CLIFramework
    {
        private Version _version = new Version(1, 6);
        public Version version { get { return _version; } }

        private string _cursor = "=> ";
        private ConsoleColor _cursorColor = ConsoleColor.Cyan;
        private ConsoleColor _inputColor = ConsoleColor.White;

        private List<string> _cliHistory = new List<string>();

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
            commands.Add(new CommandOptions("cliserver", typeof(CLIServerInfo), new object[] { _cliServer }));
            commands.Add(new CommandOptions("cliserver start", typeof(CLIServerStart), new object[] { _cliServer }));
            commands.Add(new CommandOptions("cliserver stop", typeof(CLIServerStop), new object[] { _cliServer }));

            // NOTIFICATION COMMANDS

            // OTHER
        }

        public void Start()
        {
            CommandIO.WriteToConsole(GetBanner());

            while (true)
            {
                Command _command = null;

                WriteCursor();

                //string _cliInput = Console.ReadLine();
                string _cliInput = ReadInput();

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
                        CommandIO.WriteToConsole(response);
                    }
                    else
                    {
                        // Something must be wrong with the input (parameter does not exist, to many arguments, ..).
                        CommandIO.WriteToConsole(response);
                    }
                }
            }
        }

        private string ReadInput()
        {
            string _cliInput = "";
            int _historyPointer = 0;

            while (true)
            {
                ConsoleKeyInfo _key = Console.ReadKey();

                int _cursorPos = Console.CursorLeft;

                if (_key.Key == ConsoleKey.Enter)
                {
                    _cliHistory.Add(_cliInput);
                    break;
                }
                else if (_key.Key == ConsoleKey.Backspace)
                {
                    // Problem: Windows does not recognized '\b' properly.
                    if (_cursorPos == _cursor.Length - 1)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(" \b");
                        _cliInput = _cliInput.Remove(_cliInput.Length - 1);
                    }
                }
                else if (_key.Key == ConsoleKey.UpArrow)
                {
                    string _lastInput = GetLastHistoryEntry(_historyPointer);
                    _historyPointer++;

                    while (_cursorPos != _cursor.Length - 1)
                    { 
                    
                    }
                }
                else
                {
                    _cliInput += _key.KeyChar.ToString();
                }
            }

            Console.Write("\n");

            return _cliInput;
        }

        private string GetLastHistoryEntry(int offset)
        {
            return _cliHistory[_cliHistory.Count - 1 - offset];
        }

        private void WriteCursor()
        {
            Console.ForegroundColor = _cursorColor;
            Console.Write(_cursor);
            Console.ForegroundColor = _inputColor;
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
