using System;

namespace MAD.CLI
{
    public class ParameterOption
    {
        public string parameter;
        public string parameterInfo;
        public string description;
        public bool argumentEmpty;
        public bool multiArguments;
        public Type[] argumentTypes;

        public ParameterOption(string parameter, string parameterInfo, string description, bool argumentEmpty, bool multiArguments, Type[] argumentTypes)
        {
            this.parameter = parameter;
            this.parameterInfo = parameterInfo;
            this.description = description;
            this.argumentEmpty = argumentEmpty;
            this.multiArguments = multiArguments;

            if (argumentEmpty != true)
            {
                this.argumentTypes = argumentTypes;
            }
            else
            {
                this.argumentTypes = null;
            }
        }
    }
}
