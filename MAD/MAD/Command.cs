using System;
using System.Collections.Generic;

namespace MAD
{
    abstract class Command
    {
        public string mainCommand;
        public List<string> requiredIndicators;
        public List<string> optionalIndicators;

        public virtual bool ValidArguments(List<string[]> indicators)
        {
            int i = 0;

            foreach (string[] temp in indicators)
            {
                if (requiredIndicators.Contains(temp[0]))
                    i++;
            }

            if (requiredIndicators.Count == i)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Execute command
        /// </summary>

        public abstract void Execute();
    }
}
