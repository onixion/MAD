using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MAD.cli
{
    public abstract class CLIFramework
    {
        #region member

        private Version _version = new Version(1,7);
        public string versionFramework { get { return _version.ToString(); } }

        protected List<CommandOptions> commands = new List<CommandOptions>();

        #endregion

        #region CLI main methodes

        /*
         * This function checks if the parms and args are valid and sets the command
         * object.
         * 
         * It returns the string "VALID_PARAMETER" when the parameters and arguments are valid.
         * If the parameter are not valid it returns the error text. */
        public string AnalyseInput(string cliInput, ref Command command)
        {
            // get command
            string _commandInput = GetCommandName(cliInput);

            // check if command is known by cli
            if (CommandExists(_commandInput))
            {
                // get command options
                CommandOptions _commandOptions = GetCommandOptions(_commandInput);
                // get parameter and arguments from input
                ParameterInput _parameterInput = GetParamtersFromInput(cliInput);
                // get command type
                Type _commandType = _commandOptions.commandType;

                // get objects to pass to constructor of the command
                object[] _commandObjects = _commandOptions.commandObjects;
                // get constructor and pass object[] to it
                ConstructorInfo _cInfo = _commandType.GetConstructor(new Type[1] { typeof(object[]) });

                // if _cInfo == null, constuctor is wrong
                if (_cInfo != null)
                {
                    command = (Command)_cInfo.Invoke(new object[]{_commandObjects});
                }
                else
                {
                    // try with empty constructor
                    _cInfo = _commandType.GetConstructor(new Type[0]);
                    command = (Command)_cInfo.Invoke(null);
                }

                // check if the arguments are valid (parameter valid = "VALID_PARAMETER")
                string _parameterValid = command.ValidParameters(_parameterInput);

                // set parameters if the parameter are valid
                if (_parameterValid == "VALID_PARAMETER")
                {
                    command.SetParameters(_parameterInput);
                }

                return _parameterValid;
            }
            else
            {
                return "<color><red>Command '" + _commandInput + "' unknown! Type 'help' for more information.";
            }
        }

        #endregion

        #region CLI format methodes

        protected string GetCommandName(string input)
        {
            string[] buffer = input.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);

            if (buffer.Length != 0)
            {
                if (buffer[0] == "")
                    return null;

                buffer[0] = buffer[0].Trim();

                return buffer[0];
            }
            else
                return null;
        }

        protected ParameterInput GetParamtersFromInput(string input)
        {
            ParameterInput parameterTemp = new ParameterInput();

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
                    parameterTemp.parameters.Add(new Parameter(_temp2[0], null));
                }
                else
                {
                    // one or multiple arguments
                    if (_temp2.Length == 2)
                    {
                        // one argument
                        parameterTemp.parameters.Add(new Parameter(_temp2[0], new object[] { _temp2[1] }));
                    }
                    else
                    { 
                        //multiple arguments
                        object[] _buffer = new object[_temp2.Length-1];
                        
                        for(int i2 = 1; i2 < _temp2.Length; i2++)
                        {
                            _buffer[i2-1] = _temp2[i2];
                        }

                        parameterTemp.parameters.Add(new Parameter(_temp2[0], _buffer));
                    }
                }
            }

            return parameterTemp;
        }

        #endregion

        #region CLI internal methodes

        protected bool CommandExists(string command)
        {
            foreach (CommandOptions temp in commands)
            {
                if (temp.command == command)
                {
                    return true;
                }
            }

            return false;
        }

        protected CommandOptions GetCommandOptions(string command)
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

        #region CLI TimeStamp

        protected string GetTimeStamp()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        #endregion
    }
}
