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
            ParameterError,
            ArgumentError,
            ArgumentTypeError
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

                case ErrorType.ParameterError:
                    return _buffer + "Parameter error: " + errorText;

                case ErrorType.ArgumentError:
                    return _buffer + "Argument error: " + errorText;

                case ErrorType.ArgumentTypeError:
                    return _buffer + "Argument-type error: " + errorText;

                default:
                    return "";
            }
        }
    }
}
