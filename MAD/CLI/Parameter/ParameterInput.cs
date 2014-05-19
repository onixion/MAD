using System;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class ParameterInput
    {
        public List<Parameter> parameters = new List<Parameter>();

        public Parameter GetParameter(string indicator)
        {
            foreach (Parameter temp in parameters)
            {
                if (temp.indicator == indicator)
                {
                    return temp;
                }
            }

            return null;
        }
    }
}
