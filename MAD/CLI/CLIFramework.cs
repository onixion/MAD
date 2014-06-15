using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MAD.CLI
{
    public abstract class CLIFramework
    {
        #region member

        // cli framework version
        public Version versionFramework = new Version(1,6);

        // cli vars
        protected string cursor = "=> ";

        // all commands useable for this cli
        protected List<CommandOptions> commands = new List<CommandOptions>();

        protected Command command;
        protected Type inputCommandType;

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
            string commandInput = commandInput = GetCommandName(cliInput);

            // check if command is known by cli
            if (CommandExists(commandInput))
            {
                // get command options
                CommandOptions commandOptions = GetCommandOptions(commandInput);
                // get parameter and arguments from input
                ParameterInput parameterInput = GetParamtersFromInput(cliInput);
                // get command type
                Type commandType = commandOptions.commandType;

                // get objects to pass to constructor of the command
                object[] commandObjects = commandOptions.commandObjects;
                // get constructor and pass object[] to it (if cInfo == null)
                ConstructorInfo cInfo = commandType.GetConstructor(new Type[1] { typeof(object[]) });

                if (cInfo == null)
                {
                    // try with empty constructor
                    cInfo = commandType.GetConstructor(new Type[0]);
                    command = (Command)cInfo.Invoke(null);
                }
                else
                {
                    command = (Command)cInfo.Invoke(new object[]{commandObjects});
                }

                // check if the arguments are valid (parameter valid = "VALID_PARAMETER")
                string parameterValid = command.ValidParameters(parameterInput);

                // set parameters if the parameter are valid
                if (parameterValid == "VALID_PARAMETER")
                {
                    command.SetParameters(parameterInput);
                }

                return parameterValid;
            }
            else
            {
                return "<color><red>Command '" + commandInput + "' unknown! Type 'help' for more information.";
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
