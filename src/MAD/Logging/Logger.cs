using System;
using System.Collections.Generic;
using System.IO;

using MAD;

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

        private static List<string> _logMessages = new List<string>();

        private static readonly Object _lockThis = new Object();

        private static bool _force = false;
        #endregion

        #region methods
        
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
                 case MessageType.WARNING:
                     _buffer += " WARNING: ";
                     break;
             }

             _buffer += message;
             lock (_lockThis)
             {
                 _logMessages.Add(_buffer);
                 WriteToLog();
             }           
        }

        private static void WriteToLog()
        {
            if (_logMessages.Count >= buffer || _force)
            {
                File.AppendAllLines(MadConf.conf.LOG_FILE_DIRECTORY + @"/" + logFileName, _logMessages.ToArray());
                _logMessages.Clear();
            }
        }

        public static void ForceWriteToLog()
        {
            lock (_lockThis)
            {
                _force = true;
                WriteToLog();
                _force = false;
            }
        }
        #endregion 
    }
}
