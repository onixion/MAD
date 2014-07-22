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

            oPar.Add(new ParOption("id", "<COMMAND-ID>", "ID of the specific command." , false, false, new Type[] { typeof(int) }));
            description = "This command shows information about available commands.";
        }

        public override string Execute(int consoleWidth)
        {
            if (!OParUsed("id"))
            {
                output += "<color><yellow>Type 'help -id <COMMAND-ID>' to get more information about a command.\n";
                output += "<color><yellow>Available Commands:\n\n";
                for (int i = 0; i < _commands.Count; i++)
                    output += "<color><darkyellow>[" + i + "]" + "\t<color><white>" + _commands[i].command + "\n";
                output += "\n";
            }
            else
            {
                int commandIndex = (int)pars.GetPar("id").argValues[0];

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
                    output += "<color><yellow>par\n";

                    if (!(tempCommand.rPar.Count == 0 && tempCommand.oPar.Count == 0))
                    {
                        if (tempCommand.rPar.Count != 0)
                        {
                            output += "\t<color><yellow>REQUIRED par\n\n";

                            foreach (ParOption _temp in tempCommand.rPar)
                            {
                                output += "\t<color><darkyellow>-" + _temp.par + " <color><white>";

                                if (_temp.multiargs)
                                    output += "<arg_1> <arg_2> ...";
                                else
                                    if (!_temp.argEmpty)
                                        output += "<arg>";

                                output += "\n";
                                output += "\t<color><gray>" + _temp.description + "\n\n";
                            }
                        }

                        if (tempCommand.oPar.Count != 0)
                        {
                            output += "\t<color><yellow>OPTIONAL par\n\n";

                            foreach (ParOption _temp in tempCommand.oPar)
                            {
                                output += "\t<color><darkyellow>-" + _temp.par + " <color><white>";

                                if (_temp.multiargs)
                                    output += "<arg_1> <arg_2> ...";
                                else
                                    if (!_temp.argEmpty)
                                        output += "<arg>";

                                output += "\n";
                                output += "\t<color><gray>" + _temp.description + "\n\n";
                            }
                        }
                    }
                    else
                        output += "\n\t<color><gray>(command does not use any par)\n";
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

            foreach (ParOption _temp in _command.rPar)
            {
                _buffer += " -" + _temp.par;

                if (!_temp.argEmpty)
                    _buffer += " <" + _temp.parInfo + ">";
            }

            foreach (ParOption _temp in _command.oPar)
            {
                _buffer += " [-" + _temp.par;
                if (!_temp.argEmpty)
                    _buffer += " <" + _temp.parInfo + ">";
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
            oPar.Add(new ParOption("hack", null, null, true, false, null));
            description = "This command shows informations about the program.";
        }

        public override string Execute(int consoleWidth)
        {
            output += "\n<color><yellow>MAD - Network Monitoring v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n\n";
            output += "<color><yellow>Program written by: \n";
            output += "<color><white><PORCIC Alin> <RANALTER Daniel> <SINGH Manpreet> <STOJANOVIC Marko>";

            if (OParUsed("hack"))
            {
 
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
                output += "<color>" + (string) _temp[0] + (string) _temp[0] + "\n";
            
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

        public override string Execute(int consoleWidth)
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
            rPar.Add(new ParOption("t", "INTERVALL-TIME IN SECONDS", paramDescripe , false, false, new Type[] { typeof(int) }));
            description = "Sets the time intevall in which the programm checks if the MACs still have the assigned IP or if they changed.";  
        }

        public override string Execute(int consoleWidth)
        {
            int _time = 1000 * (int)pars.GetPar("t").argValues[0];
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

        public override string Execute(int consoleWidth)
        {
            string _tmp = _feeder.PrintLists();

            return "<color><blue>" + _tmp;
        }
    }
}