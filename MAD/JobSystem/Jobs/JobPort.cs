using System;
using System.Net;
using System.Net.Sockets;

namespace MAD.JobSystem
{
    public class JobPort : Job
    {
        #region members

        public IPAddress targetAddress { get; set; }
        public int port { get; set; }

        #endregion

        #region constructors

        public JobPort()
            : base(new JobOptions("NULL", new JobTime(), JobOptions.JobType.PortRequest))
        {
            this.targetAddress = IPAddress.Loopback;
            this.port = 80;
        }

        public JobPort(JobOptions jobOptions, IPAddress targetAddress, int port)
            : base(jobOptions)
        {
            this.targetAddress = targetAddress;
            this.port = port;
        }

        #endregion

        #region methodes

        public override void DoJob()
        {
            Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(new IPEndPoint(targetAddress, port));
                jobOutput.jobState = JobOutput.State.Success;
            }
            catch (Exception)
            {
                jobOutput.jobState = JobOutput.State.Failed;
            }

            _socket.Close();
        }

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TARGET-IP: <color><white>" + targetAddress.ToString() + "\n";
            _temp += "<color><yellow>TARGET-PORT: <color><white>" + port.ToString() +"\n";

            return _temp;
        }

        #endregion
    }
}


