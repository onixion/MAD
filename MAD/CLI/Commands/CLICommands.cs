using System;
using System.Collections.Generic;

using MAD.CLIIO;
using MAD.DHCPReader;

namespace MAD.CLICore
{
    public class ExitCommand : Command
    {
        public ExitCommand()
            : base()
        {
            description = "This command exits the cli.";
        }

        public override string Execute(int consoleWidth)
        {
            return "EXIT_CLI";
        }
    }

    public class HelpCommand : Command
    {
        private List<CommandOptions> _commands;

        public HelpCommand(object[] commands)
            : base()
        {
            _commands = (List<CommandOptions>)commands[0];

            optionalParameter.Add(new ParameterOption("id", "<COMMAND-ID>", "ID of the specific command." , false, false, new Type[] { typeof(int) }));
            description = "This command shows information about available commands.";
        }

        public override string Execute(int consoleWidth)
        {
            if (!OptionalParameterUsed("id"))
            {
                output += "<color><yellow>Type 'help -id <COMMAND-ID>' to get more information about a command.\n";
                output += "<color><yellow>Available Commands:\n\n";

                for (int i = 0; i < _commands.Count; i++)
                {
                    output += "<color><white>" + _commands[i].command + "<color><darkyellow> [" + i + "]";

                    if (i != _commands.Count - 1)
                    {
                        output += "<color><white>, ";
                    }
                }

                output += "\n";
            }
            else
            {
                int commandIndex = (int)parameters.GetParameter("id").argumentValues[0];

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
                                    output += "<ARGUMENT_1> <ARGUMENT_2> ...";
                                }
                                else
                                {
                                    if (!_temp.argumentEmpty)
                                    {
                                        output += "<ARGUMENT>";
                                    }
                                }

                                output += "\n";
                                output += "\t<color><gray>" + _temp.description + "\n\n";
                            }
                        }

                        if (tempCommand.optionalParameter.Count != 0)
                        {
                            output += "\t<color><yellow>OPTIONAL PARAMETER\n\n";

                            foreach (ParameterOption _temp in tempCommand.optionalParameter)
                            {
                                output += "\t<color><darkyellow>-" + _temp.parameter + " <color><white>";

                                if (_temp.multiArguments)
                                {
                                    output += "<ARGUMENT_1> <ARGUMENT_2> ...";
                                }
                                else
                                {
                                    if (!_temp.argumentEmpty)
                                    {
                                        output += "<ARGUMENT>";
                                    }
                                }

                                output += "\n";
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
                _buffer += " -" + _temp.parameter;

                if (!_temp.argumentEmpty)
                {
                    _buffer += " <" + _temp.parameterInfo + ">";
                }
            }

            foreach (ParameterOption _temp in _command.optionalParameter)
            {
                _buffer += " [-" + _temp.parameter;

                if (!_temp.argumentEmpty)
                { 
                    _buffer += " <" + _temp.parameterInfo + ">";
                }

                _buffer += "]";
            }

            return _buffer;
        }
    }

    public class InfoCommand : Command
    {
        public InfoCommand()
            :base()
        {
            optionalParameter.Add(new ParameterOption("hack", null, null, true, false, null));
            description = "This command shows informations about the program.";
        }

        public override string Execute(int consoleWidth)
        {
            output += "\n<color><yellow>MAD - Network Monitoring v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n\n";
            output += "<color><yellow>Program written by: \n";
            output += "<color><white><PORCIC Alin> <RANALTER Daniel> <SINGH Manpreet> <STOJANOVIC Marko>";

            if (OptionalParameterUsed("hack"))
            {
                output += "<color><red>\n\nBe careful, Jack may be listening ...";
            }

            return output;
        }
    }

    public class ColorTestCommand : Command
    {
        public ColorTestCommand()
            : base()
        {
            description = "This command prints all supported colors to the console.";
        }

        public override string Execute(int consoleWidth)
        {
            output += "<color><white>" + CLIOutput.colors.Count + " colors available.\n";

            foreach (object[] _temp in CLIOutput.colors)
            {
                output += "<color>" + (string) _temp[0] + (string) _temp[0] + "\n";
            }
            
            return output;
        }
    }

    public class CatchBasicInfoStartCommand : Command
    {
        private MACFeeder _feeder;

        public CatchBasicInfoStartCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
            description = "Starts the Listener for Basic Information like MAC and IP, and how they are assigned.";
        }

        public override string Execute()
        {
            string _tmp = _feeder.Start();
            if (_tmp != null)
                return "<color><red>" + _tmp;
            else
                return "<color><blue>Start listening for Information";
        }
    }

    public class CatchBasicInfoStopCommand : Command
    {
        private MACFeeder _feeder;

        public CatchBasicInfoStopCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
            description = "Stops the listener for basic information like MAC and IP, and how they are assigend.";
        }

        public override string Execute()
        {
            string _tmp = _feeder.Stop();
            if (_tmp == null)
                return "<color><blue>Ends";
            else
                return "<color><red>" + _tmp;
        }
    }

    public class CatchBasicInfoSetTimeIntervallCommand : Command
    {
        private MACFeeder _feeder;
        private string paramDescripe = "Intervall in which the System should check. In seconds";

        public CatchBasicInfoSetTimeIntervallCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
            requiredParameter.Add(new ParameterOption("t", "INTERVALL-TIME IN SECONDS", paramDescripe , false, false, new Type[] { typeof(int) }));
            description = "Sets the time intevall in which the programm checks if the MACs still have the assigned IP or if they changed.";  
        }

        public override string Execute()
        {
            int _time = 1000 * (int)parameters.GetParameter("t").argumentValues[0];
            uint _utime = Convert.ToUInt32(_time);
            _feeder.ChangeCheckIntervall(_utime);
            return "<color><blue>Changed";
        }
    }

    public class CatchBasicInfoPrintHostsCommand : Command
    {
        private MACFeeder _feeder;

        public CatchBasicInfoPrintHostsCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
            description = "Prints all hosts which are currently in the List";
        }

        public override string Execute()
        {
            string _tmp = _feeder.PrintLists();

            return "<color><blue>" + _tmp;
        }
    }
}