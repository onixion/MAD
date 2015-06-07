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
        public static FileStream file;
        public static TextWriter writer;

        public enum MessageType
        {
            EMERGENCY = 5,
            ERROR = 4,
            WARNING = 3,
            INFORM = 2,
            DEBUG = 1
        }

        private static List<string> _logMessages = new List<string>();
        private static Object _lockThis = new Object();
        private static uint _logLevel = MadConf.conf.LOG_LEVEL; 
        
        #endregion

        #region methods

        public static void Init()
        {
            file = new FileStream(MadConf.conf.LOGPATH, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            writer = new StreamWriter(file);
        }

        public static void Dispose()
        {
            file.Flush();
            writer.Close();
            file.Close();
        }

        public static void Log(string message, MessageType type)
        {
            if ((uint)type >= _logLevel)
            {
                string _buffer = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:fff");

                switch (type)
                {
                    case MessageType.DEBUG:
                        _buffer += " DEBUG: ";
                        break;
                    case MessageType.EMERGENCY:
                        _buffer += " EMER.: ";
                        break;
                    case MessageType.ERROR:
                        _buffer += " ERROR: ";
                        break;
                    case MessageType.INFORM:
                        _buffer += " INFO:  ";
                        break;
                    case MessageType.WARNING:
                        _buffer += " WARN.: ";
                        break;
                    default:
                        break;
                }

                _logMessages.Add(_buffer + message);

                if (_logMessages.Count >= buffer)
                {
					lock(_lockThis)
					{
						foreach(string temp in _logMessages)
                            	writer.WriteLine(temp);
                    	_logMessages.Clear();
					}
				}
            }
        }

        public static void ForceWriteToLog()
        {
            lock (_lockThis)
            {
                foreach (string temp in _logMessages)
                    writer.WriteLine(temp);
                _logMessages.Clear();
            }
        }
        #endregion 
    }
}
