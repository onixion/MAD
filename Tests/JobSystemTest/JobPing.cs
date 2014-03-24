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
        int ttl;
        
        public JobPing(JobOptions options, int ttl)
        {
            InitJob();
            this.options = options;
            this.ttl = ttl;

            ping = new Ping();
            pingOptions = new PingOptions(ttl, true);
        }

        public override void DoJob()
        {
            Console.Write("JobID: " + jobID + " PING Erfolgreich? -> ");

            reply = ping.Send(options.targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), pingOptions);

            if (reply.Status == IPStatus.Success)
               options.jobSuccsess = true;
            else
               options.jobSuccsess = false;

            Console.WriteLine(success);
        }

        public override void JobStatus()
        {
            base.JobStatus();

            Console.WriteLine("TTL: " + options.ttl);
        }
        
        
        
    }
}
