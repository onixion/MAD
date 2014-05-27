using System;
using System.Net.NetworkInformation;
using System.Text;

namespace MAD
{
    class JobPing : Job
    {
        #region members

        private JobPingOptions pingJobOptions;
        private Ping ping;
        private PingOptions pingOptions;
        private PingReply reply;

        #endregion

        public JobPing(JobOptions jobOptions, JobPingOptions pingJobOptions)
        {
            InitJob(jobOptions);
            this.pingJobOptions = pingJobOptions;

            ping = new Ping();
            pingOptions = new PingOptions(pingJobOptions.ttl, true);
        }

        #region methodes

        public override void DoJob()
        {
            reply = ping.Send(pingJobOptions.targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), pingOptions);

            if (reply.Status == IPStatus.Success)
            {
                jobOutput = "True";
            }
            else
            {
                jobOutput = "False";
            }
        }

        #endregion
    }
}
