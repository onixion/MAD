using System;
using System.Collections.Generic;
using System.Reflection;

namespace MAD
{
    public class CommandOptions
    {
        public string command;
        public Type commandType;
        public Type[] commandTypes;
        public object[] commandObjects;

        public CommandOptions(string command, Type commandType, Type[] commandTypes, object[] commandObjects)
        {
            this.command = command;
            this.commandType = commandType;
            this.commandTypes = commandTypes;
            this.commandObjects = commandObjects;
        }
    }
}
