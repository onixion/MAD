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

        #region methodes

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
                            break;
                        }
                        else if (_key.Key == ConsoleKey.Tab)
                        {
                            if (_key.Modifiers == ConsoleModifiers.Control)
                            {
                                ClearInput();

                                _VINPUT = "Jack was here ...";

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
                            _VINPUT = GetLastHistoryEntry(_historyPointer);

                            if (_cliHistory.Count - 1 > _historyPointer)
                                _historyPointer++;

                            ClearInput();
                            SetCursor(_HEAD);
                            Console.Write(_VINPUT);
                        }
                        else if (_key.Key == ConsoleKey.DownArrow)
                        {
                            _VINPUT = GetLastHistoryEntry(_historyPointer);

                            if (0 < _historyPointer)
                                _historyPointer--;

                            ClearInput();
                            SetCursor(_HEAD);
                            Console.Write(_VINPUT);
                        }
                        else
                        {
                            if (_VINPUT.Length < Console.BufferWidth - _HEAD - 2)
                            {
                                _VINPUT = _VINPUT.Insert(_VPOS - _HEAD, _key.KeyChar.ToString());

                                ClearInput();
                                SetCursor(_HEAD);
                                Console.Write(_VINPUT);
                                SetCursor(_VPOS + 1);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // There are some keyboards, which can make some problems here (spezial keys, ...).
                    // So this try-catch is just used becauce of security-reasons ..
                    throw new Exception("KEYBOARD-ERROR");
                }
                
                Console.Write("\n");

                return _VINPUT;
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

        private static void ClearInput()
        {
            Console.SetCursorPosition(_HEAD, Console.CursorTop);
            Console.Write("".PadLeft(_VINPUT.Length + 1, ' '));
        }

        private static void SetCursor(int pos)
        {
            Console.SetCursorPosition(pos, Console.CursorTop);
        }

        private static int GetVPOS()
        {
            return 0;
        }

        #endregion
    }
}
