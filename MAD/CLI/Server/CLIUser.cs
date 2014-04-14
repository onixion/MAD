using System;
using System.Text;

namespace MAD
{
    public class CLIUser
    {
        public string username;
        private byte[] password;

        public CLIUser(string username, string password)
        {
            this.username = username;
            this.password = GetMD5Hash(password);
        }

        /// <summary>
        /// Check if given password equals the password from the user.
        /// </summary>
        public bool CheckPassword(string pass)
        {
            byte[] temp = GetMD5Hash(pass);

            if (password == temp)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Calculate MD5 hash.
        /// </summary>
        private byte[] GetMD5Hash(string text)
        {
            System.Security.Cryptography.MD5 alg = new System.Security.Cryptography.MD5CryptoServiceProvider();
            return alg.ComputeHash(Encoding.ASCII.GetBytes(text));
        }
    }
}
