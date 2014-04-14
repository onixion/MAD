using System;

namespace MAD
{
    public class Parameter
    {
        public string indicator;
        public object value;

        public Parameter(string indicator, string value)
        {
            this.indicator = indicator;
            this.value = value;
        }
    }
}
