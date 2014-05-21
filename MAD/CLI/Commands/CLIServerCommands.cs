using System;

namespace MAD.CLI
{
    class CLIServerStart : Command
    {
        public CLIServerStart()
        {

        }

        public override string Execute()
        {
            MadComponents.components.cliServer.Start();
            output += "CLI server started.";
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
            MadComponents.components.cliServer.Stop();
            output += "CLI server stopped.";
            return output;
        }
    }
}
