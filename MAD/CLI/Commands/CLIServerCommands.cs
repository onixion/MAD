using System;

namespace MAD.CLI
{
    public class CLIServerInfo : Command
    {
        public CLIServerInfo()
        {
            description = "This command shows informations about the CLIServer.";
            usage = "cliserver";
        }

        public override string Execute()
        {
            output += "<color><yellow>CLI-Server v" + MadComponents.components.cliServer.version + "\n";
            output += "Server listening: " + MadComponents.components.cliServer.listening + "\n";
            output += "Active Sessions:  " + MadComponents.components.cliServer.sessions.Count + "\n";
            output += "Available Users:  " + MadComponents.components.cliServer.users.Count;

            return output;
        }
    }

    public class CLIServerStart : Command
    {
        public CLIServerStart()
        {
            description = "This command starts the CLIServer.";
            usage = "cliserver start";
        }

        public override string Execute()
        {
            try
            {
                MadComponents.components.cliServer.Start();
                output += "<color><green>CLI server started.";
            }
            catch (Exception e)
            {
                output += "<color><red>" + e.Message;
            }
            
            return output;
        }
    }

    public class CLIServerStop : Command
    {
        public CLIServerStop()
        {
            description = "This command stops the CLIServer.";
            usage = "cliserver stop";
        }

        public override string Execute()
        {
            try
            {
                MadComponents.components.cliServer.Stop();
                output += "<color><green>CLI server stopped.";
            }
            catch (Exception e)
            {
                output += "<color><red>" + e.Message;
            }

            return output;
        }
    }
}
