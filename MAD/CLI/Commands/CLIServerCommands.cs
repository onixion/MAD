﻿using System;

namespace MAD.CLI
{
    public class CLIServerInfo : Command
    {
        public CLIServerInfo()
        {
            InitCommand();
            description = "This command shows informations about the CLIServer.";
        }

        public override string Execute()
        {
            output += "<color><yellow>CLI-Server v" + Handler.components.cliServer.version + "\n";
            output += "Server listening: " + Handler.components.cliServer.listening + "\n";
            output += "Active Sessions:  " + Handler.components.cliServer.sessions.Count + "\n";
            output += "Available Users:  " + Handler.components.cliServer.users.Count;

            return output;
        }
    }

    public class CLIServerStart : Command
    {
        public CLIServerStart()
        {
            InitCommand();

            description = "This command starts the CLIServer.";
        }

        public override string Execute()
        {
            try
            {
                Handler.components.cliServer.Start();
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
            InitCommand();

            description = "This command stops the CLIServer.";
        }

        public override string Execute()
        {
            try
            {
                Handler.components.cliServer.Stop();
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
