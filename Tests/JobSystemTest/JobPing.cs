using System;
using System.Net.NetworkInformation;
using System.Text;

namespace JobSystemTest
{
    class JobPing : Job
    {
        Ping ping;
        PingOptions pingOptions;
        PingReply reply;

        public JobPing(JobOptions options)
        {
            this.options = options;
            InitJob();   

            ping = new Ping();
            pingOptions = new PingOptions(200, true);
            reply = ping.Send(options.targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), pingOptions);
        }

        public override void DoJob()
        {
            while (true)
            {
                Console.Write("JobID: " + jobID + " PING Erfolgreich? -> ");

                if (reply.Status == IPStatus.Success)
                    success = true;
                else
                    success = false;

                Console.WriteLine(success);

                Wait(options.delayTime);
            }
        }
    }
}
