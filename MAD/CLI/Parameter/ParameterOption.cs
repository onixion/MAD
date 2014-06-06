using System;

namespace MAD.CLI
{
    public class ParameterOption
    {
        public string indicator;
        public string description;

        // arguemntEmpty = true -> No argument needed. argumentEmpty = false -> Argument required.
        public bool argumentEmpty;

        // if argumentEmpty = true -> argumentType = null
        public Type argumentType;

        public ParameterOption(string indicator,bool argumentEmpty, Type argumentType)
        {
            this.indicator = indicator;

            ParameterDescriptionAuto(indicator);

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

        public ParameterOption(string indicator, string description, bool argumentEmpty, Type argumentType)
        {
            this.indicator = indicator;
            this.description = description;
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
