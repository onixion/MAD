using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobPort : Job
    {
        #region members

        public int port { get; set; }

        #endregion

        #region constructors

        public JobPort()
            : base("NULL", JobType.PortScan, new JobTime(), new JobOutput(), new JobNotification())
        {
            this.port = 80;
        }

        public JobPort(string jobName, JobType jobType, JobTime jobTime, JobOutput outp, JobNotification noti, int port)
            : base(jobName, jobType, jobTime, outp, noti)
        {
            this.port = port;
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TARGET-PORT: <color><white>" + port.ToString() +"\n";

            return _temp;
        }

        #endregion
    }
}


