using System;

using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    // This class is used by the JobSystem to load and save nodes.

    public static class JSSerializer
    {
        private static object _serializerLock = new object();

        public static void SerializeNode(string fileName, JobNode node)
        {
            lock (_serializerLock)
            {
                JsonSerializerSettings _settings = new JsonSerializerSettings();
                _settings.Converters.Add(new IPAddressConverter());
                _settings.Converters.Add(new MailAddressConverter());
                _settings.Converters.Add(new MacAddressConverter());
                _settings.TypeNameHandling = TypeNameHandling.Auto;
                Serialize(fileName, node, _settings);
            }
        }

        public static JobNode DeserializeNode(string fileName)
        {
            lock (_serializerLock)
            {
                JsonSerializerSettings _settings = new JsonSerializerSettings();
                _settings.Converters.Add(new IPAddressConverter());
                _settings.Converters.Add(new MailAddressConverter());
                _settings.Converters.Add(new MacAddressConverter());
                _settings.TypeNameHandling = TypeNameHandling.Auto;
                return (JobNode)Deserialize(fileName, typeof(JobNode), _settings);
            }
        }

        public static void Serialize(string fileName, object objectToSave, JsonSerializerSettings settings)
        {
            using (FileStream _stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter _writer = new StreamWriter(_stream))
                _writer.Write(JsonConvert.SerializeObject(objectToSave, Formatting.Indented, settings));
        }

        public static object Deserialize(string fileName, Type objectType, JsonSerializerSettings settings)
        {
            using (FileStream _stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            using (StreamReader _reader = new StreamReader(_stream))
                return JsonConvert.DeserializeObject(_reader.ReadToEnd(), objectType, settings);
        }
    }

    /*
     * These classes are needed because there are some classes in NET (IPAddress, IPEndPoint, ...), which
     * are not serialize-friendly. Therefore we need to created for these types of classes custom converters.
     */

    public class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(System.Net.IPAddress))
                return true;
            else
                return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            System.Net.IPAddress _ip = (System.Net.IPAddress)value;
            writer.WriteValue(_ip.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return System.Net.IPAddress.Parse((string)reader.Value);
        }
    }

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
            System.Net.Mail.MailAddress _mail = (System.Net.Mail.MailAddress)value;
            writer.WriteValue(_mail.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return new System.Net.Mail.MailAddress(((string)reader.Value));
        }
    }

    public class MacAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(System.Net.NetworkInformation.PhysicalAddress))
                return true;
            else
                return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            System.Net.NetworkInformation.PhysicalAddress _mac = (System.Net.NetworkInformation.PhysicalAddress)value;
            writer.WriteValue(_mac.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return System.Net.NetworkInformation.PhysicalAddress.Parse((string)reader.Value);
        }
    }
}
