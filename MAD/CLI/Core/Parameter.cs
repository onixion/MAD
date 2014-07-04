using System;

namespace MAD.CLICore
{
    /* This object defines a parameter. */
    public class Parameter
    {
        public string parameter;
        public object[] argumentValues;

        public Parameter(string parameter, object[] argumentValues)
        {
            this.parameter = parameter;
            this.argumentValues = argumentValues;
        }
    }
}
