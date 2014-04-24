using System;
using System.Net.Sockets;
using System.Net;

namespace MAD
{
    class JobPort : Job
    {
        #region members

        public JobPortOptions portJobOptions;
        private Socket socket;

        #endregion

        public JobPort(JobPortOptions portJobOptions)
        {
            InitJob();

            this.portJobOptions = portJobOptions;
            this.jobOptions = (JobOptions) portJobOptions;
        }

        #region methodes

        public override void DoJob()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(new IPEndPoint(jobOptions.targetAddress, portJobOptions.port));
                jobOutput = "True";
            }
            catch (Exception)
            {
                jobOutput = "False";
            }

            socket.Close();
        }

        public override string JobStatus()
        {
            string buffer = base.JobStatus();

            buffer += "PORT:      " + portJobOptions.port + "\n";

            return buffer;
        }

        #endregion
    }
}


