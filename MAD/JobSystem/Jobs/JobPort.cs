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
            : base("NULL", JobType.PortScan, new JobTime())
        {
            this.port = 80;
        }

        public JobPort(string jobName, JobType jobType, JobTime jobTime, int port)
            : base(jobName, jobType, jobTime)
        {
            this.port = port;
        }

        public JobPort(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.port = (int)info.GetValue("SER_JOB_PORTSCAN_PORT", typeof(int));
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(new IPEndPoint(targetAddress, port));
                outState = OutState.Success;
            }
            catch (Exception)
            {
                outState = OutState.Failed;
            }

            _socket.Close();
        }

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TARGET-PORT: <color><white>" + port.ToString() +"\n";

            return _temp;
        }

        #region for serialization

        public override void GetObjectDataJobSpecific(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_JOB_PORTSCAN_PORT", this.port);
        }

        #endregion

        #endregion
    }
}


