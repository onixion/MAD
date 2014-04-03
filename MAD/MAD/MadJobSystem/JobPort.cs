using System;
using System.Net.Sockets;
using System.Net;

namespace MAD
{
    class JobPort : Job
    {
        public JobPortOptions portJobOptions;
        private Socket socket;

        public JobPort(JobPortOptions portJobOptions)
        {
            InitJob();
            this.portJobOptions = portJobOptions;
            this.jobOptions = (JobOptions) portJobOptions;
        }

        public override void DoJob()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                socket.Connect(new IPEndPoint(jobOptions.targetAddress, portJobOptions.port));
                jobOutput = "True";
            }
            catch (Exception e)
            {
                jobOutput = "False";
            }

            socket.Close();
        }

        public override void JobStatus()
        {
            base.JobStatus();
            Console.WriteLine(string.Format("PORT:     \t{0}", portJobOptions.port));
        }
    }
}


