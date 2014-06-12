using System;

namespace MAD.CLI
{
    /* This object defines a command. */
    public class CommandOptions
    {
        public string command;
        public Type commandType;
        public object[] commandObjects;

        public CommandOptions(string command, Type commandType, object[] commandObjects)
        {
            this.command = command;
            this.commandType = commandType;
            this.commandObjects = commandObjects;
        }
    }
}
