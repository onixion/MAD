using System;
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

        private static ReaderWriterLockSlim _confLock = new ReaderWriterLockSlim();
        private static MadConfigFile _conf = new MadConfigFile();

        /* The idea of be able to load the config-file, while
         * runtime is very cool, but can create many trouble.
         * So far I think we can put this idea onto our
         * 'Maybe-Implement-Later' list.
        public static event EventHandler OnConfChange; */

        #endregion

        #region methodes

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

            _confLock.EnterReadLock();
            try
            {
                using (FileStream _file = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                using (StreamWriter _writer = new StreamWriter(_file))
                    _ser.Serialize(_writer, _conf);
            }
            catch (Exception e)
            {
                _confLock.ExitReadLock();
                throw e;
            }
            _confLock.ExitReadLock();
        }

        public static void LoadConf(string filePath)
        {
            JsonSerializer _ser = new JsonSerializer();
            _ser.Converters.Add(new MailAddressConverter());

            using (FileStream _file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader _reader = new StreamReader(_file))
            {
                JsonReader _jReader = new JsonTextReader(_reader);

                _confLock.EnterWriteLock();
                try { _conf = (MadConfigFile)_ser.Deserialize(_jReader, typeof(MadConfigFile)); }
                catch (Exception e)
                {
                    _confLock.ExitWriteLock();
                    throw e;
                }
                _confLock.ExitWriteLock();
                
                //if(OnConfChange != null)
                  //  OnConfChange.Invoke(null, null);
            }
        }

        public static void SetToDefault()
        {
            _confLock.EnterWriteLock();

            _conf.DEBUG_MODE = true;
            _conf.LOG_MODE = true;

            _conf.SERVER_HEADER = "MAD-CLIServer";
            _conf.SERVER_PORT = 2222;

            _conf.NOTI_ENABLE = true;
            // These settings will be removed for the final version.
            // It does no make sense leaving them here ...
            _conf.SMTP_SERVER = "smtp-mail.outlook.com";
            _conf.SMTP_PORT = 587;
            _conf.SMTP_USER = "mad.group@outlook.com";
            _conf.SMTP_PASS= "Mad-21436587";
            _conf.MAIL_DEFAULT = new MailAddress[1] { new MailAddress("alin.porcic@gmail.com") };

            _confLock.ExitWriteLock();
        }

        #region reference handling

        public static MadConfigFile GetLockedConfRead()
        {
            _confLock.EnterReadLock();
            return _conf;
        }

        public static void UnlockConfRead()
        {
            _confLock.ExitReadLock();
        }

        public static MadConfigFile GetLockedConfWrite()
        {
            _confLock.EnterWriteLock();
            return _conf;
        }

        public static void UnlockConfWrite()
        {
            _confLock.ExitWriteLock();
        }

        #endregion

        #endregion
    }

    /* This class contains all variables, which should be saved / loaded from the config file. */

    public class MadConfigFile
    {
        // global
        public bool DEBUG_MODE;
        public bool LOG_MODE;

        public string SERVER_HEADER;
        public int SERVER_PORT;
        public string SERVER_RSA_KEYS;

        public bool NOTI_ENABLE;
        // default notification-settings
        public string SMTP_SERVER;
        public int SMTP_PORT;
        public string SMTP_USER;
        public string SMTP_PASS;
        public MailAddress[] MAIL_DEFAULT;
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
