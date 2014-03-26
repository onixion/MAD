using System;
using System.Net.NetworkInformation;
using System.Text;

namespace MAD
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
            this.jobOptions = jobOptions;
            this.ttl = ttl;

            ping = new Ping();
            pingOptions = new PingOptions(ttl, true);
        }

        public override void DoJob()
        {
            jobOutput = new string[1];

            Console.Write("JobID: " + jobID + " PING Erfolgreich? -> ");

            reply = ping.Send(jobOptions.targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), pingOptions);

            if (reply.Status == IPStatus.Success)
                jobOutput[0] = "TRUE";
            else
                jobOutput[0] = "FALSE";
        }

        public override void JobStatus()
        {
            base.JobStatus();
        }
        
        
        
    }
}
