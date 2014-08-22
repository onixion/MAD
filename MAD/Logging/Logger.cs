using System;
using System.Collections.Generic;
using System.IO;

namespace MAD.Logging
{
    public static class Logger
    {
        public static string pathToLogFile;

        public static enum MessageType
        {
            EMERGENCY,
            ERROR,
            WARNING,
            MESSAGE,
            INFORM
        }

        private static List<string> logMessages; 

        public static bool PathFileExists()
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
            string _pathToLogFile = Path.Combine(Path.GetFullPath("./"), @"../../");
            using (File.Create(_newPathToPathFile + @"/path.txt"));

            using (StreamWriter sw = new StreamWriter(_newPathToPathFile + @"/path.txt"))
                sw.WriteLine(_pathToLogFile);

            pathToLogFile = _pathToLogFile;
        }

        public static void ReadPathToLogFile()
        {
            string _pathToPathFile = Path.GetFullPath("./");
            using (StreamReader sr = new StreamReader(_pathToPathFile + @"/path.txt"))
                pathToLogFile = sr.ReadLine();
        }

        public static void Log(string message, MessageType type)
        {
            string _buffer = ""; 
            _buffer += DateTime.Now.ToString(); 

            switch (type)
            {
                case MessageType.EMERGENCY:
                    _buffer += " !EMERGENCY: ";
                    break;
                case MessageType.ERROR:
                    _buffer += " ERROR: ";
                    break;
                case MessageType.INFORM:
                    _buffer += " INFORMATION: ";
                    break;
                case MessageType.MESSAGE:
                    _buffer += " MESSAGE: ";
                    break;
                case MessageType.WARNING:
                    _buffer += " WARNING: ";
                    break;
            }

            _buffer += message;

            logMessages.Add(_buffer);

            WriteToLog();
        }

        private static void WriteToLog()
        {

        }
    }
}
