using System;

namespace MAD
{
    public class Table
    {
        private string[] columnes;
        private int columneWidth;

        public Table(string[] columnes)
        {
            this.columnes = columnes;
            columneWidth = Console.BufferWidth/columnes.Length-1;
            // NOT WORKING YET THIS SHIT
            for(int i = 0; i < columnes.Length; i++)
            {
                if (columnes[i].Length > columnes.Length)
                {
                    columnes[i] = columnes[i].Remove(columneWidth);
                }
                else
                {
                    columnes[i] = columnes[i].PadRight(columneWidth - columnes[i].Length + 2, ' ');
                }
            }
        }

        public void WriteColumnes()
        {
            foreach (string temp in columnes)
            {
                Console.Write(temp);
                Console.Write("|");
            }
        }

    }
}
