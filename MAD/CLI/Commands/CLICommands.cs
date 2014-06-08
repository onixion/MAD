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
            optionalParameter.Add(new ParameterOption("id", "Command-ID (INT)", false, false, new Type[] { typeof(int) }));

            description = "This command shows information about other available commands.";
            usage = "help -id <COMMAND_ID>";
        }

        public override string Execute()
        {
            if (!OptionalParameterUsed("id"))
            {
                output += "<color><yellow>Type 'help -id <COMMAND-ID>' to get more information about a command.\n";
                output += "<color><yellow>Available Commands:\n\n";
                output += "<color><white>";

                for (int i = 0; i < commands.Count; i++)
                {
                    output += commands[i].command + "<color><darkyellow> [" + i + "]<color><white>";

                    if (i != commands.Count - 1)
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
                    CommandOptions commandOptions = commands[commandIndex];
                    output += "<color><yellow>COMMAND <color><white>" + commandOptions.command + "<color><darkyellow> [" + commandIndex + "]\n";

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

                    output += "<color><yellow>DESCRIPTION <color><white>" + tempCommand.description + "\n";

                    output += "<color><yellow>USAGE <color><white>" + tempCommand.usage + "\n";

                    output += "<color><yellow>PARAMETER\n";

                    if (!(tempCommand.requiredParameter.Count == 0 && tempCommand.optionalParameter.Count == 0))
                    {
                        if (tempCommand.requiredParameter.Count != 0)
                        {
                            output += "\t<color><yellow>REQUIRED PARAMETER\n";

                            foreach (ParameterOption _temp in tempCommand.requiredParameter)
                            {
                                output += "\t<color><darkyellow>-" + _temp.parameter + "<color><gray> " + _temp.description + "\n";
                            }

                            output += "\n";
                        }

                        if (tempCommand.optionalParameter.Count != 0)
                        {
                            output += "\t<color><yellow>OPTIONAL PARAMETER\n";

                            foreach (ParameterOption _temp in tempCommand.optionalParameter)
                            {
                                output += "\t<color><darkyellow>-" + _temp.parameter + "<color><gray> " + _temp.description + "\n";
                            }

                            output += "\n";
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
    }

    public class InfoCommand : Command
    {
        public InfoCommand()
        { 
            optionalParameter.Add(new ParameterOption("hack", true, null));
        }

        public override string Execute()
        {
            output += "\n<color><yellow>MAD - Network Monitoring v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n\n";

            output += "Components:<color><white>\n";
            output += "CLI         v" + MadComponents.components.cli.version.ToString() + " (CLI-Framework v" + MadComponents.components.cli.versionFramework + ")\n";
            output += "CLI-Server  v" + MadComponents.components.cliServer.version.ToString() + "\n";
            output += "JobSystem   v" + MadComponents.components.jobSystem.version.ToString();

            if (OptionalParameterUsed("hack"))
            {
                output += "\n\n<color><yellow>J<color><green>A<color><blue>C";
                output += "<color><red>K <color><magenta>B<color><darkgray>Ö";
                output += "<color><darkred>S<color><green>E <color><blue>";
                output += "W<color><yellow>A<color><magenta>S <color><blue>H";
                output += "<color><darkred>E<color><magenta>R<color><white>E ";
                output += "  <color><green>.<color><red>.<color><darkgray>.\n"; ;
            }

            return output;
        }
    }

    public class ColorTestCommand : Command
    {
        public ColorTestCommand()
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

    public class TestCommand : Command
    {
        public TestCommand()
        {
            description = "This command is used to test the CLI-Framework.";

            requiredParameter.Add(new ParameterOption("a", false, new Type[] { typeof(string), typeof(int) }));
            requiredParameter.Add(new ParameterOption("b", true, null));
        }

        public override string Execute()
        {
            output += "<color><white>-a\n";


            return "JKI";
        }
    }
}