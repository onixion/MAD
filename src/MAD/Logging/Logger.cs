using System;
using System.Collections.Generic;
using System.IO;

namespace MAD.Logging
{
    public static class Logger
    {
        #region fields
        
        public static uint buffer = 20;

        public static string logFileName = "log.txt";

        public enum MessageType
        {
            EMERGENCY,
            ERROR,
            WARNING,
            INFORM
        }

        private static string _pathToLogFile;

        private static List<string> _logMessages = new List<string>();

        private static readonly Object _lockThis = new Object();

        private static bool _force = false;
        #endregion

        #region methods
        public static bool PathFileExists()
        {
            string _pathToPathFile = ".";
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
            string _pathToLogFile = _newPathToPathFile;
            using (File.Create(_newPathToPathFile + @"/path.txt"));

            using (StreamWriter sw = new StreamWriter(_newPathToPathFile + @"/path.txt", false))
                sw.WriteLine(_pathToLogFile);
        }

        public static void CreateNewPathFile(string pathExtension)
        {
            string _newPathToPathFile = Path.GetFullPath("./");
            string _newpathToLogFile = Path.Combine(Path.GetFullPath("./"), pathExtension);
            using (File.Create(_newPathToPathFile + @"/path.txt")) ;

            using (StreamWriter sw = new StreamWriter(_newPathToPathFile + @"/path.txt", false))
                sw.WriteLine(_newpathToLogFile);

            _pathToLogFile = _newpathToLogFile;
        }

        public static void ReadPathToLogFile()
        {
            string _pathToPathFile = Path.GetFullPath("./");
            using (StreamReader sr = new StreamReader(_pathToPathFile + @"/path.txt"))
                _pathToLogFile = sr.ReadLine();
        }

        public static void Log(string message, MessageType type)
        {
            lock (_lockThis)
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
                    case MessageType.WARNING:
                        _buffer += " WARNING: ";
                        break;
                }

                _buffer += message;

                _logMessages.Add(_buffer);

                WriteToLog();
            }
        }

        private static void WriteToLog()
        {
            if (_logMessages.Count >= buffer || _force)
            {
                lock (_lockThis)
                {
                    File.AppendAllLines(_pathToLogFile + @"/" + logFileName, _logMessages.ToArray());
                    _logMessages.Clear();
                }
            }
        }

        public static void ForceWriteToLog()
        {
            _force = true;
            WriteToLog();
            _force = false;
        }
        #endregion 
    }
}
