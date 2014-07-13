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
        public static bool Serialize(string fileName, object objectToSave)
        {
            try
            {
                using (FileStream _stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter _bf = new BinaryFormatter();
                    _bf.Serialize(_stream, objectToSave);

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static object Deserialize(string fileName)
        {
            try
            {
                using (FileStream _stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    BinaryFormatter _bf = new BinaryFormatter();
                    return _bf.Deserialize(_stream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
