using System;
using System.Collections.Generic;

namespace MAD.CLICore
{
    /*
     * This class is responsible for the input of the cli.
     * It defines what happend when keys are pressed. (TAB, ENTER, ...) 
    */

    public static class CLIInput
    {
        #region members

        private static object _inputLock = new object();

        private static ConsoleColor _cursorColor = ConsoleColor.Cyan;
        private static ConsoleColor _inputColor = ConsoleColor.White;

        private const int INPUT_MAX = 100;

        public static string cursor = "MAD> ";

        private static int _inputPos;
        private static int _inputPosMIN;
        private static int _inputPosMAX;

        private static List<string> _cliHistory = new List<string>();
        private const int _maxHistoryEntries = 5;
        private static int _historyPointer;

        #endregion

        #region methodes

        public static void WriteCursor()
        {
            Console.ForegroundColor = _cursorColor;
            Console.Write(cursor);
        }

        public static string ReadInput()
        {
            lock (_inputLock)
            {
                string _cliInput = "";
                Console.ForegroundColor = _inputColor;

                try
                {
                    _historyPointer = -1;
                    _inputPos = cursor.Length;
                    _inputPosMIN = cursor.Length;

                    Console.SetCursorPosition(_inputPos, Console.CursorTop);

                    while (true)
                    {
                        ConsoleKeyInfo _key = Console.ReadKey(true);

                        _inputPos = Console.CursorLeft;
                        _inputPosMAX = cursor.Length + _cliInput.Length;

                        if (_key.Key == ConsoleKey.Enter)
                        {
                            // Add cli-input to history.
                            AddToHistory(_cliInput);
                            break;
                        }
                        else if (_key.Key == ConsoleKey.Tab)
                        {
                            // Tab does nothing .. 
                        }
                        else if (_key.Key == ConsoleKey.Escape)
                        {
                            // Clear cli-input.
                            ClearInput(_cliInput.Length + 1);
                            _cliInput = "";

                            // Set cursor new.
                            SetCursor(cursor.Length);
                        }
                        else if (_key.Key == ConsoleKey.Backspace)
                        {
                            if (_inputPos > _inputPosMIN)
                            {
                                ClearInput(_cliInput.Length);
                                _cliInput = _cliInput.Remove(_inputPos - cursor.Length - 1, 1);

                                SetCursor(cursor.Length);
                                Console.Write(_cliInput);
                                SetCursor(_inputPos - 1);
                            }
                        }
                        else if (_key.Key == ConsoleKey.LeftArrow)
                        {
                            if (_inputPos > _inputPosMIN)
                            {
                                SetCursor(_inputPos - 1);
                            }
                        }
                        else if (_key.Key == ConsoleKey.RightArrow)
                        {
                            if (_inputPos < _inputPosMAX)
                            {
                                SetCursor(_inputPos + 1);
                            }
                        }
                        else if (_key.Key == ConsoleKey.UpArrow)
                        {
                            if (_cliHistory.Count - 1 > _historyPointer)
                            {
                                _historyPointer++;
                            }

                            string _lastInput = GetLastHistoryEntry(_historyPointer);

                            ClearInput(_cliInput.Length);
                            SetCursor(cursor.Length);
                            Console.Write(_lastInput);

                            _cliInput = _lastInput;
                        }
                        else if (_key.Key == ConsoleKey.DownArrow)
                        {
                            if (0 < _historyPointer)
                            {
                                _historyPointer--;
                            }

                            string _lastInput = GetLastHistoryEntry(_historyPointer);

                            ClearInput(_cliInput.Length);
                            SetCursor(cursor.Length);
                            Console.Write(_lastInput);

                            _cliInput = _lastInput;
                        }
                        else
                        {
                            if (_cliInput.Length < INPUT_MAX)
                            {
                                _cliInput = _cliInput.Insert(_inputPos - cursor.Length, _key.KeyChar.ToString());

                                // Update cli-input.
                                ClearInput(_cliInput.Length);
                                SetCursor(cursor.Length);

                                Console.Write(_cliInput);
                                SetCursor(_inputPos + 1);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    // There are some keyboards, which can make some problems here (spezial keys, ...).
                    // So this try-catch is just used becauce securess
                    return CLIError.Error(CLIError.ErrorType.inputError, "An exception has been thrown: " + e.Message + ".", true);
                }
                
                Console.Write("\n");

                return _cliInput;
            }
        }

        private static string GetLastHistoryEntry(int pointer)
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

        private static void AddToHistory(string command)
        {
            if (command != "")
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
        }

        private static void ClearInput(int cliInputLength)
        {
            Console.SetCursorPosition(cursor.Length, Console.CursorTop);
            Console.Write("".PadLeft(cliInputLength, ' '));
        }

        private static void SetCursor(int pos)
        {
            Console.SetCursorPosition(pos, Console.CursorTop);
        }

        #endregion
    }
}
