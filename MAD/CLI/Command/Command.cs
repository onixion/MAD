using System;
using System.Net;
using System.Collections.Generic;

namespace MAD.CLI
{
    public abstract class Command
    {
        #region members

        public string description;

        public List<ParameterOption> requiredParameter = new List<ParameterOption>();
        public List<ParameterOption> optionalParameter = new List<ParameterOption>();

        protected ParameterInput parameters;
        protected string output = "";

        #endregion

        #region methodes

        public void SetParameters(ParameterInput parameters)
        {
            this.parameters = parameters;
        }

        public bool ParameterExists(Parameter parameter)
        {
            // required parameter
            foreach (ParameterOption temp in requiredParameter)
            {
                if (temp.indicator == parameter.indicator)
                {
                    return true;
                }
            }

            // optional parameter
            foreach (ParameterOption temp in optionalParameter)
            {
                if (temp.indicator == parameter.indicator)
                {
                    return true;
                }
            }

            return false;
        }

        public bool RequiredParameterExist(Parameter parameter)
        {
            foreach (ParameterOption temp in requiredParameter)
            {
                if (temp.indicator == parameter.indicator)
                {
                    return true;
                }
            }

            return false;
        }

        public bool OptionalParameterExist(Parameter parameter)
        {
            foreach (ParameterOption temp in optionalParameter)
            {
                if (temp.indicator == parameter.indicator)
                {
                    return true;
                }
            }

            return false;
        }

        public bool OptionalParameterUsed(string indicator)
        {
            foreach (Parameter temp in parameters.parameters)
            {
                if ((string)temp.indicator == indicator)
                {
                    return true;
                }
            }

            return false;
        }

        public Type GetArgumentType(string indicator)
        {
            foreach (ParameterOption temp in requiredParameter)
            {
                if (temp.indicator == indicator)
                {
                    return temp.argumentType;
                }
            }

            foreach (ParameterOption temp in optionalParameter)
            {
                if (temp.indicator == indicator)
                {
                    return temp.argumentType;
                }
            }

            return null;
        }

        public string ValidParameters(ParameterInput parameters)
        {
            int requiredArgsFound = 0;

            for (int i = 0; i < parameters.parameters.Count; i++)
            {
                Parameter temp = parameters.parameters[i];

                // check if all arguments are known by the command
                if (!ParameterExists(temp))
                {
                    return "<color><red>Parameter '-" + temp.indicator + "' does not exist for this command!";
                }

                // if the given arg is a required arg increase requiredArgsFound
                if (RequiredParameterExist(temp))
                {
                    requiredArgsFound++;
                }

                object argument;

                // check if the given args can have a value or not
                if (GetParameterOptions(temp.indicator).argumentEmpty)
                {
                    // check if argument is not null
                    if(temp.value != null)
                    {
                        return "<color><red>Value of parameter '-" + temp.indicator + "' must be null!";
                    }

                    argument = new object();
                }
                else
                { 
                    // check if argument is null
                    if (temp.value == null)
                    {
                        return "<color><red>Value of parameter '-" + temp.indicator + "' can't be null!";
                    }

                    // try to parse the argument to the specific type
                    argument = Convert((string)temp.value, GetArgumentType(temp.indicator));
                }

                if (argument == null)
                {
                    return "<color><red>Could not parse argument '" + temp.value + "'. Type help for view full commands.";
                }
                else
                {
                    // set value to the parsed object
                    temp.value = argument;
                }
            }

            // check if all required parameters are known
            if (requiredParameter.Count != requiredArgsFound)
            {
                return "<color><red>Some required parameters are missing! Type 'help' to see full commands.";
            }

            return "VALID_PARAMETER";
        }

        /* The method 'Convert' can only parse object to: 
         * 
         *  System.Int32
         *  System.String
         *  System.Net.IPAddress */
        private object Convert(string value, Type convertType)
        {
            try
            {
                switch (convertType.ToString())
                {
                    case "System.Int32":
                        return Int32.Parse(value);
                    case "System.String":
                        return value;
                    case "System.Net.IPAddress":
                        return IPAddress.Parse(value);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ParameterOption GetParameterOptions(string indicator)
        {
            foreach (ParameterOption temp in requiredParameter)
            {
                if (temp.indicator == indicator)
                {
                    return temp;
                }
            }

            foreach (ParameterOption temp in optionalParameter)
            {
                if (temp.indicator == indicator)
                {
                    return temp;
                }
            }

            return null;
        }

        public abstract string Execute();

        #endregion
    }
}
