using System;
using System.IO;

using Newtonsoft.Json;

namespace MAD
{
    public static class MadConf
    {
        private static object _confLock = new object();
        private static JsonSerializer _ser = new JsonSerializer();

        public static MadConfigFile conf = new MadConfigFile();
        public static event EventHandler OnConfChange;

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

        public static bool TryCreateConf(string filePath)
        {
            if (!ConfExist(filePath))
            {
                try
                {
                    lock (_confLock)
                    {
                        using (FileStream _file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                        using (StreamWriter _writer = new StreamWriter(_file))
                            _ser.Serialize(_writer, conf);
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
            else
                return false;
        }

        public static bool TryReadConf(string filePath)
        {
            if (ConfExist(filePath))
            {
                try
                {
                    using (StreamReader _reader = new StreamReader(filePath))
                    {
                        JsonReader _jReader = new JsonTextReader(_reader);
                        conf = (MadConfigFile)_ser.Deserialize(_jReader, typeof(MadConfigFile));
                        OnConfChange.Invoke(null, null);
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
            else
                return false;
        }

        public static void SetToDefault()
        {
            conf.VERSION = "v0.0.6.0";
        }
    }

    public class MadConfigFile
    {
        public string VERSION;
    }
}
