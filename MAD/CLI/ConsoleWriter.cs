using System;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class ConsoleWriter
    {
        private const string colorTag = "<color>";

        // supported colors (for now)
        public List<object[]> colors = new List<object[]>()
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

        public void WriteToConsole(string text)
        {
            string[] temp = text.Split(new string[] { colorTag }, StringSplitOptions.None);

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

            // set color to default
            Console.ForegroundColor = ConsoleColor.Gray;
        }


    }
}
