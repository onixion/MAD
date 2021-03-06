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

        public static MadConfigFile conf = new MadConfigFile();

        /* The idea of be able to load the config-file, while
         * runtime is very cool, but can create many trouble.
         * So far I think we can put this idea onto our
         * 'Maybe-Implement-Later' list.
        public static event EventHandler OnConfChange; */

        #endregion

        #region methods

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

        public static bool LoadConf(string filePath)
        {
            try
            {
                JsonSerializer _ser = new JsonSerializer();
                _ser.Converters.Add(new MailAddressConverter());

                using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader _reader = new StreamReader(_file))
                {
                    JsonReader _jReader = new JsonTextReader(_reader);
                    conf = (MadConfigFile)_ser.Deserialize(_jReader, typeof(MadConfigFile));
                }

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public static void SetToDefault()
        {
            // logger
            conf.LOGPATH = Path.Combine(Mad.DATADIR, "log.txt");
            conf.LOG_LEVEL = 2;

            // jobsystem
            conf.NOTI_ENABLE = true;

            // notification
            conf.SMTP_SERVER = "smtp-mail.outlook.com";
            conf.SMTP_PORT = 587;
            conf.SMTP_USER = new MailAddress("mad.group@outlook.com");
            conf.SMTP_PASS = "MAD-21436587";
            conf.MAIL_DEFAULT = new MailAddress[1] { new MailAddress("singh.manpreet@live.at") };

            // cliserver
            conf.SERVER_HEADER = "MAD-CLIServer";
            conf.SERVER_PORT = 2222;
            conf.AES_PASS = "PASSWORT";

            conf.arpInterface = 2;
            conf.snmpInterface = "12";
            conf.defaultHostname = "www.google.at";
        }

        #endregion
    }

    /* This class contains all READ-only config-variables. They are saved / loaded from the config file. */
    public class MadConfigFile
    {
        // logger
        public string LOGPATH;
        public uint LOG_LEVEL;

        // jobsystem
        public bool NOTI_ENABLE;

        // notification
        public string SMTP_SERVER;
        public int SMTP_PORT;
        public MailAddress SMTP_USER;
        public string SMTP_PASS;
        public MailAddress[] MAIL_DEFAULT;

        // cliserver
        public string SERVER_HEADER;
        public int SERVER_PORT;
        public string AES_PASS;

        // networking vars
        public uint arpInterface;
		public string snmpInterface;
        public string defaultHostname;
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
