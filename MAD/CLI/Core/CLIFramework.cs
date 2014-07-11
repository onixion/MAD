using System;
using System.Reflection;
using System.Collections.Generic;

using MAD.CLIServerCore;
using MAD.JobSystemCore;

namespace MAD.CLICore
{
    public abstract class CLIFramework
    {
        #region member

        public List<CommandOptions> commands = new List<CommandOptions>();
        public enum CommandGroup { Gereral, JobSystem, CLIServer}

        #endregion

        #region methodes

        /*
         * This method checks the parameters and arguments of their validity.
         * If everything is alright, the command object will be set.
         * It returns the string "VALID_PARAMETER" when the parameters and arguments are valid.
         * If the parameter are not valid it returns the error text, which get displayed onto
         * the CLI. When the parsing was successful, the Command-Object will be set and be ready
         * for execution. */
        protected string AnalyseInput(ref Command command, string cliInput)
        {
            // First get the command-name.
            string _commandInput = GetCommandName(cliInput);

            // Get the configuration of the command.
            CommandOptions _commandOptions = GetCommandOptions(_commandInput);

            // Check if the command exist.
            if (_commandOptions == null)
            {
                return "<color><red>Command '" + _commandInput + "' unknown! Type 'help' for more information.";
            }

            // Get the parameters and arguments from input.
            ParameterInput _parameterInput = GetParamtersFromInput(cliInput);
            // Get the command-type.
            Type _commandType = _commandOptions.commandType;
            // Get the needed objects for the constructor of the command.
            object[] _commandObjects = _commandOptions.commandObjects;

            // Constructor of the command.
            ConstructorInfo _cInfo;

            // Check if the command need any objects.
            if (_commandObjects == null)
            {
                // Command does not need any objects for its constructor.
                _cInfo = _commandType.GetConstructor(new Type[0]);
                // Invoke the constructor.
                command = (Command)_cInfo.Invoke(null);
            }
            else
            {
                // Command need some objects for its constructor.
                _cInfo = _commandType.GetConstructor(new Type[1] { typeof(object[]) });
                // Invoke the constructor.
                command = (Command)_cInfo.Invoke(new object[] { _commandObjects });
            }

            // Set parameters and arguments of the command.
            command.parameters = _parameterInput;

            // Check if the parameters and arguments are valid.
            string _parameterValid = CLIFramework.ValidParameters(command, _parameterInput);

            return _parameterValid;
        }

        private string GetCommandName(string input)
        {
            string[] _buffer = input.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            if (_buffer.Length != 0)
            {
                if (_buffer[0] == "")
                {
                    return null;
                }

                _buffer[0] = _buffer[0].Trim();

                return _buffer[0];
            }
            else
            {
                return null;
            }
        }

        private ParameterInput GetParamtersFromInput(string input)
        {
            ParameterInput _parameterTemp = new ParameterInput();

            string[] _temp = input.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < _temp.Length; i++)
            {
                _temp[i] = _temp[i].Trim();
            }

            for (int i = 1; i < _temp.Length; i++)
            {
                string[] _temp2 = _temp[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (_temp2.Length == 1)
                {
                    // parameter arguments are null
                    _parameterTemp.parameters.Add(new Parameter(_temp2[0], null));
                }
                else
                {
                    // one or multiple arguments
                    if (_temp2.Length == 2)
                    {
                        // one argument
                        _parameterTemp.parameters.Add(new Parameter(_temp2[0], new object[] { _temp2[1] }));
                    }
                    else
                    { 
                        //multiple arguments
                        object[] _buffer = new object[_temp2.Length-1];
                        
                        for(int i2 = 1; i2 < _temp2.Length; i2++)
                        {
                            _buffer[i2-1] = _temp2[i2];
                        }

                        _parameterTemp.parameters.Add(new Parameter(_temp2[0], _buffer));
                    }
                }
            }

            return _parameterTemp;
        }

        private CommandOptions GetCommandOptions(string command)
        {
            foreach (CommandOptions temp in commands)
            {
                if (temp.command == command)
                {
                    return temp;
                }
            }

            return null;
        }

        public static string ValidParameters(Command command, ParameterInput parameters)
        {
            List<ParameterOption> _requiredParameter = command.requiredParameter;
            List<ParameterOption> _optionalParameter = command.optionalParameter;

            int _requiredArgsFound = 0;

            for (int i = 0; i < command.parameters.parameters.Count; i++)
            {
                Parameter _temp = parameters.parameters[i];
                ParameterOption _parOptions = CLIFramework.GetParameterOptions(_requiredParameter, _optionalParameter, _temp.parameter);

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

                    foreach (Type _type in CLIFramework.GetAcceptedArgumentTypes(_requiredParameter, _optionalParameter, _temp.parameter))
                    {
                        int _argsConverted = 0;

                        for (int i2 = 0; i2 < _temp.argumentValues.Length; i2++)
                        {
                            _arguments[i2] = CLIFramework.Convert((string)_temp.argumentValues[i2], _type);

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

        #endregion
    }
}
