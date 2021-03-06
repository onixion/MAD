﻿using System;
using System.Collections.Generic;

namespace CLIIO
{
    public static class CLIOutput
    {
        #region members

        private static object _outputLock = new object();
        private const string _colorTag = "<color>";
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

        private static ConsoleColor _defaultColor = ConsoleColor.Gray;

        #endregion

        #region methods

        public static void WriteToConsole(string data)
        {
            lock (_outputLock)
            {
                if (data != "")
                {
                    string[] temp = data.Split(new string[] { _colorTag }, StringSplitOptions.None);
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
                                Console.ForegroundColor = _defaultColor;
                                Console.Write(temp[i]);
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = _defaultColor;
                        Console.Write(data);
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        #endregion
    }
}
