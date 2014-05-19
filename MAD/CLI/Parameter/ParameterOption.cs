using System;

namespace MAD.CLI
{
    public class ParameterOption
    {
        public string indicator;
        public bool argumentEmpty;
        public Type argumentType;

        public ParameterOption(string indicator, bool argumentEmpty, Type argumentType)
        {
            this.indicator = indicator;
            this.argumentEmpty = argumentEmpty;
            this.argumentType = argumentType;
        }
    }
}
