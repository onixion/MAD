using System;
using System.Collections.Generic;

namespace MAD.CLICore
{
    /* This object contains all parameter of the input. */
    public class ParameterInput
    {
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
