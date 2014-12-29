using System;
using System.IO;
using System.Threading;
using System.Net.Mail;

using Newtonsoft.Json;

namespace MadProbe
{
    /* This class handles the config file. */

    public static class MadConf
    {
        #region members

        public static MadConfigFile conf = new MadConfigFile();

        /* The idea of be able to load the config-file, while
         * runtime is very cool, but can create many trouble.
         * So far I think we can put this idea onto our
         * 'Maybe-Implement-Later' list.
        public static event EventHandler OnConfChange; */

        #endregion

        #region methods

        public static bool TryCreateDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                return true;
            }
            else
                return false;
        }

        public static void SaveConf(string filePath)
        {
            JsonSerializer _ser = new JsonSerializer();
            _ser.Formatting = Formatting.Indented;
            _ser.Converters.Add(new MailAddressConverter());

            using (FileStream _file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (StreamWriter _writer = new StreamWriter(_file))
                _ser.Serialize(_writer, conf);
        }

        public static void OverwriteConf(string filePath, MadConfigFile conf)
        {
            JsonSerializer _ser = new JsonSerializer();
            _ser.Formatting = Formatting.Indented;
            _ser.Converters.Add(new MailAddressConverter());

            using (FileStream _file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter _writer = new StreamWriter(_file))
                _ser.Serialize(_writer, conf);
        }

        public static void LoadConf(string filePath)
        {
            JsonSerializer _ser = new JsonSerializer();
            _ser.Converters.Add(new MailAddressConverter());

            using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader _reader = new StreamReader(_file))
            {
                JsonReader _jReader = new JsonTextReader(_reader);
                conf = (MadConfigFile)_ser.Deserialize(_jReader, typeof(MadConfigFile));
            }
        }

        public static void SetToDefault()
        {
            conf.aesPass = "PASSWORD";
            conf.port = 2221;
        }

        #endregion
    }

    /* This class contains all READ-only config-variables. They are saved / loaded from the config file. */
    public class MadConfigFile
    {
        public string aesPass;
        public int port;
    }

    /* json converters */
    public class MailAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(System.Net.Mail.MailAddress))
                return true;
            else
                return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            System.Net.Mail.MailAddress _mail = (MailAddress)value;
            writer.WriteValue(_mail.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new MailAddress(((string)reader.Value));
        }
    }
}
