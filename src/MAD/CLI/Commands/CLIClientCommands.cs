using System;

namespace MAD.CLICore
{
    public class ClientExitCommand : Command
    {
        public ClientExitCommand()
            : base()
        {
            description = "Exit client and disconnect from server.";
        }

        public override string Execute(int consoleWidth)
        {
            return "SERVER_DISCONNECT";
        }
    }
}
