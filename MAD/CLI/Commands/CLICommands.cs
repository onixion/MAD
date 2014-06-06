using System;
using System.Collections.Generic;

namespace MAD.CLI
{
    public class HelpCommand : Command
    {
        private List<CommandOptions> commands;

        public HelpCommand(object[] commandPars)
        {
            this.commands = (List<CommandOptions>)commandPars[0];
            optionalParameter.Add(new ParameterOption("id", "Command-ID", false, typeof(int)));

            description = "This command shows information about all available commands.";
        }

        public override string Execute()
        {
            if (!OptionalParameterUsed("id"))
            {
                output += "<color><yellow>Type 'help -id <COMMAND-ID>' to get more information about a command.\n";
                output += "<color><yellow>Available Commands:\n\n";
                output += "<color><gray>";

                for (int i = 0; i < commands.Count; i++)
                {
                    output += commands[i].command + "<color><darkyellow> [" + i + "]<color><gray>";

                    if (i != commands.Count - 1)
                    {
                        output += ", ";
                    }
                }

                output += "\n";
            }
            else
            {
                int commandIndex = (int)parameters.GetParameter("id").value;

                try
                {
                    CommandOptions commandOptions = commands[commandIndex];
                    output += "<color><yellow>COMMAND <color><gray> " + commandOptions.command + "<color><darkyellow>[" + commandIndex + "]<color><gray>\n\n";

                    Type commandType = commandOptions.commandType;
                    Command tempCommand = (Command)commandType.GetConstructor(new Type[1] { typeof(object[]) }).Invoke(new object[]{commandOptions.commandParameterObjects});

                    output += "DESCRIPTION: " + tempCommand.description + "\n\n";

                    if (tempCommand.requiredParameter.Count != 0)
                    {
                        output += "<color><yellow>REQUIRED PARAMETER\n\n";

                        foreach (ParameterOption _temp in tempCommand.requiredParameter)
                        {
                            output += "\t<color><darkyellow>-" + _temp.indicator + "\n";
                            output += "\t<color><white>Description: " + _temp.description + "\n";
                        }
                    }

                    if (tempCommand.optionalParameter.Count != 0)
                    {
                        output += "<color><yellow>OPTIONAL PARAMETER\n\n";

                        foreach (ParameterOption _temp in tempCommand.optionalParameter)
                        {
                            output += "\t<color><darkyellow>-" + _temp.indicator + "\n";
                            output += "\t<color><white>Description: " + _temp.description + "\n";
                        }
                    }

                    tempCommand = null;
                }
                catch (Exception)
                {
                    output = "<color><red>Command with the ID '" + commandIndex + "' does not exist!";
                }
            }

            return output;
        }
    }

    public class InfoCommand : Command
    {
        public override string Execute()
        {
            output += "<color><yellow>MAD - Network Monitoring v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n";
            output += "Components:\n";
            output += "CLI         v" + MadComponents.components.cli.version.ToString() + " (CLI-Framework v" + MadComponents.components.cli.versionFramework + ")\n";
            output += "CLI-Server  v" + MadComponents.components.cliServer.version.ToString() + "\n";
            output += "JobSystem   v" + MadComponents.components.jobSystem.version.ToString();
            return output;
        }
    }

    public class ColorTestCommand : Command
    {
        public ColorTestCommand(object[] commandParameter)
        {
            description = "This command tests all supported colors on the console.";
        
        }

        public override string Execute()
        {
            output += "<color><gray>" + ConsoleWriter.colors.Count + " colors supported.\n";
            
            foreach(object[] temp in ConsoleWriter.colors)
            {
                output += "<color>" + (string) temp[0] + (string) temp[0] + "\n";
            }
            
            return output;
        }
    }
}