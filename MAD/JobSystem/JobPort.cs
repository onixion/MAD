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

        public JobPort(JobOptions jobOptions, JobPortOptions portJobOptions)
        {
            InitJob(jobOptions);
            this.portJobOptions = portJobOptions;
        }

        #region methodes

        public override void DoJob()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(new IPEndPoint(portJobOptions.targetAddress, portJobOptions.port));
                jobOutput = "True";
            }
            catch (Exception)
            {
                jobOutput = "False";
            }

            socket.Close();
        }

        #endregion
    }
}


