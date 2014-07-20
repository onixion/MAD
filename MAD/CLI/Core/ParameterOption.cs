using System;

namespace MAD.CLICore
{
    /* This object defines one parameter for a command. */

    public class ParOption
    {
        public string par;
        public string parInfo;
        public string description;
        public bool argEmpty;
        public bool multiargs;
        public Type[] argTypes;

        private bool _used = false;
        public bool IsUsed { get { return _used; } }

        public ParOption(string par, string parInfo, string description, bool argEmpty, bool multiargs, Type[] argTypes)
        {
            this.par = par;
            this.parInfo = parInfo;
            this.description = description;
            this.argEmpty = argEmpty;
            this.multiargs = multiargs;
            this.argTypes = argTypes;
        }

        public void SetUsedFlag()
        {
            _used = true;
        }
    }
}
