using System;
using System.Net.NetworkInformation;
using System.Text;

namespace MAD
{
    class JobPing : Job
    {
        public JobPingOptions pingJobOptions;
        private Ping ping;
        private PingOptions pingOptions;
        private PingReply reply;
        
        public JobPing(JobPingOptions pingJobOptions)
        {
            InitJob();
            this.pingJobOptions = pingJobOptions;
            this.jobOptions = (JobOptions)pingJobOptions;

            ping = new Ping();
            pingOptions = new PingOptions(pingJobOptions.ttl, true);
        }

        public override void DoJob()
        {
            reply = ping.Send(jobOptions.targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), pingOptions);

            if (reply.Status == IPStatus.Success)
                jobOutput = "True";
            else
                jobOutput = "False";
        }

        public override string JobStatus()
        {
            string temp = base.JobStatus();

            temp += "TTL:       " + pingJobOptions.ttl + "\n";

            return temp;
        }
        
        
        
    }
}
