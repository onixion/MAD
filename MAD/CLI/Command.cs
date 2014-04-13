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

        /// <summary>
        /// Set parameters of the command.
        /// </summary>
        public void SetParameters(ParameterInput parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// Check if parameter exist for the command.
        /// </summary>
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
        /// Check if a required parameter exist for the command.
        /// </summary>
        public bool RequiredParameterExist(Parameter parameter)
        {
            foreach (ParameterOption temp in requiredParameter)
                if (temp.indicator == parameter.indicator)
                    return true;

            return false;
        }

        /// <summary>
        /// Check if an optioanl parameter exist for the command.
        /// </summary>
        public bool OptionalParameterExist(Parameter parameter)
        {
            foreach (ParameterOption temp in optionalParameter)
                if (temp.indicator == parameter.indicator)
                    return true;

            return false;
        }

        /// <summary>
        /// Check if a optional parameter is used.
        /// </summary>
        public bool OptionalParameterUsed(string indicator)
        {
            return false;
        }

        /// <summary>
        /// Get the argument type.
        /// </summary>
        public Type GetArgumentType(Parameter parameter)
        {
            foreach (ParameterOption temp in requiredParameter)
                if (temp.indicator == parameter.indicator)
                    return temp.argumentType;

            foreach (ParameterOption temp in requiredParameter)
                if (temp.indicator == parameter.indicator)
                    return temp.argumentType;

            return null;
        }

        /// <summary>
        /// Check if the given parameters are valid for the command.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool ValidParameters(ParameterInput parameters)
        {
            int requiredArgsFound = 0;

            for (int i = 0; i < parameters.parameters.Count; i++)
            {
                Parameter temp = parameters.parameters[i];

                // check if all arguments are known by the command
                if (!ParameterExists(temp))
                {
                    MadComponents.components.cli.ErrorMessage("Parameter '-" + temp.indicator + "' does not exist for this command!");
                    return false;
                }

                // if the given arg is a required arg increase requiredArgsFound
                if (RequiredParameterExist(temp))
                    requiredArgsFound++;

                // check if the given args can have a value or not
                if (GetParameterOptions(temp.indicator).argumentEmpty)
                {
                    MadComponents.components.cli.ErrorMessage("Value of parameter '-" + temp.indicator + "' can't be null!");
                    return false;
                }

                // try to parse the argument to the specific type
                object argument = Convert((string)temp.value,GetArgumentType(temp));

                if (argument == null)
                {
                    MadComponents.components.cli.ErrorMessage("Could not parse argument '" + temp.value + ". Type help for view full commands.");
                    return false;
                }
                else
                {
                    // set value to the parsed object
                    temp.value = argument;
                }
            }

            // check if all required parameters are known
            if (requiredParameter.Count != requiredArgsFound)
            {
                MadComponents.components.cli.ErrorMessage("Some required parameters are missing! Type 'help' to see full commands.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Convert argument (string) to the needed type and return it.
        /// (this method can only parse Int32, String and IPAddress)
        /// </summary>
        private object Convert(string value, Type convertType)
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
                        return IPAddress.Parse(value);
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get a ParameterOptions object from a specific parameter.
        /// </summary>
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

        /// <summary>
        /// This abstract method will be executed every cycle (=delayTime).
        /// </summary>
        public abstract void Execute();
    }
}
