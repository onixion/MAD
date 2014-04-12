using System;
using System.Net.Sockets;

namespace MAD
{
    public class CLIServer : TcpServer
    {
        public CLIServer(int port)
        {
            Init(port);
        }

        public override void HandleClient(TcpClient client)
        {
            // DO STUFF
        }
    }
}
