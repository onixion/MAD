using System;

using MAD.CLIServerCore;

namespace MAD.CLICore
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

    public class CLIChangePort : Command
    {
        private CLIServer _cliServer;

        public CLIChangePort(object[] args)
            : base()
        {
            _cliServer = (CLIServer)args[0];

            requiredParameter.Add(new ParameterOption("p", "<PORT>", "The specific port.", false, false, new Type[] { typeof(int) }));

            description = "This command changes the port on which the server listens.";
        }

        public override string Execute()
        {
            try
            {
                int _port = (int)parameters.GetParameter("p").argumentValue[0];

                _cliServer.ChangePort(_port);

                return "<color><green>Port changed to '" + _port + "'.";
            }
            catch (Exception)
            { 
                return "<color><red>Could not change server port, because server is running!";
            }
        }
    }
}
