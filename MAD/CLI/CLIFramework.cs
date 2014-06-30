using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD.cli
{
    public abstract class CLIFramework
    {
        #region member

        protected List<CommandOptions> commands = new List<CommandOptions>();

        #endregion

        #region methodes

        /*
         * This method checks the parameters and arguments of their validity.
         * If everything is alright, the command object will be set.
         * 
         * It returns the string "VALID_PARAMETER" when the parameters and arguments are valid.
         * If the parameter are not valid it returns the error text, which get displayed onto
         * the CLI. When the parsing was successful, the Command-Object will be set and be ready
         * for execution. */
        protected string AnalyseInput(string cliInput, ref Command command)
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

            // Check if the parameters and arguments are valid.
            string _parameterValid = command.ValidParameters(_parameterInput);

            /* If the parameters and arguments are valid then '_parameterValid' is
             * equal to 'VALID_PARAMETERS'. */
            if (_parameterValid == "VALID_PARAMETER")
            {
                // Set parameters and arguments of the command.
                command.SetParameters(_parameterInput);
            }

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

        #endregion
    }
}
