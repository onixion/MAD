using System;

namespace MAD.cli
{
    public class CLIUser
    {
        public string username;
        public string passwordMD5;

        // TODO: Do we really want groups?

        public Group group;
        public enum Group { root,jobs,clis }

        public bool online = false;

        public CLIUser(string username, string passwordMD5, Group group)
        {
            this.username = username;
            this.passwordMD5 = passwordMD5;
            this.group = group;
        }
    }
}
