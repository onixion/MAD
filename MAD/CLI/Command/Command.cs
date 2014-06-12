using System;
using System.Net;
using System.Collections.Generic;

namespace MAD.CLI
{
    public abstract class Command
    {
        #region members

        public List<ParameterOption> requiredParameter;
        public List<ParameterOption> optionalParameter;

        public string description;
        protected string output;

        // this object contains all parameters from cli
        // (it has been checked and converted the argument types)
        protected ParameterInput parameters;

        #endregion

        #region methodes

        protected void InitCommand()
        {
            requiredParameter = new List<ParameterOption>();
            optionalParameter = new List<ParameterOption>();

            description = "(undefined)";
            output = "";
        }

        public void SetParameters(ParameterInput parameters)
        {
            this.parameters = parameters;
        }

        public string ValidParameters(ParameterInput parameters)
        {
            int requiredArgsFound = 0;

            for (int i = 0; i < parameters.parameters.Count; i++)
            {
                Parameter _temp = parameters.parameters[i];

                // check if all arguments are known by the command
                if (!ParameterExists(_temp))
                {
                    return "<color><red>Parameter '-" + _temp.parameter + "' does not exist for this command!";
                }

                // if the given parameter is a required parameter increase requiredParamterFound
                if (RequiredParameterExist(_temp))
                {
                    requiredArgsFound++;
                }

                // TODO: MULTIPLE ARGUMENTS DO NOT WORK YET! SOMETHING HERE DO NOT WORK RIGHT!
                ParameterOption _parOptions = GetParameterOptions(_temp.parameter);

                // check if the given args can have a value or not
                if (_parOptions.argumentEmpty)
                {
                    // check if argument is not null
                    if (_temp.argumentValue != null)
                    {
                        return "<color><red>Value of parameter '-" + _temp.parameter + "' must be null!";
                    }
                }
                else
                {
                    // check if argument is null
                    if (_temp.argumentValue == null)
                    {
                        return "<color><red>Value of parameter '-" + _temp.parameter + "' can't be null!";
                    }

                    // check if multi-args are supported for this parameter
                    if (!_parOptions.multiArguments)
                    {
                        if (_temp.argumentValue.Length > 1)
                        {
                            return "<color><red>The parameter '-" + _temp.parameter + "' can't have multiple arguments!";
                        }
                    }

                    /*
                     * This CLI supports multi-type arguments.
                     * But it is importand when writing the command to check
                     * the type of the arguments, else the program throws
                     * exceptions. */

                    object[] arguments = new object[_temp.argumentValue.Length];

                    // try to convert the args into the needed types
                    foreach (Type _type in GetArgumentTypes(_temp.parameter))
                    {
                        int _temp2 = _temp.argumentValue.Length;

                        for (int i2 = 0; i2 < _temp.argumentValue.Length; i2++)
                        {
                            arguments[i2] = Convert((string)_temp.argumentValue[i2], _type);

                            if (arguments[i2] != null)
                            {
                                _temp2 = _temp2 - 1;
                            }
                        }

                        // check if all arguments could be converted to the type
                        if (_temp2 != 0)
                        {
                            return "<color><red>Could not parse some arguments from parameter '" + _temp.parameter + "'. Type help for view full commands.";
                        }
                        else
                        { 
                            // all arguments could be parsed without any problems
                            break;
                        }
                    }

                    _temp.argumentValue = arguments;
                }
            }

            // check if all required parameters are known
            if (requiredParameter.Count != requiredArgsFound)
            {
                return "<color><red>Some required parameters are missing! Type help for view full commands.";
            }

            return "VALID_PARAMETER";
        }

        public abstract string Execute();

        #region helper methodes

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
                if (temp.parameter == indicator)
                {
                    return temp;
                }
            }

            foreach (ParameterOption temp in optionalParameter)
            {
                if (temp.parameter == indicator)
                {
                    return temp;
                }
            }

            return null;
        }

        public bool ParameterExists(Parameter parameter)
        {
            // required parameter
            foreach (ParameterOption temp in requiredParameter)
            {
                if (temp.parameter == parameter.parameter)
                {
                    return true;
                }
            }

            // optional parameter
            foreach (ParameterOption temp in optionalParameter)
            {
                if (temp.parameter == parameter.parameter)
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
                if (temp.parameter == parameter.parameter)
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
                if (temp.parameter == parameter.parameter)
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
                if ((string)temp.parameter == indicator)
                {
                    return true;
                }
            }

            return false;
        }

        public Type[] GetArgumentTypes(string indicator)
        {
            foreach (ParameterOption temp in requiredParameter)
            {
                if (temp.parameter == indicator)
                {
                    return temp.argumentTypes;
                }
            }

            foreach (ParameterOption temp in optionalParameter)
            {
                if (temp.parameter == indicator)
                {
                    return temp.argumentTypes;
                }
            }

            return null;
        }

        #endregion

        #endregion
    }
}
