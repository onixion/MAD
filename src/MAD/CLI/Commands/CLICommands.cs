﻿using System;
using System.Collections.Generic;

using CLIIO;

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
                    output += "<color><yellow>PARAMETER(S)\n";

                    if (!(tempCommand.rPar.Count == 0 && tempCommand.oPar.Count == 0))
                    {
                        if (tempCommand.rPar.Count != 0)
                        {
                            output += "\t<color><yellow>REQUIRED PARAMETER(S)\n\n";

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
                            output += "\t<color><yellow>OPTIONAL PARAMETER(S)\n\n";

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
            description = "This command shows informations about the program.";
        }

        public override string Execute(int consoleWidth)
        {
            output += "\n<color><yellow>MAD - Network Monitoring v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n\n";
            output += "<color><yellow>Program written by: \n";
            output += "<color><white><PORCIC Alin> <RANALTER Daniel> <SINGH Manpreet> <STOJANOVIC Marko>";

            return output;
        }
    }

    public class SetWidthCommand : Command
    {
        public SetWidthCommand()
            : base()
        {
            description = "This command sets the console width.";
            rPar.Add(new ParOption("w", "CONSOLE-WIDTH", "New console width.", false, false, new Type[] { typeof(int) })); 
        }

        public override string Execute(int consoleWidth)
        {
            Console.WindowWidth = (int)pars.GetPar("w").argValues[0];
            return "<color><green>Console width set.";
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

    public class LoadConfigFileCommand : Command
    {
        public LoadConfigFileCommand()
            : base()
        {
            description = "This command load a conf file from another path.";
            rPar.Add(new ParOption("p", "PATH-TO-CONF", "File to load from.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            string _filepath = (string)pars.GetPar("p").argValues[0];

            try
            {
                MadConf.LoadConf(_filepath);
                return "<color><green>Config loaded.";
            }
            catch (Exception e)
            {
                return "<color><red>Could not load config: " + e.Message;
            }
        }
    }

    public class LoadDefaultConfigCommand : Command
    {
        public LoadDefaultConfigCommand()
        {
            description = "This command loads the default conf file.";
        }

        public override string Execute(int consoleWidth)
        {
            try
            {
                MadConf.LoadConf(Mad.CONFFILE);
                return "<color><green>Default config loaded.";
            }
            catch (Exception e)
            {
                return "<color><red>Could not load default config: " + e.Message;
            }
        }
    }

    public class SaveConfigCommand : Command
    {
        public SaveConfigCommand()
        {
            description = "This command saves the current conf file.";
            rPar.Add(new ParOption("p", "PATH-TO-CONF", "File to load from.", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            string _filepath = (string)pars.GetPar("p").argValues[0];

            try
            {
                MadConf.SaveConf(_filepath);
                return "<color><green>Config saved.";
            }
            catch (Exception e)
            {
                return "<color><red>Could not save conf file: " + e.Message;
            }
        }
    }

    public class ConfShowCommand : Command
    {
        public ConfShowCommand()
        {
            description = "This command shows the content of the current loaded conf file.";
        }

        public override string Execute(int consoleWidth)
        {
            //output = "<color><yellow>VERSION: <color><white>" + MadConf.conf.VERSION + "\n";

            return output;
        }
    }
}