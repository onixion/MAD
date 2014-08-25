using System;
using System.Text;
using System.Security.Cryptography;

namespace MadNet
{
    public static class MD5Hashing
    {
        public static string GetHash(string data)
        {
            MD5 _m = new MD5CryptoServiceProvider();
            byte[] _data = Encoding.Unicode.GetBytes(data);
            return Encoding.Unicode.GetString(_m.ComputeHash(_data, 0, _data.Length));
        }
    }
}
