using System;
using System.Collections.Generic;

namespace CLIIO
{
    /*
     * This class is responsible for the input of the cli.
     * It defines what happend when keys are pressed. (TAB, ENTER, ...) 
    */

    public static class CLIInput
    {
        #region members

        private static object _inputLock = new object();

        private static string _VINPUT;
        private static int _HEAD;
        private static int _VPOS;
        private static int _MIN;
        private static int _MAX;

        private static List<string> _cliHistory = new List<string>();
        private const int _maxHistoryEntries = 5;
        private static int _historyPointer;

        #endregion

        #region methods

        public static string ReadInput(int offset)
        {
            lock (_inputLock)
            {
                Console.SetCursorPosition(offset, Console.CursorTop);

                _HEAD = offset;
                _VINPUT = "";
                _MIN = _HEAD;
                _historyPointer = -1; 

                try
                {
                    while (true)
                    {
                        ConsoleKeyInfo _key = Console.ReadKey(true);

                        _VPOS = Console.CursorLeft;
                        _MAX = _HEAD + _VINPUT.Length;

                        if (_key.Key == ConsoleKey.Enter)
                        {
                            AddToHistory(_VINPUT);
                            _historyPointer = -1;
                            break;
                        }
                        else if (_key.Key == ConsoleKey.Tab)
                        {
                            if (_key.Modifiers == ConsoleModifiers.Control)
                            {

                                ClearInput();

                                _VINPUT = "KASE JA WC HER";

                                SetCursor(_HEAD);
                                Console.Write(_VINPUT);
                            }
                        }
                        else if (_key.Key == ConsoleKey.Escape)
                        {
                            _VINPUT = "";

                            ClearInput();
                            SetCursor(_HEAD);
                        }
                        else if (_key.Key == ConsoleKey.Backspace)
                        {
                            if (_VPOS > _HEAD)
                            {
                                _VINPUT = _VINPUT.Remove(_VPOS - _HEAD - 1, 1);

                                ClearInput();
                                SetCursor(_HEAD);
                                Console.Write(_VINPUT);
                                SetCursor(_VPOS - 1);
                            }
                        }
                        else if (_key.Key == ConsoleKey.LeftArrow)
                        {
                            if (_VPOS > _MIN)
                                SetCursor(_VPOS - 1);
                        }
                        else if (_key.Key == ConsoleKey.RightArrow)
                        {
                            if (_VPOS < _MAX)
                                SetCursor(_VPOS + 1);
                        }
                        else if (_key.Key == ConsoleKey.UpArrow)
                        {
                            if (_cliHistory.Count != 0)
                            {
                                if (_historyPointer < _cliHistory.Count - 1)
                                {
                                    _historyPointer++;
                                    ClearInput();
                                    _VINPUT = GetHistoryEntry(_historyPointer);

                                    SetCursor(_HEAD);
                                    Console.Write(_VINPUT);
                                }
                            }
                        }
                        else if (_key.Key == ConsoleKey.DownArrow)
                        {
                            if (_cliHistory.Count != 0)
                            {
                                if (_historyPointer >= 1)
                                {
                                    _historyPointer--;
                                    ClearInput();
                                    _VINPUT = GetHistoryEntry(_historyPointer);

                                    SetCursor(_HEAD);
                                    Console.Write(_VINPUT);
                                }
                            }
                        }
                        else
                        {
                            if (_VINPUT.Length < Console.BufferWidth - _HEAD - 2)
                            {
                                ClearInput();
                                _VINPUT = _VINPUT.Insert(_VPOS - _HEAD, _key.KeyChar.ToString());

                                SetCursor(_HEAD);
                                Console.Write(_VINPUT);
                                SetCursor(_VPOS + 1);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    SetCursor(_HEAD);
                }
                
                Console.Write("\n");
                return _VINPUT.Trim();
            }
        }

        private static string GetHistoryEntry(int pointer)
        {
            return _cliHistory[_cliHistory.Count - 1 - pointer];
        }

        private static void AddToHistory(string command)
        {
            if (command != "")
            {
                if (_cliHistory.Contains(command))
                    _cliHistory.Remove(command);

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

        private static void ClearInput()
        {
            Console.SetCursorPosition(_HEAD, Console.CursorTop);
            Console.Write("".PadLeft(_VINPUT.Length + 1, ' '));
        }

        private static void SetCursor(int pos)
        {
            Console.SetCursorPosition(pos, Console.CursorTop);
        }

        #endregion
    }
}
