using System;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class ParameterInput
    {
        // All parameter from input.
        public List<Parameter> parameters = new List<Parameter>();

        public Parameter GetParameter(string parameter)
        {
            foreach (Parameter temp in parameters)
            {
                if (temp.parameter == parameter)
                {
                    return temp;
                }
            }

            return null;
        }
    }
}
