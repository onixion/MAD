using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAD
{
    abstract class Command
    {
        public string mainCommand;
        public enum validArg { a }

        /// <summary>
        /// Execute command
        /// </summary>

        public abstract void Execute();
    }
}
