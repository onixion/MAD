using System;
using System.Collections.Generic;

namespace MAD
{
    public abstract class Command
    {
        public List<string> requiredIndicators = new List<string>();
        public List<string> optionalIndicators = new List<string>();

        public List<string[]> args;

        public bool ValidArguments(List<string[]> args)
        {
            // check if any args are empty
            foreach (string[] temp in args)
                if (temp.Length == 0)
                    return false;

            // check if all arguments are known by the command
            foreach (string[] temp in args)
                if (!ArgumentExists(temp[0]))
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

        /// <summary>
        /// Returns true if the arguments exists
        /// </summary>
        public bool ArgumentExists(string indicator)
        {
            if (requiredIndicators.Contains(indicator))
                return true;

            if (optionalIndicators.Contains(indicator))
                return true;
        
            return false;
        }

        /// <summary>
        /// Execute command
        /// </summary>
        public abstract int Execute();
    }
}
