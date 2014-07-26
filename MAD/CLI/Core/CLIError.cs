using System;

namespace MAD.CLICore
{
    static class CLIError
    {
        public const string _colorTag = "red";
        public enum ErrorType
        {
            SyntaxError,
            CommandError,
            parError,
            argError,
            argTypeError,
            inputError,
            outputError
        }

        public static string Error(ErrorType type, string errorText, bool colorEnable)
        {
            string _buffer = "";

            if(colorEnable)
                _buffer = "<color><" + _colorTag +">";

            switch (type)
            { 
                case ErrorType.SyntaxError:
                    return _buffer + "Syntax error: " + errorText;

                case ErrorType.CommandError:
                    return _buffer + "Command error: " + errorText;

                case ErrorType.parError:
                    return _buffer + "Parameter error: " + errorText;

                case ErrorType.argError:
                    return _buffer + "Argument error: " + errorText;

                case ErrorType.argTypeError:
                    return _buffer + "Argument-type error: " + errorText;

                case ErrorType.inputError:
                    return _buffer + "Input error: " + errorText;

                case ErrorType.outputError:
                    return _buffer + "Output error: " + errorText;

                default:
                    return "";
            }
        }
    }
}
