using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MadNet
{
    public class RSA : IDisposable
    {
        private bool _Private;
        private RSACryptoServiceProvider _RSA = new RSACryptoServiceProvider(4096);
        private SHA1CryptoServiceProvider _SHA1 = new SHA1CryptoServiceProvider();

        public RSA() { _Private = true; }
        public RSA(string File) { LoadKeys(File); }
        public RSA(byte[] Modulus, byte[] Exponent) { SetPublicKey(Modulus, Exponent); }

        public void Dispose() { _RSA.Dispose(); }

        public void SaveKeys(string File)
        {
            using (FileStream fs = new FileStream(File, FileMode.Create, FileAccess.Write, FileShare.Read))
            using (StreamWriter writer = new StreamWriter(fs, Encoding.ASCII))
                writer.WriteLine(_RSA.ToXmlString(_Private).Replace("><", ">" + Environment.NewLine + "<"));
        }

        public void LoadKeys(string File)
        {
            using (FileStream fs = new FileStream(File, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (StreamReader reader = new StreamReader(fs, Encoding.ASCII))
                _RSA.FromXmlString(reader.ReadToEnd());
            _Private = !_RSA.PublicOnly;
        }

        public void GetPublicKey(out byte[] Modulus, out byte[] Exponent)
        {
            RSAParameters rsaParams = _RSA.ExportParameters(false);
            Modulus = rsaParams.Modulus;
            Exponent = rsaParams.Exponent;
        }

        public string GetPublicKey()
        {
            RSAParameters rsaParams = _RSA.ExportParameters(false);
            byte[] pubKey = CombineByteArrays(rsaParams.Modulus, rsaParams.Exponent);
            return Convert.ToBase64String(pubKey);
        }

        public void SetPublicKey(byte[] Modulus, byte[] Exponent)
        {
            RSAParameters rsaParams = new RSAParameters()
            {
                Modulus = Modulus,
                Exponent = Exponent
            };
            _Private = false;
            _RSA.ImportParameters(rsaParams);
        }

        public string GetFingerprint()
        {
            RSAParameters rsaParams = _RSA.ExportParameters(false);
            byte[] pubkey = CombineByteArrays(rsaParams.Modulus, rsaParams.Exponent);
            pubkey = _SHA1.ComputeHash(pubkey);
            StringBuilder sb = new StringBuilder(22);
            sb.Append("0x");
            for (byte i = 0; i < 5; i++)
                sb.Append(((byte)(pubkey[i] ^ pubkey[i + 5] ^ pubkey[i + 10] ^ pubkey[i + 15])).ToString("X2"));
            return sb.ToString();
        }

        public byte[] EncryptPublic(byte[] Data) { return _RSA.Encrypt(Data, false); }

        public byte[] DecryptPrivate(byte[] Data)
        {
            if (_Private)
                return _RSA.Decrypt(Data, false);
            else throw new Exception("No private key");
        }

        public byte[] Sign(byte[] Data) { return _RSA.SignData(Data, _SHA1); }

        public bool CheckSignature(byte[] Data, byte[] Signature) { return _RSA.VerifyData(Data, _SHA1, Signature); }

        public byte[] CombineByteArrays(byte[] Array1, byte[] Array2)
        {
            byte[] resultArray = new byte[Array1.Length + Array2.Length];
            Array.Copy(Array1, resultArray, Array1.Length);
            Array.Copy(Array2, 0, resultArray, Array1.Length, Array2.Length);
            return resultArray;
        }
    }
}