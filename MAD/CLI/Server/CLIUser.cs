using System;
using System.Text;

namespace MAD
{
    public class CLIUser
    {
        public string username;
        public string passwordMD5;

        public CLIUser(string username, string passwordMD5)
        {
            this.username = username;
            this.passwordMD5 = passwordMD5;
        }
    }
}
