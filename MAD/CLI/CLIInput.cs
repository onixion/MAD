using System;
using System.Collections.Generic;

namespace MAD.CLIIO
{
    public class CLIInput
    {
        private ConsoleColor _cursorColor = ConsoleColor.Cyan;
        private ConsoleColor _inputColor = ConsoleColor.White;

        public string cursor = "=> ";

        private string _cliInput;

        private int _inputPosReal;
        private int _inputPosVirtual;
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
            lock (cursor)
            {
                Console.ForegroundColor = _inputColor;

                _cliInput = "";
                _historyPointer = -1;

                _inputPosReal = cursor.Length;
                _inputPosVirtual = cursor.Length;

                _inputPosMIN = cursor.Length + 1;

                Console.SetCursorPosition(_inputPosReal, Console.CursorTop);

                while (true)
                {
                    ConsoleKeyInfo _key = Console.ReadKey();

                    _inputPosReal = Console.CursorLeft;
                    _inputPosMAX = cursor.Length + _cliInput.Length + 2;
                    // HERE
                    if (_key.Key == ConsoleKey.Enter)
                    {
                        // Add cli-input to history.
                        AddToHistory(_cliInput);
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
                        if (_inputPos == _inputPosMIN)
                        {
                            Console.Write(" ");
                        }
                        else
                        {
                            int _cliInputOldLength = _cliInput.Length;
                            _cliInput = _cliInput.Remove(_inputPos - cursor.Length, 1);

                            ClearInput(_cliInputOldLength);

                            SetCursor(cursor.Length);
                            Console.Write(_cliInput);

                            SetCursor(_inputPos);
                        }
                    }
                    else if (_key.Key == ConsoleKey.LeftArrow)
                    {
                        ClearInput(_cliInput.Length);

                        SetCursor(cursor.Length);
                        Console.Write(_cliInput);

                        if (_inputPos != _inputPosMIN)
                        {
                            SetCursor(_inputPos - 2);
                        }
                        else
                        {
                            SetCursor(_inputPos - 1);
                        }
                    }
                    else if (_key.Key == ConsoleKey.RightArrow)
                    {
                        ClearInput(_cliInput.Length);

                        SetCursor(cursor.Length);
                        Console.Write(_cliInput);

                        if (_inputPos != _inputPosMAX)
                        {
                            
                        }
                        else
                        {
                            SetCursor(_inputPos - 1);
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
