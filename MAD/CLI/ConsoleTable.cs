using System;

namespace MAD
{
    public class ConsoleTable
    {
        private string[] columnesTitles;
        private int columneWidth;
        public string splitline = "".PadRight(Console.BufferWidth, '_');

        public ConsoleTable(string[] columnesTitles, int consoleWidth)
        {
            this.columnesTitles = columnesTitles;
            columneWidth = consoleWidth / columnesTitles.Length - 1;
        }

        #region methodes

        public string[] FormatStringArray(string[] data)
        {
            for (int i = 0; i < columnesTitles.Length; i++)
            {
                if (data[i].Length > columneWidth)
                    data[i] = data[i].Remove(columneWidth);
                else
                    data[i] = data[i].PadRight(columneWidth);
            }
            return data;
        }

        public string WriteColumnes(string[] data)
        {
            string buffer = "";

            foreach (string temp in data)
            {
                buffer += temp;
                buffer += " ";
            }

            return buffer;
        }

        #endregion
    }
}
