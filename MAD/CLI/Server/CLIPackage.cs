using System;
using System.Security.Cryptography;


namespace MAD.CLI.Server
{
    public class CLIPackage
    {
        public string version;
        public int packageID;
        public string data;

        public CLIPackage()
        {
            version = "";
            packageID = 0;
            data = "";
        }

        public CLIPackage(string version, int packageID, string data)
        {
            this.version = version;
            this.packageID = packageID;
            this.data = data;
        }
    }
}
