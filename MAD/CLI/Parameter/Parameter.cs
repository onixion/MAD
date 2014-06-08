using System;

namespace MAD.CLI
{
    public class Parameter
    {
        /*
         * EXAMPE:
         * 
         * string parameter = "n";
         * object[] argumentValue = new object[]{ "Name1", "Name2" };
         * 
         * With this configuration we have a parameter 'n' which can have multi-arguments.
         */

        public string parameter;
        public object[] argumentValue;

        public Parameter(string parameter, object[] argumentValue)
        {
            this.parameter = parameter;
            this.argumentValue = argumentValue;
        }
    }
}
