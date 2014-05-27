using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class CLI : CLIFramework
    {
        public Version version = new Version(1, 0, 7000);

        public CLI()
        { 
            // init CLI
            _InitCLI();
        }

        public void Start()
        {

            while (true)
            {
                // print cursor
                Console.Write(cursor);

                // read input
                cliInput = Console.ReadLine();

                if (cliInput != "")
                {
                    // get command
                    commandInput = GetCommand(cliInput);

                    // check if command are known
                    if (CommandExists(commandInput))
                    {
                        // get arguments from input
                        parameterInput = GetParamtersFromInput(cliInput);
                        // get command type
                        inputCommandType = GetCommandType(commandInput);

                        // create command object (pass the command none objects)
                        command = (Command)inputCommandType.GetConstructor(new Type[0]).Invoke(new object[0]);

                        // check if the arguments are valid (string = VALID_PARAMETER)
                        parameterValid = command.ValidParameters(parameterInput);

                        if (parameterValid == "VALID_PARAMETER")
                        {
                            // set command parameters 
                            command.SetParameters(parameterInput);

                            // EXECUTE COMMAND AND SEND OUTPUT
                            cliWriter.WriteToConsole(command.Execute() + "\n");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(parameterValid);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Command '" + commandInput + "' unknown! Type 'help' for more information.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }
        }
    }
}
