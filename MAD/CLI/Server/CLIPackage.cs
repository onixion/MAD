using System;
using System.Security.Cryptography;


namespace MAD.CLI.Server
{
    public class CLIPackage
    {
        private byte[] _version = new byte[8];
        private byte[] _packageID = new byte[8];
        private byte[] _data;

        public CLIPackage(byte[] versionNumber, byte[] packageID, byte[] data)
        {
            _version = versionNumber;
            _packageID = packageID;
            _data = data;
        }

        public byte[] GetPackageByte()
        {
            byte[] _buffer = new byte[_version.Length + _data.Length];
            

            return _buffer;
        }
    }
}
