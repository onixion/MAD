using System;
using System.Threading;
using System.Net.NetworkInformation;
using System.Text;

namespace JobSystemTest
{
    class JobPing : Job
    {
        // JobPing spezific variables
        PingOptions pingOptions;
        Ping ping;
        PingReply reply;

        public JobPing(JobOptions options)
        {
            this.options = options;
            InitJob();

            // PING SHIT
            pingOptions = new PingOptions(200, true);
            ping = new Ping();
            reply = ping.Send(options.target, 5000, Encoding.ASCII.GetBytes("1111111111111111"), pingOptions);
            // PING SHIT
        }

        public override void DoJob()
        {
            while (true)
            {
                Console.Write("JobID: " + jobID + " PING -> ");

                if (reply.Status == IPStatus.Success)
                    Console.Write("Erfolgreicher Ping! :)");
                else
                    Console.Write("Nicht erfolgreicher Ping! :(");

                Console.WriteLine();

                // wait delayTime
                Thread.Sleep(options.delayTime);
            }
        }
    }
}
