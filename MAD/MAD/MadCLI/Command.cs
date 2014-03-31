using System;
using System.Collections.Generic;

namespace MAD
{
    public abstract class Command
    {
        //public List<object[]> mustIndicators = new List<object[]>();

        public List<object[]> requiredIndicators = new List<object[]>();
        public List<object[]> optionalIndicators = new List<object[]>();

        public List<string[]> args = new List<string[]>();

        public bool ValidArguments(List<string[]> args)
        {
            // check if all arguments are known by the command
            foreach (string[] temp in args)
                if (!ArgumentExists(temp[0]))
                {
                    ErrorMessage("Argument \"" + temp[0] + "\" do not exist!");
                    return false;
                }

            // check if all needed arguments are known
            int i = 0;
            foreach (string[] temp in args)
                if (RequiredArgumentExists(temp[0]))
                    i++;
            if (requiredIndicators.Count != i)
            {
                ErrorMessage("Missing required arguments!");
                return false;
            }
           
            // check the if the types are valid
            for (int i2 = 0; i2 < args.Count; i2++)
            {
                string[] temp = args[i2];

                try
                {
                    int i3 = Int32.Parse(temp[1]);
                    Type type = GetType(temp[0]);
                    if (GetType(temp[0]) != typeof(Int32))
                    {
                        ErrorMessage("Argument \"" + temp[0] + "\" must be a string!");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    if (GetType(temp[0]) != typeof(string))
                    {
                        ErrorMessage("Argument \"" + temp[0] + "\" must be a number!");
                        return false;
                    }
                }
            }

            return true;
        }

        public void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public bool RequiredArgumentExists(string indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if ((string)temp[0] == indicator)
                    return true;
            return false;
        }

        public bool OptionalArgumentExists(string indicator)
        {
            foreach (object[] temp in optionalIndicators)
                if ((string)temp[0] == indicator)
                    return true;
            return false;
        }

        public bool OptionalArgumentUsed(string indicator)
        { 
            foreach(object[] temp in optionalIndicators)
            foreach(string[] temp2 in args)
            {
                if ((string)temp[0] == temp2[0])
                    return true;
            }
            return false;
        }

        public void SetArguments(List<string[]> args)
        {
            this.args = args;
        }

        public Type GetType(string identifier)
        { 
            foreach(object[] temp in requiredIndicators)
            {
                if((string)temp[0] == identifier)
                    return (Type)temp[1];
            }

            foreach (object[] temp in optionalIndicators)
                if ((string)temp[0] == identifier)
                    return (Type)temp[1];

            return null;
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

        public bool ArgumentExists(string indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if ((string)temp[0] == indicator)
                    return true;

            foreach (object[] temp in optionalIndicators)
                if ((string)temp[0] == indicator)
                    return true;

            return false;
        }

        public bool ArgumentEmpty(string indicator)
        {
            foreach (string[] temp in args)
                if (temp[0] == indicator)
                    if (temp.Length == 1)
                        return true;
            return false;
        }

        /// <summary>
        /// Execute command
        /// </summary>
        public abstract int Execute();
    }
}
