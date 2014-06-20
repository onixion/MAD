using System;

namespace MAD.cli
{
    public class CLIServerInfo : Command
    {
        public CLIServerInfo()
            : base()
        {
            description = "This command shows informations about the CLIServer.";
        }

        public override string Execute()
        {
            output += "<color><yellow>CLI-Server v";// +Handler.components.cliServer.version + "\n";
            output += "Server listening: ";// +Handler.components.cliServer.listening + "\n";

            return output;
        }
    }

    public class CLIServerStart : Command
    {
        public CLIServerStart()
            : base()
        {
            description = "This command starts the CLIServer.";
        }

        public override string Execute()
        {
            try
            {
                //Handler.components.cliServer.Start();
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
            : base()
        {
            description = "This command stops the CLIServer.";
        }

        public override string Execute()
        {
            try
            {
                //Handler.components.cliServer.Stop();
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
