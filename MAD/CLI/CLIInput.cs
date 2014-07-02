using System;
using System.Collections.Generic;

namespace MAD.cli
{
    public class CLIInput
    {
        public string cursor = "=> ";
        private string _cliInput = "";
        private int _inputPos = 0;

        private List<string> _cliHistory = new List<string>();
        private int _maxHistoryEntries = 5;
        private int _historyPointer = -1;

        public string ReadInput()
        {
            _cliInput = "";
            _historyPointer = -1;

            while (true)
            {
                ConsoleKeyInfo _key = Console.ReadKey();
                _inputPos = Console.CursorLeft;

                if (_key.Key == ConsoleKey.Enter)
                {
                    // Add cli-input to history.
                    AddToHistory(_cliInput);

                    // Return cli-input.
                    break;
                }
                else if (_key.Key == ConsoleKey.Tab)
                {
                   // TODO
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
                    if (_inputPos == cursor.Length - 1)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        if (_cliInput.Length >= 1)
                        {
                            int _cliInputOldLength = _cliInput.Length;
                            _cliInput = _cliInput.Remove(_inputPos - cursor.Length, 1);

                            ClearInput(_cliInputOldLength);
                            SetCursor(cursor.Length);
                            Console.Write(_cliInput);

                            SetCursor(_inputPos);
                        }
                    }
                }
                else if (_key.Key == ConsoleKey.LeftArrow)
                {
                    ClearInput(_cliInput.Length);
                    SetCursor(cursor.Length);
                    Console.Write(_cliInput);

                    if (_inputPos != cursor.Length + 1)
                    {
                        SetCursor(_inputPos - 2);
                    }
                }
                else if (_key.Key == ConsoleKey.RightArrow)
                {
                    ClearInput(_cliInput.Length);
                    SetCursor(cursor.Length);
                    Console.Write(_cliInput);

                    if (_inputPos < _cliInput.Length + 1)
                    {
                        SetCursor(_inputPos);
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

                    ClearInput(_cliInput.Length);
                    SetCursor(cursor.Length);
                    Console.Write(_lastInput);

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

                    ClearInput(_cliInput.Length);
                    SetCursor(cursor.Length);
                    Console.Write(_lastInput);

                    _cliInput = _lastInput;
                }
                else
                {
                    _cliInput = _cliInput.Insert(_inputPos - (cursor.Length + 1), _key.KeyChar.ToString());

                    ClearInput(cursor.Length);
                    SetCursor(cursor.Length);
                    Console.Write(_cliInput);
                    SetCursor(_inputPos);
                }
            }

            Console.Write("\n");
            return _cliInput;
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

        private void ClearInput(int cliInputLength)
        {
            Console.SetCursorPosition(cursor.Length, Console.CursorTop);
            Console.Write("".PadLeft(cliInputLength, ' '));
        }

        private void SetCursor(int pos)
        {
            Console.SetCursorPosition(pos, Console.CursorTop);
        }
    }
}
