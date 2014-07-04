using System;
using System.Collections.Generic;

namespace MAD.CLIIO
{
    /*
     * This class is responsible for the input of the cli.
     * It defines what happend when keys are pressed. (TAB, ENTER, ...) 
    */

    public class CLIInput
    {
        private ConsoleColor _cursorColor = ConsoleColor.Cyan;
        private ConsoleColor _inputColor = ConsoleColor.White;

        private const int INPUT_MAX = 100;

        public string cursor = "MAD> ";
        private string _cliInput;

        private int _inputPos;
        private int _inputPosMIN;
        private int _inputPosMAX;

        private List<string> _cliHistory = new List<string>();
        private int _maxHistoryEntries = 5;
        private int _historyPointer;

        public void WriteCursor()
        {
            Console.ForegroundColor = _cursorColor;
            Console.Write(cursor);
        }

        public string ReadInput()
        {
            Console.ForegroundColor = _inputColor;

            _cliInput = "";
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
                    // Tab does nothing.
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
