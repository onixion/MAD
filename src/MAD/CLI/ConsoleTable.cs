using System;

namespace MAD.CLICore
{
    public static class ConsoleTable
    {
        #region methods

        public static string GetSplitline(int width)
        { 
            return "".PadRight(Console.BufferWidth, '_');
        }

        public static string FormatStringArray(int consoleWidth, params string[] data)
        {
            int _columneWidth = consoleWidth / data.Length;

            if (_columneWidth < 3)
                return "Columne-width to small!";

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i].Length >= _columneWidth)
                {
                    data[i] = data[i].Remove(_columneWidth - 2);
                    data[i] += ".";
                }
                else
                {
                    data[i] = data[i].PadRight(_columneWidth - 1);
                }
            }

            return AppendColumnes(consoleWidth, data);
        }

        private static string AppendColumnes(int consoleWidth, string[] data)
        {
            string _buffer = "";

            foreach (string _temp in data)
            {
                _buffer += _temp;
                _buffer += " ";
            }

            if (_buffer.Length < consoleWidth)
                _buffer = _buffer.PadRight(consoleWidth);

            return _buffer;
        }

        #endregion
    }
}
