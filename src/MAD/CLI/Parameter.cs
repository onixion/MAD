using System;

namespace MAD.CLICore
{
    /* This object defines a par. */
    public class Parameter
    {
        public string par;
        public object[] argValues;

        public Parameter()
        { }

        public Parameter(string par, object[] argValues)
        {
            this.par = par;
            this.argValues = argValues;
        }
    }
}
