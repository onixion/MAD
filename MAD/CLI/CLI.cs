using System;
using System.Reflection;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class CLI : CLIFramework
    {
        public CLI()
        { 
            // init CLI
            _InitCLI();
        }

        public void Start()
        {

            Console.WriteLine(CLIInfo());

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
                            Console.WriteLine(command.Execute());
                        }
                        else
                            Console.WriteLine(parameterValid);
                    }
                    else
                        Console.WriteLine("Command '" + commandInput + "' unknown! Type 'help' for more information.");
                }
            }
        }
    }
}
