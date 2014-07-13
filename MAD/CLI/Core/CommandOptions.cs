using System;

namespace MAD.CLICore
{
    /* This object defines a command for the CLI. */
    public class CommandOptions
    {
        public string command;
        public Type commandType;
        public object[] commandObjects;

        public CommandOptions(string command, Type commandType, params object[] commandObjects)
        {
            this.command = command;
            this.commandType = commandType;
            this.commandObjects = commandObjects;
        }
    }
}
