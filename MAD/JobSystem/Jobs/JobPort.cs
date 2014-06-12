using System;
using System.Net;
using System.Net.Sockets;

namespace MAD.JobSystem
{
    public class JobPort : Job
    {
        #region members

        public IPAddress targetAddress;
        public int port;

        private Socket _socket;

        #endregion

        #region constructors

        public JobPort()
        {
            InitJob(JobDefaultValues.defaultValues.defaultJobOptions);
            this.jobOptions.jobType = JobOptions.JobType.PortRequest;
            this.targetAddress = JobDefaultValues.defaultValues.defaultTargetAddress;
            this.port = JobDefaultValues.defaultValues.defaultPort;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public JobPort(JobOptions jobOptions, IPAddress targetAddress, int port)
        {
            InitJob(jobOptions);

            this.targetAddress = targetAddress;
            this.port = port;

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        #endregion

        #region methodes

        public override void DoJob()
        {
            try
            {
                _socket.Connect(new IPEndPoint(targetAddress, port));
                jobOutput.jobState = JobOutput.State.Success;
            }
            catch (Exception)
            {
                jobOutput.jobState = JobOutput.State.Failed;
            }
        }

        public override string Status()
        {
            string _temp = base.Status();

            _temp += "<color><yellow>TARGET-IP: <color><white>" + targetAddress.ToString() + "\n";
            _temp += "<color><yellow>TARGET-PORT: <color><white>" + port.ToString() +"\n";

            return _temp;
        }

        #endregion
    }
}


