using System;

namespace MAD.CLI
{
    class CLIServerInfo : Command
    {
        public CLIServerInfo()
        { 
        
        }

        public override string Execute()
        {
            output += "<color><red>CLI-Server v" + MadComponents.components.cliServer.version + " (CLI-Framework v" + MadComponents.components.cliServer.versionFramework + ") \n";
            output += "<color><yellow>";
            output += "Server listening: " + MadComponents.components.cliServer.listening + "\n";
            output += "Active Sessions:  " + MadComponents.components.cliServer.sessions.Count + "\n";
            output += "Available Users:  " + MadComponents.components.cliServer.users.Count + "\n";

            return output;
        }
    }

    class CLIServerStart : Command
    {
        public CLIServerStart()
        {

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

    class CLIServerStop : Command
    {
        public CLIServerStop()
        {

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
