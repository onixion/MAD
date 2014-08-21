using System;
using System.IO;

namespace MAD.Logging
{
    public static class Logger
    {
        public static string logFile; 

        private static bool PathFileExists()
        {
            string _pathToPathFile = Directory.GetCurrentDirectory();
            string _content = "lorem ipsum";
            if (File.Exists(_pathToPathFile + @"/path.txt"))
            {
                using (StreamReader sr = new StreamReader(_pathToPathFile + @"/path.txt"))
                    _content = sr.ReadToEnd();

                if (!String.IsNullOrEmpty(_content))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static void CreateNewPathFile()
        {
            string _newPathToPathFile = Path.GetFullPath("./");
            string _pathToLogFile = Path.GetFullPath("./") + @"../../";
            File.Create(_newPathToPathFile + @"/path.txt");

            using (StreamWriter sw = new StreamWriter(_newPathToPathFile + @"/path.txt"))
                sw.WriteLine(_pathToLogFile);
        }
    }
}
