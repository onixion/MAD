using System;

namespace MAD.CLI
{
    public class Parameter
    {
        public string indicator;
        public object value;

        /*
         * Example:
         * 
         *  string indicator = "id"
         *  string value = "0"
         *  
         * Parameter "id", Argument is "0".
         */

        public Parameter(string indicator, string value)
        {
            this.indicator = indicator;
            this.value = value;
        }
    }
}
