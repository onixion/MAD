using System;
using System.IO;

using Newtonsoft.Json;

namespace MAD
{
    /* This class handles the config file (MadConf) */

    public static class MadConf
    {
        #region members

        private static object _confLock = new object();

        public static MadConfigFile conf = new MadConfigFile();
        private static JsonSerializer _ser = new JsonSerializer();

        public static event EventHandler OnConfChange;

        #endregion

        #region methodes

        public static bool ConfDirExist(string dirPath)
        {
            if (Directory.Exists(dirPath))
                return true;
            else
                return false;
        }

        public static bool ConfExist(string filePath)
        {
            if (File.Exists(filePath))
                return true;
            else
                return false;
        }

        public static bool TryCreateDir(string dirPath)
        {
            if (!ConfDirExist(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                return true;
            }
            else
                return false;
        }

        public static void SaveConf(string filePath)
        {
            lock (_confLock)
            {
                using (FileStream _file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (StreamWriter _writer = new StreamWriter(_file))
                    _ser.Serialize(_writer, conf);
            }
        }

        public static void LoadConf(string filePath)
        {
            using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader _reader = new StreamReader(_file))
            {
                JsonReader _jReader = new JsonTextReader(_reader);
                conf = (MadConfigFile)_ser.Deserialize(_jReader, typeof(MadConfigFile));
                
                if(OnConfChange != null)
                    OnConfChange.Invoke(null, null);
            }
        }

        public static void SetToDefault()
        {
            conf.DEBUG_MODE = true;
            conf.LOG_MODE = true;
            conf.SERVER_PORT = 2222;

            conf.smtpServer = "smtp-mail.outlook.com";
            conf.smtpPort = 587;
            conf.username = "mad.group@outlook.com";
            conf.password = "Mad-21436587";
        }

        #endregion
    }

    /* This class contains all variables, which should be saved / loaded from the config file. */

    public class MadConfigFile
    {
        public bool DEBUG_MODE;
        public bool LOG_MODE;
        public int SERVER_PORT;

        public string smtpServer;
        public int smtpPort;
        public string username;
        public string password;
    }
}
