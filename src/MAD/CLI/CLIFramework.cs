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

        public const string fwver = "2.0.6.0 stable";
        public List<CommandOptions> commands = new List<CommandOptions>();

        #endregion

        #region methods

        /*
         * This method checks the pars and args of their validity.
         * If everything is alright, the command object will be set.
         * It returns the string 'VALID_PARAMETERS' when the pars and args are valid.
         * If the par are not valid it returns the error text, which get displayed onto
         * the CLI. When the parsing was successful, the Command-Object will be set and be ready
         * for execution. */
        protected string CLIInterpreter(ref Command command, string cliInput)
        {
            string _commandInput = GetCommandName(cliInput);
            CommandOptions _commandOptions = GetCommandOptions(_commandInput);

            if (_commandOptions == null)
                return CLIError.Error(CLIError.ErrorType.CommandError, "Command '" + _commandInput + "' not known!", true);

            ParInput _parInput = GetParamtersFromInput(cliInput);
            Type _commandType = _commandOptions.commandType;

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

            command.pars = _parInput;

            return CLIFramework.ValidPar(command);
        }

        private string GetCommandName(string input)
        {
            List<string> _args = new List<string>();

            string[] _buffer = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _buffer.Length; i++)
            {
                if (_buffer[i].StartsWith("-"))
                {
                    break;
                }
                else
                {
                    _args.Add(_buffer[i]);
                }
            }

            string _temp = "";
            foreach (string _buff in _args)
                _temp += _buff + " ";

            return _temp.Trim();
        }

        private CommandOptions GetCommandOptions(string command)
        {
            foreach (CommandOptions temp in commands)
                if (temp.command == command)
                    return temp;
            return null;
        }

        private ParInput GetParamtersFromInput(string input)
        {
            ParInput _temp = new ParInput();

            string[] _buffer = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Parameter _par = null;
            List<string> _args = new List<string>();

            for (int i = 0; i < _buffer.Length; i++)
            {
                if (_buffer[i].StartsWith("-"))
                {
                    if (_par != null)
                    {
                        _par.argValues = _args.ToArray();
                        _temp.pars.Add(_par);
                        _args.Clear();
                    }
                    _par = new Parameter(_buffer[i].Remove(0,1), null);
                }
                else
                {
                    if (_par != null)
                    {
                        _args.Add(_buffer[i]);
                    }
                }
            }

            if (_par != null)
            {
                _par.argValues = _args.ToArray();
                _temp.pars.Add(_par);
            }

            return _temp;
        }

        public static string ValidPar(Command command)
        {
            List<ParOption> _rPar = command.rPar;
            List<ParOption> _oPar = command.oPar;

            Parameter _temp;
            ParOption _tempOptions;

            int _requiredArgsFound = 0;

            // Check every parameter.
            for (int i = 0; i < command.pars.pars.Count; i++)
            {
                _temp = command.pars.pars[i];
                _tempOptions = CLIFramework.GetParOptions(_rPar, _oPar, _temp.par);

                // Check if the parameter is known.
                if(_tempOptions == null)
                    return CLIError.Error(CLIError.ErrorType.ParameterError, "Parameter '" + _temp.par + "' does not exist for this command!", true);

                // Check if parameter has been used befor.
                if(_tempOptions.IsUsed)
                    return CLIError.Error(CLIError.ErrorType.ParameterError, "Parameter '" + _temp.par + "' cannot be used multiple times!", true);

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
                    if (_temp.argValues.Length != 0)
                        return CLIError.Error(CLIError.ErrorType.SyntaxError, "The parameter '" + _temp.par + "' does not need any args!", true);
                }
                else
                {
                    if (_temp.argValues.Length == 0)
                        return CLIError.Error(CLIError.ErrorType.SyntaxError, "The par '" + _temp.par + "' needs one or more args!", true);

                    if (!_tempOptions.multiargs)
                    {
                        // Check if more than one arg is given.
                        if (_temp.argValues.Length > 1)
                            return CLIError.Error(CLIError.ErrorType.SyntaxError, "The par '" + _temp.par + "' can't have multiple args!", true);
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
                        return CLIError.Error(CLIError.ErrorType.ArgumentTypeError, 
                            "The argument of the parameter '" + _temp.par + "' could not be parsed correctly!", true);

                    _temp.argValues = _args;
                }
            }

            // Check if all required pars has been found.
            if (_rPar.Count != _requiredArgsFound)
                return CLIError.Error(CLIError.ErrorType.SyntaxError, "Some required parameters are missing!", true);

            return "VALID_PARAMETERS";
        }

        /* The method 'Convert' can only parse object to: 
         *  System.Int32
         *  System.UInt32
         *  System.String
         *  System.Net.IPAddress
         *  System.Net.NetworkInformation.PhysicalAddress 
         *  System.DateTime */
        public static object Convert(string value, Type convertType)
        {
            try
            {
                switch (convertType.ToString())
                {
                    case "System.Int32":
                        return Int32.Parse(value);
                    case "System.UInt32":
                        return UInt32.Parse(value);
                    case "System.String":
                        return value;
                    case "System.Net.IPAddress":
                        return System.Net.IPAddress.Parse(value);
                    case "System.Net.NetworkInformation.PhysicalAddress":
                        if (value.Length < 12)
                            value = value.PadRight(12 - value.Length, '0');
                        else if (value.Length > 12)
                            throw new Exception("MAC-Address too long!");
                        return System.Net.NetworkInformation.PhysicalAddress.Parse(value);
                    case "System.Net.Mail.MailAddress":
                        return new System.Net.Mail.MailAddress(value);
                    case "System.DateTime":
                        return DateTime.Parse(value);
                    default:
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static ParOption GetParOptions(List<ParOption> rPar, List<ParOption> oPar, string par)
        {
            foreach (ParOption _temp in rPar)
                if (_temp.par == par)
                    return _temp;

            foreach (ParOption _temp in oPar)
                if (_temp.par == par)
                    return _temp;

            return null;
        }

        public static bool IsRequiredPar(List<ParOption> rPar, Parameter par)
        {
            foreach (ParOption _temp in rPar)
                if (_temp.par == par.par)
                    return true;

            return false;
        }

        protected string GetBanner(int consoleWidth)
        {
            string _buffer = "";

            _buffer += @"<color><cyan>" + "".PadLeft(consoleWidth, '_');
            _buffer += @"<color><cyan>  __  __  ___  _____" + "\n";
            _buffer += @"<color><cyan> |  \/  |/ _ \|  _  \ <color><yellow>PROJECT VERS. " + "  <color><white>" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + "\n";
            _buffer += @"<color><cyan> |      / /_\ \ | \ | <color><yellow>CLI-FRAMEWORK " + "  <color><white>" + fwver + "\n";
            _buffer += @"<color><cyan> | |\/| |  _  | | | | " + "\n";
            _buffer += @"<color><cyan> | |  | | | | | |_/ | <color><yellow>DATE" + "  <color><white>" + DateTime.Now.ToString("dd.MM.yyyy") + "\n";
            _buffer += @"<color><cyan> |_|  |_|_| |_|_____/ <color><yellow>TIME" + "  <color><white>" + DateTime.Now.ToString("HH:mm:ss") + "\n";
            _buffer += @"<color><cyan>" + "".PadLeft(consoleWidth, '_');

            return _buffer;
        }

        #endregion
    }
}
