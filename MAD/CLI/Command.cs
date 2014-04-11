using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace MAD
{
    public abstract class Command
    {
        public List<object[]> args = new List<object[]>();
        public List<object[]> requiredIndicators = new List<object[]>();
        public List<object[]> optionalIndicators = new List<object[]>();

        public bool ValidArguments(List<object[]> args)
        {
            int requiredArgsFound = 0;

            for (int i = 0; i < args.Count; i++)
            {
                object[] temp = args[i];

                // check if all arguments are known by the command
                if (!ParameterExists(temp[0]))
                {
                    ErrorMessage("Parameter '-" + temp[0] + "' does not exist for this command!");
                    return false;
                }

                // if the given arg is a required arg increase requiredArgsFound
                if (RequiredParameterExist(temp[0]))
                    requiredArgsFound++;

                // check if the given args can have a value or not
                if (!ParameterConfigEmpty(temp[0]))
                {
                    if (temp[1] == null)
                    {
                        ErrorMessage("Value of argument '-" + temp[0] + "' can't be null!");
                        return false;
                    }
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

                /*
                Type neededType = GetType(temp[0]);
                var typeChanger = TypeDescriptor.GetConverter(neededType);
                */

                try
                {
                    //temp[1] = Convert.ChangeType(temp[1], GetType(temp[0]));
                    //typeChanger.ConvertFrom(temp[0]);
                }
                catch (Exception)
                {
                    ErrorMessage("Could not parse argument '" + temp[1] + "' to " /*+ neededType.ToString() + "!"*/);
                    return false;
                }
            }

            // check if all required args are known
            if (requiredIndicators.Count != requiredArgsFound)
            {
                ErrorMessage("Some required parameters are missing! Type 'help' to see full commands.");
                return false;
            }

            return true;
        }

        public Type GetType(object indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return (Type)temp[2];
            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return (Type)temp[2];
            return null;
        }

        public void ErrorMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public bool RequiredParameterExist(object indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return true;
            return false;
        }

        public bool OptionalParameterExist(object indicator)
        {
            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return true;
            return false;
        }

        public bool OptionalParameterUsed(object indicator)
        { 
            foreach(object[] temp in optionalIndicators)
            foreach(string[] temp2 in args)
            {
                if ((string)temp[0] == temp2[0])
                    return true;
            }
            return false;
        }

        public void SetParameters(List<object[]> args)
        {
            this.args = args;
        }

        public bool ParameterConfigEmpty(object identifier)
        { 
            foreach(object[] temp in requiredIndicators)
                if (temp[0].ToString() == identifier.ToString())
                    return (bool)temp[1];

            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == identifier.ToString())
                    return (bool)temp[1];

            return false;
        }

        // this must be object
        public string GetArgument(object identifier)
        {
            foreach(object[] temp in args)
                if (temp.Length == 2)
                    if (identifier.ToString() == temp[0].ToString())
                        return (string)temp[1];
            return null;
        }

        public bool ParameterExists(object indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return true;
            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return true;
            return false;
        }

        public bool ArgumentEmpty(string indicator)
        {
            foreach (string[] temp in args)
                if (temp[0] == indicator)
                    if (temp.Length == 1)
                        return true;
            return false;
        }

        public abstract int Execute();
    }
}
