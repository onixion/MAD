using System;

using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace MAD.JobSystemCore
{
    // This class is used by the JobSystem to load and save nodes.
    // JobNodes into files.

    public static class JSSerializer
    {
        private static object _serializerLock = new object();
        private static XmlSerializer _ser;

        public static void Serialize(string fileName, object objectToSave)
        {
            lock (_serializerLock)
                using (FileStream _stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    //BinaryFormatter _bf = new BinaryFormatter();
                    //_bf.Serialize(_stream, objectToSave);

                    _ser = new XmlSerializer(objectToSave.GetType());
                    _ser.Serialize(_stream, objectToSave);
                }
        }

        public static object Deserialize(string fileName, Type objectType)
        {
            lock (_serializerLock)
                using (FileStream _stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    //BinaryFormatter _bf = new BinaryFormatter();
                    //return _bf.Deserialize(_stream);

                    _ser = new XmlSerializer(objectType);
                    return _ser.Deserialize(_stream);
                }
        }
    }
}
