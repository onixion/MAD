using System;

namespace MAD.CLI
{
    public class CommandOptions
    {
        public string command;
        public Type commandType;

        public CommandOptions(string command, Type commandType)
        {
            this.command = command;
            this.commandType = commandType;
        }
    }
}
