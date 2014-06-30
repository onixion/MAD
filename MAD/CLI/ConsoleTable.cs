using System;

namespace MAD
{
    public class ConsoleTable
    {
        #region members

        private string[] _columnesTitles;
        private int _columneWidth;
        public string splitline = "".PadRight(Console.BufferWidth, '_');

        #endregion

        #region constructor

        public ConsoleTable(string[] columnesTitles, int consoleWidth)
        {
            _columnesTitles = columnesTitles;
            _columneWidth = consoleWidth / columnesTitles.Length - 1;
        }

        #endregion

        #region methodes

        public string FormatStringArray(string[] data)
        {
            for (int i = 0; i < _columnesTitles.Length; i++)
            {
                if (data[i].Length > _columneWidth)
                {
                    data[i] = data[i].Remove(_columneWidth);
                }
                else
                {
                    data[i] = data[i].PadRight(_columneWidth);
                }
            }

            return AppendColumnes(data);
        }

        private string AppendColumnes(string[] data)
        {
            string _buffer = "";

            foreach (string _temp in data)
            {
                _buffer += _temp;
                _buffer += " ";
            }

            return _buffer;
        }

        #endregion
    }
}
