using System;

namespace MAD.cli
{
    public class CLIServerInfo : Command
    {
        private CLIServer _cliServer;

        public CLIServerInfo(object[] args)
            : base()
        {
            _cliServer = (CLIServer)args[0];
            description = "This command shows informations about the CLIServer.";
        }

        public override string Execute()
        {
            output += "<color><yellow>CLI-Server v" + _cliServer.version + "\n";
            output += "Server listening: " + _cliServer.listening + "\n";

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

        public override string Execute()
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

        public override string Execute()
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
