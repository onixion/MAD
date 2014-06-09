using System;

namespace MAD.CLI
{
    public class ParameterOption
    {
        public string parameter;
        public string description;
        public bool argumentEmpty;
        public bool multiArguments;
        public Type[] argumentTypes;

        public ParameterOption(string parameter,bool argumentEmpty, Type[] argumentTypes)
        {
            this.parameter = parameter;

            ParameterDescriptionAuto(parameter);

            this.argumentEmpty = argumentEmpty;

            if (argumentEmpty != true)
            {
                this.argumentTypes = argumentTypes;
            }
            else
            {
                this.argumentTypes = null;
            }
        }

        public ParameterOption(string parameter, string description, bool argumentEmpty, bool multiArguments, Type[] argumentTypes)
        {
            this.parameter = parameter;
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

        private void ParameterDescriptionAuto(string indicator)
        {
            switch(indicator)
            {
                case "n":
                    description = "Name";
                    break;
                case "ip":
                    description = "Target-IPAddress";
                    break;
                case "t":
                    description = "Job Delaytime";
                    break;
                case "p":
                    description = "Target-Port";
                    break;
                case "ttl":
                    description = "TTL";
                    break;
                default:
                    description = "NO DESCRIPTION YET!";
                    break;
            }
        }
    }
}
