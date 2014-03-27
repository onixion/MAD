using System;
using System.Collections.Generic;

namespace MAD
{
    public abstract class Command
    {
        public List<string> requiredIndicators = new List<string>();
        public List<string> optionalIndicators = new List<string>();

        public List<string[]> args = new List<string[]>();

        public bool ValidArguments(List<string[]> args)
        {
            // check if all arguments are known by the command
            foreach (string[] temp in args)
                if (!ArgumentSupported(temp[0]))
                    return false; 

            // check if all needed arguments are known
            int i = 0;
            foreach (string[] temp in args)
                if (requiredIndicators.Contains(temp[0]))
                    i++;

            if (requiredIndicators.Count == i)
                return true;
            else
                return false;
        }

        public void SetArguments(List<string[]> args)
        {
            this.args = args;
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

        public bool ArgumentSupported(string indicator)
        {
            foreach (string temp in requiredIndicators)
                if (temp == indicator)
                    return true;

            foreach (string temp in optionalIndicators)
                if (temp == indicator)
                    return true;

            return false;
        }

        public bool OptionalArgumentExists(string indicator)
        {
            for (int i = 0; i < args.Count; i++)
            {
                string[] buffer = args[i];

                if (buffer[0] == indicator)
                    return true;
            }

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
