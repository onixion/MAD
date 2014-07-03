using System;
using System.Text;
using System.Security.Cryptography;

namespace MAD
{
    public static class MD5Hashing
    {
        public static string ToMD5(string data)
        {
            MD5 _m = new MD5CryptoServiceProvider();
            byte[] _data = Encoding.Unicode.GetBytes(data);
            return Encoding.Unicode.GetString(_m.ComputeHash(_data, 0, _data.Length));
        }
    }
}
