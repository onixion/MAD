using System;

namespace MAD
{
    public class ConsoleTable
    {
        private string[] columnesTitles;
        private int columneWidth;

        public string splitline = "".PadRight(Console.BufferWidth, '═');

        public ConsoleTable(string[] columnes)
        {
            this.columnesTitles = columnes;
            columneWidth = Console.BufferWidth / columnesTitles.Length - 1;
        }

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

        public void WriteColumnes(string[] data)
        {
            foreach (string temp in data)
            {
                Console.Write(temp);
                Console.Write(" ");
            }
        }
    }
}
