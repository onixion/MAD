using System;
using System.Collections.Generic;
using System.Reflection;

namespace MAD
{
    public class CommandOptions
    {
        public string command;
        public Type commandType;

        // objects that are needed for execute the command
        public Type[] commandObjectsTypes;
        public object[] commandObjects;

        public CommandOptions(string command, Type commandType, Type[] commandObjectsTypes, object[] commandObjects)
        {
            this.command = command;
            this.commandType = commandType;
            this.commandObjectsTypes = commandObjectsTypes;
            this.commandObjects = commandObjects;
        }
    }
}
