using System;
using System.Collections.Generic;

namespace MAD.CLICore
{
    /* This object contains all parameters of the input. */
    public class ParInput
    {
        public List<Parameter> pars = new List<Parameter>();
        public Parameter GetPar(string par)
        {
            foreach (Parameter temp in pars)
            {
                if (temp.par == par)
                {
                    return temp;
                }
            }

            return null;
        }
    }
}
