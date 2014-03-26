using System;
using System.Net.Sockets;
using System.Net;

namespace MAD
{
    class JobPort : Job
    {
        Socket socket;

        public JobPort(/*JobOptions options*/)
        {
            //this.options = options;
            //Init();
        }

        public override void DoJob()
        {
            /*Console.Write("JobID: " + jobID + " PORT-SCAN on " + options.targetPort + " -> ");

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

            Console.WriteLine(success);*/
        }
    }
}


