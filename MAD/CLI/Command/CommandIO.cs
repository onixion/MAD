using System;
using System.Collections.Generic;
using System.Threading;

namespace MAD.cli
{
    public static class CommandIO
    {
        private static object _consoleLock = new object();

        #region Colors

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

        public static void WriteToConsole(string data)
        {
            lock (_consoleLock)
            {
                if (data != "")
                {
                    string[] temp = data.Split(new string[] { colorTag }, StringSplitOptions.None);

                    for (int i = 0; i < temp.Length; i++)
                    {
                        foreach (object[] buffer in colors)
                        {
                            string color = (string)buffer[0];

                            if (temp[i].StartsWith(color))
                            {
                                temp[i] = temp[i].Remove(0, color.Length);
                                Console.ForegroundColor = (ConsoleColor)buffer[1];
                                Console.Write(temp[i]);
                                break;
                            }
                        }
                    }

                    Console.Write("\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        public static string ReadFromConsole()
        {
            lock (_consoleLock)
            {
                return Console.ReadLine();
            }
        }
    }
}
