using System;
using System.Collections.Generic;

namespace MAD
{
    public class ParameterInput
    {
        public List<Parameter> parameters;

        public ParameterInput()
        {
            parameters = new List<Parameter>();
        }

        /// <summary>
        /// Get the parameter for a specific indicator (if parameter do not exist: return null)
        /// </summary>
        public Parameter GetParameter(string indicator)
        {
            foreach (Parameter temp in parameters)
                if (temp.indicator == indicator)
                    return temp;

            return null;
        }
    }
}
