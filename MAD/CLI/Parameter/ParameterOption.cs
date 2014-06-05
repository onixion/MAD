using System;

namespace MAD.CLI
{
    public class ParameterOption
    {
        public string indicator;

        // arguemntEmpty = true -> No argument needed. argumentEmpty = false -> Argument required.
        public bool argumentEmpty;

        // if argumentEmpty = true -> argumentType = null
        public Type argumentType;

        public ParameterOption(string indicator, bool argumentEmpty, Type argumentType)
        {
            this.indicator = indicator;
            this.argumentEmpty = argumentEmpty;

            if (argumentEmpty != true)
            {
                this.argumentType = argumentType;
            }
            else
            {
                this.argumentType = null;
            }
        }
    }
}
