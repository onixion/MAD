using System;
using System.Collections.Generic;

namespace MAD.CLICore
{
    /* CChecker - CommandChecker
     *  This static class is used to provide the 'ValidParameters'-method to the CLIFramework.
     *  The method 'ValidParameters' is used to check the validity of the given parameters.
     */

    public static class CChecker
    {
        public static string ValidParameters(Command command, ParameterInput parameters)
        {
            List<ParameterOption> _requiredParameter = command.requiredParameter;
            List<ParameterOption> _optionalParameter = command.optionalParameter;

            int _requiredArgsFound = 0;

            for (int i = 0; i < command.parameters.parameters.Count; i++)
            {
                Parameter _temp = parameters.parameters[i];
                ParameterOption _parOptions = CChecker.GetParameterOptions(_requiredParameter, _optionalParameter, _temp.parameter);

                // Check if the parameter is known.
                if (!ParameterExists(_requiredParameter, _optionalParameter, _temp))
                {
                    return "<color><red>Parameter '-" + _temp.parameter + "' does not exist for this command!";
                }

                // If it is a required parameter, increase _requiredArgsFound.
                // If _requiredArgsFound is equal to the lengh of the _requiredParamters,
                // all required parameter are given.
                if (RequiredParameterExist(_requiredParameter, _temp))
                {
                    _requiredArgsFound++;
                }

                // Check if the given parameter can have a value or not.
                if (_parOptions.argumentEmpty)
                {
                    // Check if the argument is not null.
                    if (_temp.argumentValues != null)
                    {
                        return "<color><red>Value of parameter '-" + _temp.parameter + "' must be null!";
                    }
                }
                else
                {
                    // Check if argument is null
                    if (_temp.argumentValues == null)
                    {
                        return "<color><red>Value of parameter '-" + _temp.parameter + "' can't be null!";
                    }

                    // Check if multiple args are supported for this parameter
                    if (!_parOptions.multiArguments)
                    {
                        // Check if more than one arg is given.
                        if (_temp.argumentValues.Length > 1)
                        {
                            return "<color><red>The parameter '-" + _temp.parameter + "' can't have multiple arguments!";
                        }
                    }

                    /* Try to convert the given args into the specific types.
                     * If it cannot convert all args into ONE type, it will fail. */

                    object[] _arguments = new object[_temp.argumentValues.Length];
                    bool _allArgsConverted = false;

                    foreach (Type _type in CChecker.GetAcceptedArgumentTypes(_requiredParameter, _optionalParameter, _temp.parameter))
                    {
                        int _argsConverted = 0;

                        for (int i2 = 0; i2 < _temp.argumentValues.Length; i2++)
                        {
                            _arguments[i2] = CChecker.Convert((string)_temp.argumentValues[i2], _type);

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
                            _allArgsConverted = true;
                            break;
                        }
                    }

                    if (!_allArgsConverted)
                    {
                        return "<color><red>Some of the arguments of the parameter '-" + _temp.parameter + "' could not be parsed!";
                    }

                    _temp.argumentValues = _arguments;
                }
            }

            // Check if all required parameters has been found.
            if (_requiredParameter.Count != _requiredArgsFound)
            {
                return "<color><red>Some required parameters are missing! Type help for view full commands.";
            }

            return "VALID_PARAMETER";
        }

        /* The method 'Convert' can only parse object to: 
         *  System.Int32
         *  System.String
         *  System.Net.IPAddress
         *  System.Net.NetworkInformation.PhysicalAddress */
        public static object Convert(string value, Type convertType)
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
                    case "System.Net.NetworkInformation.PhysicalAddress":
                        return System.Net.NetworkInformation.PhysicalAddress.Parse(value);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ParameterOption GetParameterOptions(List<ParameterOption> requiredParameter, List<ParameterOption> optionalParameter, string parameter)
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

        public static Type[] GetAcceptedArgumentTypes(List<ParameterOption> requiredParameter, List<ParameterOption> optionalParameter, string parameter)
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

        public static Type GetArgumentType(ParameterInput parameters, string parameter)
        {
            foreach (Parameter _temp in parameters.parameters)
            {
                if (_temp.parameter == parameter)
                {
                    return _temp.argumentValues[0].GetType();
                }
            }

            return null;
        }

        public static bool ParameterExists(List<ParameterOption> requiredParameter, List<ParameterOption> optionalParameter, Parameter parameter)
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

        public static bool RequiredParameterExist(List<ParameterOption> requiredParameter, Parameter parameter)
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

        public static bool OptionalParameterExist(List<ParameterOption> optionalParameter, Parameter parameter)
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
    }
}
