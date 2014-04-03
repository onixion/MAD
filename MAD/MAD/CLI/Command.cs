using System;
using System.Collections.Generic;

namespace MAD
{
    public abstract class Command
    {
        //public List<object[]> mustIndicators = new List<object[]>();

        public List<object[]> requiredIndicators = new List<object[]>();
        public List<object[]> optionalIndicators = new List<object[]>();

        public List<string[]> args = new List<string[]>();

        public bool ValidArguments(List<string[]> args)
        {
            // check if all arguments are known by the command
            foreach (string[] temp in args)
                if (!ArgumentExists(temp[0]))
                {
                    ErrorMessage("Argument \"" + temp[0] + "\" does not exist!");
                    return false;
                }

            // check if all needed arguments are known
            int i = 0;
            foreach (string[] temp in args)
                if (RequiredArgumentExist(temp[0]))
                    i++;
            if (requiredIndicators.Count != i)
            {
                ErrorMessage("Some arguments are missing!");
                return false;
            }
           
            // check if they have values if neede
            for (int i2 = 0; i2 < args.Count; i2++)
            {
                string[] temp = args[i2];

                if (!GetArgumentConfig(temp[0]))
                {
                    if (temp[1] == null)
                    {
                        ErrorMessage("Argument \"-" + temp[0] + "\" is null!");
                        return false;
                    }
                }
            }

            return true;
        }

        public void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public bool RequiredArgumentExist(string indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if ((string)temp[0] == indicator)
                    return true;
            return false;
        }

        public bool OptionalArgumentExist(string indicator)
        {
            foreach (object[] temp in optionalIndicators)
                if ((string)temp[0] == indicator)
                    return true;
            return false;
        }

        public bool OptionalArgumentUsed(string indicator)
        { 
            foreach(object[] temp in optionalIndicators)
            foreach(string[] temp2 in args)
            {
                if ((string)temp[0] == temp2[0])
                    return true;
            }
            return false;
        }

        public void SetArguments(List<string[]> args)
        {
            this.args = args;
        }

        public bool GetArgumentConfig(string identifier)
        { 
            foreach(object[] temp in requiredIndicators)
                if((string)temp[0] == identifier)
                    return (bool)temp[1];

            foreach (object[] temp in optionalIndicators)
                if ((string)temp[0] == identifier)
                    return (bool)temp[1];

            return false;
        }

        public string GetArgument(string identifier)
        {
            foreach (string[] temp in args)
            {
                if (temp.Length == 2)
                {
                    if (identifier == temp[0])
                        return temp[1];
                }
                else
                    return null;
            }

            return null;
        }

        public bool ArgumentExists(string indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if ((string)temp[0] == indicator)
                    return true;
            foreach (object[] temp in optionalIndicators)
                if ((string)temp[0] == indicator)
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

        /// <summary>
        /// Execute command
        /// </summary>
        public abstract int Execute();
    }
}
