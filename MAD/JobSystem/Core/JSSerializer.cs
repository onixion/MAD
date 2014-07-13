using System;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MAD.JobSystemCore
{
    // This class is used by the JobSystem to load and save
    // JobNodes into files.

    public static class JSSerializer
    {
        public static void Serialize(string fileName, object objectToSave)
        {
            using (FileStream _stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter _bf = new BinaryFormatter();
                _bf.Serialize(_stream, objectToSave);
            }
        }

        public static object Deserialize(string fileName)
        {
            using (FileStream _stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                BinaryFormatter _bf = new BinaryFormatter();
                return _bf.Deserialize(_stream);
            }
        }
    }
}
