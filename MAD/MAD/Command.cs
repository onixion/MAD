using System;
using System.Collections.Generic;

namespace MAD
{
    abstract class Command
    {
        public string mainCommand;
        public List<string[]> args;

        public List<string> requiredIndicators = new List<string>();
        public List<string> optionalIndicators = new List<string>();

        public virtual bool ValidArguments(List<string[]> args)
        {
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
        public abstract void Execute();
    }
}
