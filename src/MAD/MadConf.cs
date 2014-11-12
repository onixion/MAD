﻿using System;
using System.IO;
using System.Threading;
using System.Net.Mail;

using Newtonsoft.Json;

namespace MAD
{
    /* This class handles the config file. */

    public static class MadConf
    {
        #region members

        public static object confLock = new object();
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
            lock (confLock)
            {
                JsonSerializer _ser = new JsonSerializer();
                _ser.Formatting = Formatting.Indented;
                _ser.Converters.Add(new MailAddressConverter());

                using (FileStream _file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (StreamWriter _writer = new StreamWriter(_file))
                    _ser.Serialize(_writer, conf);
            }
        }

        public static void LoadConf(string filePath)
        {
            JsonSerializer _ser = new JsonSerializer();
            _ser.Converters.Add(new MailAddressConverter());

            using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader _reader = new StreamReader(_file))
            {
                JsonReader _jReader = new JsonTextReader(_reader);

                lock(confLock)
                    conf = (MadConfigFile)_ser.Deserialize(_jReader, typeof(MadConfigFile));

                //if(OnConfChange != null)
                  //  OnConfChange.Invoke(null, null);
            }
        }

        public static void SetToDefault()
        {
            lock (confLock)
            {
                conf.DEBUG_MODE = true;
                conf.LOG_MODE = true;

                conf.SERVER_HEADER = "MAD-CLIServer";
                conf.SERVER_PORT = 2222;
                conf.AES_PASS = "";
                conf.AES_SALT = new byte[] { 32, 23, 12, 54, 42 };

                conf.NOTI_ENABLE = true;
                conf.SMTP_SERVER = "smtp-mail.outlook.com";
                conf.SMTP_PORT = 587;
                conf.SMTP_USER = "mad.group@outlook.com";
                conf.SMTP_PASS = "Mad-21436587";
                conf.MAIL_DEFAULT = new MailAddress[1] { new MailAddress("alin.porcic@gmail.com") };

                conf.arpInterface = 4;
                conf.snmpInterface = "12";

                conf.LOG_FILE_DIRECTORY = Directory.GetCurrentDirectory();
            }
        }

        #endregion
    }

    /* This class contains all variables, which should be saved / loaded from the config file. */
    public class MadConfigFile
    {
        // global
        public bool DEBUG_MODE;
        public bool LOG_MODE;

        // cliserver
        public string SERVER_HEADER;
        public int SERVER_PORT;
        public string AES_PASS;
        public byte[] AES_SALT;

        // notification
        public bool NOTI_ENABLE;
        // global notification
        public string SMTP_SERVER;
        public int SMTP_PORT;
        public string SMTP_USER;
        public string SMTP_PASS;
        public MailAddress[] MAIL_DEFAULT;

        // networking vars
        public uint arpInterface;
		public string snmpInterface;

        // logger var
        public string LOG_FILE_DIRECTORY;
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
