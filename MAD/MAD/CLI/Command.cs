﻿using System;
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

            for (int i = 0; i < args.Count; i++)
            {
                object[] temp = args[i];

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
                    /*  TODO: check what type the command wants -> GetType(temp[0])
                     *  get argument type and try to parse it into the type
                     *  
                     *  if not working -> Error: Could not parse argument ...
                     *  if working     -> continue 
                     * 
                     *  now it is not needed to check (inside a command) if the argument value
                     *  have the right type, this saves time and lines of codes
                     */

                    // check if the argument value can be parsed to the needed type
                    //temp[1] = Convert.ChangeType(2, GetType(temp[0]));
                    //temp[1] = Convert.ChangeType(temp[1], Type.GetType(typeof(Int32).ToString()));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    ErrorMessage("Could not parse argument \"" + temp[1].ToString() + "\" to " + GetType(temp[0]));
                    return false;
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

        public Type GetType(object indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return Type.GetType(temp[2].ToString());
            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return Type.GetType(temp[2].ToString());
            return null;
        }

        public void ErrorMessage(object message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[ERROR] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public bool RequiredArgumentExist(object indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return true;
            return false;
        }

        public bool OptionalArgumentExist(object indicator)
        {
            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return true;
            return false;
        }

        public bool OptionalArgumentUsed(object indicator)
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

        public bool GetArgumentConfig(object identifier)
        { 
            foreach(object[] temp in requiredIndicators)
                if (temp[0].ToString() == identifier.ToString())
                    return (bool)temp[1];

            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == identifier.ToString())
                    return (bool)temp[1];

            return false;
        }

        public object GetArgument(object identifier)
        {
            foreach (object[] temp in args)
            {
                if (temp.Length == 2)
                {
                    if (identifier.ToString() == temp[0].ToString())
                        return temp[1];
                }
                else
                    return null;
            }

            return null;
        }

        public bool ArgumentExists(object indicator)
        {
            foreach (object[] temp in requiredIndicators)
                if (temp[0].ToString() == indicator.ToString())
                    return true;
            foreach (object[] temp in optionalIndicators)
                if (temp[0].ToString() == indicator.ToString())
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

        public abstract int Execute();
    }
}
