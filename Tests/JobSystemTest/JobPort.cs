using System;
using System.Net.Sockets;
using System.Net;

namespace JobSystemTest
{
    class JobPort : Job
    {
        Socket socket;

        public JobPort(JobOptions options)
        {
            this.options = options;
            InitJob();
        }

        public override void DoJob()
        {
            while (true)
            {
                Console.Write("JobID: " + jobID + " PORT-SCAN on " + options.targetPort + " -> ");

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    socket.Connect(new IPEndPoint(options.targetAddress, options.targetPort));
                    success = true;
                }
                catch (Exception e)
                {
                    success = false;
                }

                socket.Close();

                Console.WriteLine(success);

                Wait(options.delayTime);
            }
        }
    }
}
