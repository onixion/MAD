﻿using System;

namespace MAD.cli
{
    /* This object defines a parameter. */
    public class Parameter
    {
        public string parameter;
        public object[] argumentValue;

        public Parameter(string parameter, object[] argumentValue)
        {
            this.parameter = parameter;
            this.argumentValue = argumentValue;
        }
    }
}
