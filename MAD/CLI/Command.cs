using System;
using System.Collections.Generic;
using System.Reflection;
using System.Net;

namespace MAD
{
    public abstract class Command
    {
        public List<ParameterOption> requiredParameter = new List<ParameterOption>();
        public List<ParameterOption> optionalParameter = new List<ParameterOption>();

        public ParameterInput parameters;

        public bool ValidArguments(ParameterInput parameters)
        {
            int requiredArgsFound = 0;

            for (int i = 0; i < parameters.parameters.Count; i++)
            {
                Parameter temp = parameters.parameters[i];

                // check if all arguments are known by the command
                if (!ParameterExists(temp))
                {
                    ErrorMessage("Parameter '-" + temp.indicator + "' does not exist for this command!");
                    return false;
                }

                // if the given arg is a required arg increase requiredArgsFound
                if (RequiredParameterExist(temp))
                    requiredArgsFound++;

                // check if the given args can have a value or not
                if (GetParameterOptions(temp.indicator).argumentEmpty)
                {
                        ErrorMessage("Value of parameter '-" + temp.indicator + "' can't be null!");
                        return false;   
                }

                /*  TODO: check what type the command wants -> GetType(temp[0])
                 *  get argument type and try to parse it into the type neede 
                 *  for the command
                 *  
                 *  if not working -> Error: Could not parse argument ...
                 *  if working     -> continue 
                 * 
                 *  now it is not needed to check (inside a command) if the argument value
                 *  have the right type, this saves time and lines of codes
                 */
            }

            // check if all required args are known
            if (requiredParameter.Count != requiredArgsFound)
            {
                ErrorMessage("Some required parameters are missing! Type 'help' to see full commands.");
                return false;
            }

            return true;
        }

        public Type GetType(string indicator)
        {
            foreach (ParameterOption temp in requiredParameter)
                if (temp.indicator == indicator)
                    return temp.argumentType;
            foreach (ParameterOption temp in optionalParameter)
                if (temp.indicator == indicator)
                    return temp.argumentType;
            return null;
        }

        public void ErrorMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public bool RequiredParameterExist(Parameter parameter)
        {
            foreach (ParameterOption temp in requiredParameter)
                if (temp.indicator == parameter.indicator)
                    return true;
            return false;
        }

        public bool OptionalParameterExist(Parameter parameter)
        {
            foreach (ParameterOption temp in optionalParameter)
                if (temp.indicator == parameter.indicator)
                    return true;

            return false;
        }

        public ParameterOption GetParameterOptions(string indicator)
        {
            foreach (ParameterOption temp in requiredParameter)
                if (temp.indicator == indicator)
                    return temp;

            foreach (ParameterOption temp in optionalParameter)
                if (temp.indicator == indicator)
                    return temp;

            return null;
        }

        public void SetParameters(ParameterInput parameters)
        {
            this.parameters = parameters;
        }

        public bool ParameterExpectsEmptyArgument(Parameter parameter)
        {
            if (ParameterExists(parameter))
                return GetParameterOptions(parameter.indicator).argumentEmpty;

            return false;
        }

        public bool ParameterExists(Parameter parameter)
        {
            foreach (ParameterOption temp in requiredParameter)
                if (temp.indicator == parameter.indicator)
                    return true;

            foreach (ParameterOption temp in optionalParameter)
                if (temp.indicator == parameter.indicator)
                    return true;

            return false;
        }

        /// <summary>
        /// This abstract method will be executed every cycletime (delay).
        /// </summary>
        public abstract void Execute();
    }
}
