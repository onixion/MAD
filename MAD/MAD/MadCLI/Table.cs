using System;

namespace MAD
{
    public class Table
    {
        private string[] columnes;
        private int columneWidth;

        public string line = "".PadRight(Console.BufferWidth, '═');

        public Table(string[] columnes)
        {
            this.columnes = columnes;
            columneWidth = Console.BufferWidth/columnes.Length-1;
        }

        public string[] FormatStringArray(string[] data)
        {
            for (int i = 0; i < columnes.Length; i++)
            {
                // if string length is longer than columne width
                if (data[i].Length > columneWidth)
                {
                    data[i] = data[i].Remove(columneWidth);
                }
                else
                {
                    data[i] = data[i].PadRight(columneWidth);
                }
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
