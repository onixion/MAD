using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace MAD
{
    public abstract class Command
    {
        public List<object[]> args = new List<object[]>();

        public List<object[]> requiredIndicators = new List<object[]>();
        public List<object[]> optionalIndicators = new List<object[]>();

        public bool ValidArguments(List<object[]> args)
        {
            int requiredArgsFound = 0;

            foreach (string[] temp in args)
            {
                // check if all arguments are known by the command
                if (!ArgumentExists(temp[0]))
                {
                    ErrorMessage("Argument \"" + temp[0] + "\" does not exist for this command!");
                    return false;
                }

                // if the given arg is a required arg increase requiredArgsFound
                if (RequiredArgumentExist(temp[0]))
                    requiredArgsFound++;

                // check if the given args can have a value or not
                if (!GetArgumentConfig(temp[0]))
                {
                    if (temp[1] == null)
                    {
                        ErrorMessage("Argument \"-" + temp[0] + "\" can not be null!");
                        return false;
                    }
                }

                try
                {
                    //var converter = TypeDescriptor.GetConverter("LOL");
                    //var result = converter.ConvertFrom("AWS");
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

            }

            // check if all required args are known
            if (requiredIndicators.Count != requiredArgsFound)
            {
                ErrorMessage("Some required arguments are missing!");
                return false;
            }

            return true;
        }

        public Type GetType(string indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if (temp[0].ToString() == indicator)
                    return Type.GetType(temp[2].ToString());
            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == indicator)
                    return Type.GetType(temp[2].ToString());
            return null;
        }

        public void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public bool RequiredArgumentExist(string indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if ((string)temp[0] == indicator)
                    return true;
            return false;
        }

        public bool OptionalArgumentExist(string indicator)
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

        public void SetArguments(List<object[]> args)
        {
            this.args = args;
        }

        public bool GetArgumentConfig(string identifier)
        { 
            foreach(object[] temp in requiredIndicators)
                if((string)temp[0] == identifier)
                    return (bool)temp[1];

            foreach (object[] temp in optionalIndicators)
                if ((string)temp[0] == identifier)
                    return (bool)temp[1];

            return false;
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
