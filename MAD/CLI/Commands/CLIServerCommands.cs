using System;

namespace MAD.CLI
{
    public class CLIServerInfo : Command
    {
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
