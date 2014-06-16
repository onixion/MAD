using System;
using System.Collections.Generic;

namespace MAD.CLI
{
    public abstract class Command
    {
        #region members

        private List<ParameterOption> _requiredParameter;
        public List<ParameterOption> requiredParameter { get { return _requiredParameter; } }
       
        private List<ParameterOption> _optionalParameter;
        public List<ParameterOption> optionalParameter { get { return _optionalParameter; } }

        public string description { get; set; }
        protected string output = "";

        // This object contains all parameters given by the cli.
        protected ParameterInput parameters;

        #endregion

        #region constructor

        protected Command()
        {
            _requiredParameter = new List<ParameterOption>();
            _optionalParameter = new List<ParameterOption>();
        }

        #endregion

        #region methodes

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

                    /* Try to convert the given args into the specific types.
                     * If it cannot convert all args into ONE type, it will fail. */

                    object[] _arguments = new object[_temp.argumentValue.Length];
                    int _argsConverted = 0;
                    bool _validConvert = false;

                    foreach (Type _type in GetAcceptedArgumentTypes(_temp.parameter))
                    {
                        for (int i2 = 0; i2 < _temp.argumentValue.Length; i2++)
                        {
                            _arguments[i2] = Convert((string)_temp.argumentValue[i2], _type);

                            if (_arguments[i2] != null)
                            {
                                _argsConverted++;
                            }
                        }

                        if (_argsConverted == _arguments.Length)
                        {
                            // All args could be converted into one type.
                            _validConvert = true;
                            break;
                        }
                    }

                    if (_validConvert == false)
                    {
                        return "<color><red>Could not parse some arguments from parameter '" + _temp.parameter + "'. Type help for view full commands.";
                    }

                    _temp.argumentValue = _arguments;
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
                        return System.Net.IPAddress.Parse(value);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ParameterOption GetParameterOptions(string parameter)
        {
            foreach (ParameterOption _temp in requiredParameter)
            {
                if (_temp.parameter == parameter)
                {
                    return _temp;
                }
            }

            foreach (ParameterOption _temp in optionalParameter)
            {
                if (_temp.parameter == parameter)
                {
                    return _temp;
                }
            }

            return null;
        }

        private bool ParameterExists(Parameter parameter)
        {
            // required parameter
            foreach (ParameterOption _temp in requiredParameter)
            {
                if (_temp.parameter == parameter.parameter)
                {
                    return true;
                }
            }

            // optional parameter
            foreach (ParameterOption _temp in optionalParameter)
            {
                if (_temp.parameter == parameter.parameter)
                {
                    return true;
                }
            }

            return false;
        }

        private bool RequiredParameterExist(Parameter parameter)
        {
            foreach (ParameterOption _temp in requiredParameter)
            {
                if (_temp.parameter == parameter.parameter)
                {
                    return true;
                }
            }

            return false;
        }

        private bool OptionalParameterExist(Parameter parameter)
        {
            foreach (ParameterOption _temp in optionalParameter)
            {
                if (_temp.parameter == parameter.parameter)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool OptionalParameterUsed(string parameter)
        {
            foreach (Parameter _temp in parameters.parameters)
            {
                if ((string)_temp.parameter == parameter)
                {
                    return true;
                }
            }

            return false;
        }

        private Type[] GetAcceptedArgumentTypes(string parameter)
        {
            foreach (ParameterOption _temp in requiredParameter)
            {
                if (_temp.parameter == parameter)
                {
                    return _temp.argumentTypes;
                }
            }

            foreach (ParameterOption _temp in optionalParameter)
            {
                if (_temp.parameter == parameter)
                {
                    return _temp.argumentTypes;
                }
            }

            return null;
        }

        protected Type GetArgumentType(string parameter)
        {
            foreach (Parameter _temp in parameters.parameters)
            {
                if (_temp.parameter == parameter)
                {
                    return _temp.argumentValue[0].GetType();
                }
            }

            return null;
        }

        #endregion
    }
}
