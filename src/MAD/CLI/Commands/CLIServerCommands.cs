using System;

using MAD.CLIServerCore;

namespace MAD.CLICore
{
    /* These commands are not necessarry anymore, because the cliserver can't run at the same time as the cli... 
     * But we keep it, maybe we need it some day ...
     */

    public class CLIServerInfo : Command
    {
        private CLIServer _cliServer;

        public CLIServerInfo(object[] args)
            : base()
        {
            _cliServer = (CLIServer)args[0];
            description = "This command shows informations about the CLIServer.";
        }

        public override string Execute(int consoleWidth)
        {
            output += "<color><white>Listening: " + _cliServer.IsListening + "\n";
            output += "Port: " + _cliServer.serverPort + "\n";

            return output;
        }
    }

    public class CLIServerStart : Command
    {
        private CLIServer _cliServer;

        public CLIServerStart(object[] args)
            : base()
        {
            _cliServer = (CLIServer)args[0];
            description = "This command starts the CLIServer.";
        }

        public override string Execute(int consoleWidth)
        {
            try
            {
                _cliServer.Start();
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
        private CLIServer _cliServer;

        public CLIServerStop(object[] args)
            : base()
        {
            _cliServer = (CLIServer) args[0];
            description = "This command stops the CLIServer.";
        }

        public override string Execute(int consoleWidth)
        {
            try
            {
                _cliServer.Stop();
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
