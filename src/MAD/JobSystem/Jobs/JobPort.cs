using System;
using System.Net;
using System.Net.Sockets;

namespace MAD.JobSystemCore
{
    public class JobPort : Job
    {
        #region members

        public int port { get; set; }
        public int timeout { get; set; }

        #endregion

        #region constructors

        public JobPort()
            : base("NULL", JobType.PortScan, new JobTime(), new JobOutput())
        {
            port = 80;
            timeout = 500;
        }

        #endregion

        #region methods

        public override void Execute(IPAddress targetAddress)
        {
            Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SendTimeout = timeout;
            _socket.ReceiveTimeout = timeout;

            try
            {
                _socket.Connect(new IPEndPoint(targetAddress, port));
                outp.outState = JobOutput.OutState.Success;
            }
            catch (Exception)
            {
                outp.outState = JobOutput.OutState.Failed;
            }
            _socket.Close();
        }

        #endregion
    }
}


