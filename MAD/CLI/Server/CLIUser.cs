using System;

namespace MAD
{
    public class CLIUser
    {
        public string username;
        public string passwordMD5;

        public bool online = false;

        public CLIUser(string username, string passwordMD5)
        {
            this.username = username;
            this.passwordMD5 = passwordMD5;
        }
    }
}
