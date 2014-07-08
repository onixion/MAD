using System;
using System.Collections.Generic;

namespace MAD.CLIIO
{
    public static class CLIOutput
    {
        // TODO: text wraping

        #region members

        public static object _consoleLock = new object();

        private const string colorTag = "<color>";

        public static List<object[]> colors = new List<object[]>()
        {
            new object[] { "<blue>" , ConsoleColor.Blue },
            new object[] { "<cyan>" , ConsoleColor.Cyan },
            new object[] { "<darkblue>" , ConsoleColor.DarkBlue },
            new object[] { "<darkcyan>" , ConsoleColor.DarkCyan },
            new object[] { "<darkgray>" , ConsoleColor.DarkGray },
            new object[] { "<darkgreen>" , ConsoleColor.DarkGreen },
            new object[] { "<darkmagenta>" , ConsoleColor.DarkMagenta },
            new object[] { "<darkred>" , ConsoleColor.DarkRed },
            new object[] { "<darkyellow>" , ConsoleColor.DarkYellow },
            new object[] { "<gray>" , ConsoleColor.Gray },
            new object[] { "<green>" , ConsoleColor.Green },
            new object[] { "<magenta>" , ConsoleColor.Magenta },
            new object[] { "<red>" , ConsoleColor.Red },
            new object[] { "<white>" , ConsoleColor.White },
            new object[] { "<yellow>" , ConsoleColor.Yellow }
        };

        #endregion

        #region methodes

        public static void WriteToConsole(string data)
        {
            lock (_consoleLock)
            {
                if (data != "")
                {
                    string[] temp = data.Split(new string[] { colorTag }, StringSplitOptions.None);
                    bool _colorTagFound;

                    if (temp.Length != 1)
                    {
                        for (int i = 0; i < temp.Length; i++)
                        {
                            _colorTagFound = false;

                            foreach (object[] buffer in colors)
                            {
                                string color = (string)buffer[0];

                                if (temp[i].StartsWith(color))
                                {
                                    temp[i] = temp[i].Remove(0, color.Length);

                                    Console.ForegroundColor = (ConsoleColor)buffer[1];
                                    Console.Write(temp[i]);

                                    _colorTagFound = true;
                                    break;
                                }
                            }

                            // If no color-tag was found, write it in gray.
                            if (!_colorTagFound)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.Write(temp[i]);
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(data);
                    }

                    Console.Write("\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        // Removed, because it cannot be used in combination with colors ...
        /*
        public static void WrapTextToConsole(string textToPrint, int lineWidth)
        {
            string[] _words = textToPrint.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string _buffer = null;

            for (int i = 0; i < _words.Length; i++)
            {
                if (_buffer == null)
                {
                    _buffer = "";
                }

                // Add one word to buffer.
                _buffer += _words[i];

                // Check if the buffer length is equal or bigger than the line width.
                if (_buffer.Length > lineWidth)
                {
                    if (_buffer.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length != 1)
                    {
                        // Remove last word from buffer.
                        _buffer = _buffer.Remove(_buffer.Length - _words[i].Length - 1);
                        // Write it to console.
                        Console.Write(_buffer);
                        // Clear buffer.
                        _buffer = null;
                        // Set i one back.
                        i--;
                    }
                    else
                    { 
                        // If the line has one word and it is to long for the line, than just write it into the console.
                        Console.Write(_buffer);
                        // Clear buffer.
                        _buffer = null;
                    }
                }
                else if (_buffer.Length == lineWidth)
                {
                    Console.Write(_buffer);
                    _buffer = null;
                }
                else
                {
                    // Check if it is the last word.
                    if (_words.Length - 1 == i)
                    {
                        Console.Write(_buffer);
                        break;
                    }
                    else
                    {
                        _buffer += " ";
                    }
                }
            }
        }*/

        #endregion
    }
}
