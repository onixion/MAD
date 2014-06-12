using System;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class HelpCommand : Command
    {
        private List<CommandOptions> _commands;

        public HelpCommand(object[] commands)
        {
            _commands = (List<CommandOptions>)commands[0];

            InitCommand();

            optionalParameter.Add(new ParameterOption("id", "COMMAND-ID", "ID for the specific command." , false, false, new Type[] { typeof(int) }));
            description = "This command shows information about other available commands.";
        }

        public override string Execute()
        {
            if (!OptionalParameterUsed("id"))
            {
                output += "<color><yellow>Type 'help -id <COMMAND_ID>' to get more information about a command.\n";
                output += "<color><yellow>Available Commands:\n\n";
                output += "<color><white>";

                for (int i = 0; i < _commands.Count; i++)
                {
                    output += _commands[i].command + "<color><darkyellow> [" + i + "]<color><white>";

                    if (i != _commands.Count - 1)
                    {
                        output += ", ";
                    }
                }

                output += "\n";
            }
            else
            {
                int commandIndex = (int)parameters.GetParameter("id").argumentValue[0];

                try
                {
                    CommandOptions commandOptions = _commands[commandIndex];
                    Type commandType = commandOptions.commandType;
                    Command tempCommand;

                    System.Reflection.ConstructorInfo cInfo = commandType.GetConstructor(new Type[1] { typeof(object[]) });

                    if (cInfo == null)
                    {
                        cInfo = commandType.GetConstructor(new Type[0]);
                        tempCommand = (Command)cInfo.Invoke(null);
                    }
                    else
                    {
                        tempCommand = (Command)cInfo.Invoke(new object[] { commandOptions.commandObjects });
                    }

                    output += "<color><yellow>COMMAND     <color><white>" + commandOptions.command + "<color><darkyellow> [" + commandIndex + "]\n";
                    output += "<color><yellow>DESCRIPTION <color><white>" + tempCommand.description + "\n";
                    output += "<color><yellow>USAGE       <color><white>" + GenUsageText(tempCommand, commandOptions) + "\n";
                    output += "<color><yellow>PARAMETER\n";

                    if (!(tempCommand.requiredParameter.Count == 0 && tempCommand.optionalParameter.Count == 0))
                    {
                        if (tempCommand.requiredParameter.Count != 0)
                        {
                            output += "\t<color><yellow>REQUIRED PARAMETER\n\n";

                            foreach (ParameterOption _temp in tempCommand.requiredParameter)
                            {
                                output += "\t<color><darkyellow>-" + _temp.parameter + " <color><white>";

                                if (_temp.multiArguments)
                                {
                                    output += "<ARGUMENT_1> <ARGUMENT_2> ...\n";
                                }
                                else
                                {
                                    output += "<ARGUMENT>\n";
                                }

                                output += "\t<color><gray>" + _temp.description + "\n\n";
                            }
                        }

                        if (tempCommand.optionalParameter.Count != 0)
                        {
                            output += "\t<color><yellow>REQUIRED PARAMETER\n\n";

                            foreach (ParameterOption _temp in tempCommand.optionalParameter)
                            {
                                output += "\t<color><darkyellow>-" + _temp.parameter + " <color><white>";

                                if (_temp.multiArguments)
                                {
                                    output += "<ARGUMENT_1> <ARGUMENT_2> ...\n";
                                }
                                else
                                {
                                    if (!_temp.argumentEmpty)
                                    {
                                        output += "<ARGUMENT>\n";
                                    }
                                    {
                                        output += "\n";
                                    }
                                }

                                output += "\t<color><gray>" + _temp.description + "\n\n";
                            }
                        }
                    }
                    else
                    {
                        output += "\n\t<color><gray>(command does not use any parameter)\n";
                    }
                }
                catch (Exception)
                {
                    output = "<color><red>Command with the ID '" + commandIndex + "' does not exist!";
                }
            }

            return output;
        }

        private string GenUsageText(Command _command, CommandOptions _commandOptions)
        {
            string _buffer = _commandOptions.command;

            foreach (ParameterOption _temp in _command.requiredParameter)
            {
                _buffer += " -" + _temp.parameter + " <" + _temp.parameterInfo + ">";
            }

            foreach (ParameterOption _temp in _command.optionalParameter)
            {
                _buffer += " [-" + _temp.parameter + " <" + _temp.parameterInfo + ">]";
            }

            return _buffer;
        }
    }

    public class InfoCommand : Command
    {
        public InfoCommand()
        {
            InitCommand();
        }

        public override string Execute()
        {
            output += "\n<color><yellow>MAD - Network Monitoring v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n";

            output += "Program written by: \n";
            output += "<color><white><PORCIC Alin>\n<RANALTER Daniel>\n<SINGH Manpreet>\n<STOJANOVIC Marko>\n\n";

            output += "<color><yellow>Components:<color><white>\n";
            output += "CLI         v" + MadComponents.components.cli.version.ToString() + " (CLI-Framework v" + MadComponents.components.cli.versionFramework + ")\n";
            output += "CLI-Server  v" + MadComponents.components.cliServer.version.ToString() + "\n";
            output += "JobSystem   v" + MadComponents.components.jobSystem.version.ToString();

            return output + "\n";
        }
    }

    public class ColorTestCommand : Command
    {
        public ColorTestCommand()
        {
            InitCommand();

            description = "This command prints all supported colors to the console.";
        }

        public override string Execute()
        {
            output += "<color><white>" + ConsoleWriter.colors.Count + " colors available.\n";
            
            foreach(object[] temp in ConsoleWriter.colors)
            {
                output += "<color>" + (string) temp[0] + (string) temp[0] + "\n";
            }
            
            return output;
        }
    }

    public class TestCommand : Command
    {
        public TestCommand()
        {
            // first init command
            InitCommand();

            // than add parameters and other things
            requiredParameter.Add(new ParameterOption("par", "TEXT", "Text to print to console.", false, true, new Type[] { typeof(string) }));
            optionalParameter.Add(new ParameterOption("par2", "INTEGER", "Integer to print to console.", false, true, new Type[] { typeof(int) }));
            description = "This command is used to test the CLI.";
        }

        public override string Execute()
        {
            // The Parameter "par" has multipleArguments on true, so that means
            // we get an array of objects, which we can convert to strings.
            object[] _buffer = parameters.GetParameter("par").argumentValue;

            // Because "par" is a required-parameter, we do not need to check
            // the value of it (optional-parameters need to be checked first!).
            output += "<color><white>Arguments of parameter 'par':\n";

            foreach (object _temp in _buffer)
            {
                output += "\t" + _temp.ToString();
            }

            output += "\n";

            // The parameter "par2" is an optional parameter, so we do not know for
            // sure if the user have set it or not. So we need to check, if the
            // parameter is used. Also it wants arguments as integer.
            if (OptionalParameterUsed("par2"))
            {
                // get arguments
                object[] _buffer2 = parameters.GetParameter("par2").argumentValue;

                output += "<color><white>Arguments of parameter 'par2':\n";

                foreach (object _temp in _buffer2)
                {
                    output += "\t" + _temp.ToString();
                }
            }
            else
            {
                output += "<color><red>Parameter 'par2' not used.\n";
            }

            return output;
        }
    }
}