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
            int _requiredArgsFound = 0;

            for (int i = 0; i < parameters.parameters.Count; i++)
            {
                Parameter _temp = parameters.parameters[i];
                ParameterOption _parOptions = GetParameterOptions(_temp.parameter);

                // Check if the parameter is known.
                if (!ParameterExists(_temp))
                {
                    return "<color><red>Parameter '-" + _temp.parameter + "' does not exist for this command!";
                }

                // If it is a required parameter, increase _requiredArgsFound.
                // If _requiredArgsFound is equal to the lengh of the _requiredParamters,
                // all required parameter are given.
                if (RequiredParameterExist(_temp))
                {
                    _requiredArgsFound++;
                }

                // Check if the given parameter can have a value or not.
                if (_parOptions.argumentEmpty)
                {
                    // Check if the argument is not null.
                    if (_temp.argumentValue != null)
                    {
                        return "<color><red>Value of parameter '-" + _temp.parameter + "' must be null!";
                    }
                }
                else
                {
                    // Check if argument is null
                    if (_temp.argumentValue == null)
                    {
                        return "<color><red>Value of parameter '-" + _temp.parameter + "' can't be null!";
                    }

                    // Check if multiple args are supported for this parameter
                    if (!_parOptions.multiArguments)
                    {
                        // Check if more than one arg is given.
                        if (_temp.argumentValue.Length > 1)
                        {
                            return "<color><red>The parameter '-" + _temp.parameter + "' can't have multiple arguments!";
                        }
                    }

                    /* Try to convert the given args into the specific types.
                     * If it cannot convert all args into ONE type, it will fail. */

                    object[] _arguments = new object[_temp.argumentValue.Length];
                    bool _convertSuccess = false;

                    foreach (Type _type in GetAcceptedArgumentTypes(_temp.parameter))
                    {
                        int _argsConverted = 0;

                        for (int i2 = 0; i2 < _temp.argumentValue.Length; i2++)
                        {
                            _arguments[i2] = Convert((string)_temp.argumentValue[i2], _type);

                            if (_arguments[i2] != null)
                            {
                                _argsConverted++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (_argsConverted == _arguments.Length)
                        {
                            _convertSuccess = true;
                        }
                    }

                    if (!_convertSuccess)
                    {
                        return "<color><red>Some of the arguments of the parameter '-" + _temp.parameter + "' could not be parsed!";
                    }

                    _temp.argumentValue = _arguments;
                }
            }

            // Check if all required parameters has been found.
            if (requiredParameter.Count != _requiredArgsFound)
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
