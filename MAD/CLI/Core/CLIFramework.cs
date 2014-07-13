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
         * This method checks the pars and args of their validity.
         * If everything is alright, the command object will be set.
         * It returns the string 'VALID_PARAMETERS' when the pars and args are valid.
         * If the par are not valid it returns the error text, which get displayed onto
         * the CLI. When the parsing was successful, the Command-Object will be set and be ready
         * for execution. */
        protected string AnalyseInput(ref Command command, string cliInput)
        {
            // First get the command-name.
            string _commandInput = GetCommandName(cliInput);

            // Get the configuration for the command.
            CommandOptions _commandOptions = GetCommandOptions(_commandInput);

            // Check if the command exist.
            if (_commandOptions == null)
                return CLIError.Error(CLIError.ErrorType.CommandError, "Command not know!", true);

            // Get the pars and args from input.
            ParInput _parInput = GetParamtersFromInput(cliInput);

            // Get the command-type.
            Type _commandType = _commandOptions.commandType;

            // Constructor of the command.
            ConstructorInfo _cInfo;

            // Check if the command need any objects.
            if (_commandOptions.commandObjects == null)
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
                command = (Command)_cInfo.Invoke(new object[] { _commandOptions.commandObjects });
            }

            // Set pars and args of the command.
            command.pars = _parInput;

            // Check if the pars and args are valid.
            return CLIFramework.ValidPar(command, _parInput);
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

        private ParInput GetParamtersFromInput(string input)
        {
            ParInput _temp = new ParInput();
            string[] _buffer = input.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            // Remove unnecessary spaces.
            for (int i = 1; i < _buffer.Length; i++)
                _buffer[i] = _buffer[i].Trim();

            for (int i = 1; i < _buffer.Length; i++)
            {
                string[] _buffer2 = _buffer[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (_buffer2.Length == 1)
                {
                    // no args
                    _temp.pars.Add(new Parameter(_buffer2[0], null));
                }
                else
                {
                    // one or multiple args
                    if (_buffer2.Length == 2)
                    {
                        // one arg
                        _temp.pars.Add(new Parameter(_buffer2[0], new object[] { _buffer2[1] }));
                    }
                    else
                    {
                        //multiple args
                        object[] _buffer3 = new object[_buffer2.Length - 1];

                        for (int i2 = 1; i2 < _buffer2.Length; i2++)
                            _buffer3[i2 - 1] = _buffer2[i2];

                        _temp.pars.Add(new Parameter(_buffer2[0], _buffer));
                    }
                }
            }

            return _temp;
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

        public static string ValidPar(Command command, ParInput pars)
        {
            List<ParOption> _rPar = command.rPar;
            List<ParOption> _oPar = command.oPar;

            Parameter _temp;
            ParOption _tempOptions;

            int _requiredArgsFound = 0;

            // Check every parameter.
            for (int i = 0; i < command.pars.pars.Count; i++)
            {
                _temp = pars.pars[i];
                _tempOptions = CLIFramework.GetParOptions(_rPar, _oPar, _temp.par);

                // Check if the parameter is known.
                if(_tempOptions == null)
                    return CLIError.Error(CLIError.ErrorType.parError, "Parameter '" + _temp.par + "' does not exist for this command!", true);

                // Check if parameter has been used befor.
                if(_tempOptions.IsUsed)
                    return CLIError.Error(CLIError.ErrorType.parError, "Parameter '" + _temp.par + "' used multiple times!", true);

                // Mark the parameter as 'used'.
                _tempOptions.SetUsedFlag();

                // If it is a required parameter, increase _requiredArgsFound.
                // If _requiredArgsFound is equal to the lengh of the _requiredParamters,
                // all required parameter has been found in the input of the cli.
                if (IsRequiredPar(_rPar, _temp))
                    _requiredArgsFound++;

                // Check if the given parameter can have a value or not.
                if (_tempOptions.argEmpty)
                {
                    // Check if the arg is not null.
                    if (_temp.argValues != null)
                        return CLIError.Error(CLIError.ErrorType.argError, "The parameter '" + _temp.par + "' does not need any args!", true);
                }
                else
                {
                    // Check if arg is null
                    if (_temp.argValues == null)
                        return CLIError.Error(CLIError.ErrorType.argError, "The par '" + _temp.par + "' needs one or more args!", true);

                    // Check if multiple args are supported for this par
                    if (!_tempOptions.multiargs)
                    {
                        // Check if more than one arg is given.
                        if (_temp.argValues.Length > 1)
                            return CLIError.Error(CLIError.ErrorType.argError, "The par '" + _temp.par + "' can't have multiple args!", true);
                    }

                    /* Try to convert the given args into the specific types.
                     * If it cannot convert all args into ONE type, it will fail. */

                    object[] _args = new object[_temp.argValues.Length];
                    bool _allArgsConverted = false;

                    foreach (Type _type in _tempOptions.argTypes)
                    {
                        int _argsConverted = 0;

                        for (int i2 = 0; i2 < _temp.argValues.Length; i2++)
                        {
                            _args[i2] = CLIFramework.Convert((string)_temp.argValues[i2], _type);

                            if (_args[i2] != null)
                            {
                                _argsConverted++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (_argsConverted == _args.Length)
                        {
                            _allArgsConverted = true;
                            break;
                        }
                    }

                    if (!_allArgsConverted)
                    {
                        return CLIError.Error(CLIError.ErrorType.argTypeError, "The arg of the par '" + _temp.par + "' could not be parsed!", true);
                    }

                    _temp.argValues = _args;
                }
            }

            // Check if all required pars has been found.
            if (_rPar.Count != _requiredArgsFound)
                return CLIError.Error(CLIError.ErrorType.parError, "Some required parameters are missing!", true);

            return "VALID_PARAMETERS";
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

        public static ParOption GetParOptions(List<ParOption> rPar, List<ParOption> oPar, string par)
        {
            foreach (ParOption _temp in rPar)
            {
                if (_temp.par == par)
                {
                    return _temp;
                }
            }

            foreach (ParOption _temp in oPar)
            {
                if (_temp.par == par)
                {
                    return _temp;
                }
            }

            return null;
        }

        public static bool IsRequiredPar(List<ParOption> rPar, Parameter par)
        {
            foreach (ParOption _temp in rPar)
            {
                if (_temp.par == par.par)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
