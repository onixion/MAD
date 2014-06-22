﻿using System;
using System.Collections.Generic;
using System.IO;

using MAD.jobSys;

namespace MAD.cli
{
    public class CLI : CLIFramework
    {
        private Version _version = new Version(1, 6);
        public Version version { get { return _version; } }

        private int _maxHistoryEntries = 5;
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
            int _historyPointer = -1;

            while (true)
            {
                ConsoleKeyInfo _key = Console.ReadKey();

                int _cursorPos = Console.CursorLeft;

                if (_key.Key == ConsoleKey.Enter)
                {
                    AddToHistory(_cliInput);
                    break;
                }
                else if (_key.Key == ConsoleKey.Backspace)
                {
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
                else if (_key.Key == ConsoleKey.LeftArrow)
                {
                    ShiftCursorLeft();

                    if (_cursor.Length < Console.CursorLeft)
                    {
                        ShiftCursorLeft();
                    }
                }
                else if (_key.Key == ConsoleKey.RightArrow)
                {
                    // HERE

                    if (_cursor.Length + _cliInput.Length + 1 > Console.CursorLeft)
                    {
                        char _overwrittenChar = GetOverwrittenChar(_cliInput, Console.CursorLeft - 1);
                        ShiftCursorLeft();
                        Console.Write(_overwrittenChar);
                    }
                    else
                    {
                        ShiftCursorLeft();
                    }
                }
                else if (_key.Key == ConsoleKey.UpArrow)
                {
                    Console.Write(" \b");

                    if (_cliHistory.Count - 1 > _historyPointer)
                    {
                        _historyPointer++;
                    }

                    string _lastInput = GetLastHistoryEntry(_historyPointer);

                    ClearInput(_cliInput);
                    WriteIntoInput(_lastInput);

                    _cliInput = _lastInput;
                }
                else if (_key.Key == ConsoleKey.DownArrow)
                {
                    Console.Write(" \b");

                    if (0 < _historyPointer)
                    {
                        _historyPointer--;
                    }

                    string _lastInput = GetLastHistoryEntry(_historyPointer);

                    ClearInput(_cliInput);
                    WriteIntoInput(_lastInput);
                 
                    _cliInput = _lastInput;
                }
                else
                {
                    _cliInput += _key.KeyChar.ToString();
                }
            }

            Console.Write("\n");

            return _cliInput;
        }

        private char GetOverwrittenChar(string cliInput, int pos)
        {
            return cliInput[pos - _cursor.Length];
        }

        private string GetLastHistoryEntry(int pointer)
        {
            if (pointer != -1)
            {
                return _cliHistory[_cliHistory.Count - 1 - pointer];
            }
            else
            {
                return "";
            }
        }

        private void AddToHistory(string command)
        {
            if (_cliHistory.Count >= _maxHistoryEntries)
            {
                _cliHistory.RemoveAt(0);
                _cliHistory.Add(command);
            }
            else
            {
                _cliHistory.Add(command);
            }
        }

        private void ClearInput(string text)
        {
            Console.SetCursorPosition(_cursor.Length, Console.CursorTop);
            Console.Write("".PadLeft(text.Length, ' '));
        }

        private void WriteIntoInput(string text)
        {
            Console.SetCursorPosition(_cursor.Length, Console.CursorTop);
            Console.Write(text);
        }

        private void ShiftCursorLeft()
        {
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }

        private void ShiftCursorRight()
        {
            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
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
