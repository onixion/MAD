using System;

namespace MAD.CLI
{
    public class CommandOptions
    {
        public string command;
        public Type commandType;
        public object[] commandParameterObjects;

        public CommandOptions(string command, Type commandType, object[] commandParameterObjects)
        {
            this.command = command;
            this.commandType = commandType;
            this.commandParameterObjects = commandParameterObjects;
        }
    }
}
