using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace MAD.JobSystem
{
    public class JobPing : Job
    {
        #region members

        public IPAddress targetAddress;
        public int ttl;
        public bool dontFragment;

        private Ping _ping = new Ping();

        #endregion

        public JobPing()
            : base(new JobOptions("NULL", new JobTime(), JobOptions.JobType.PingRequest))
        {
            this.targetAddress = IPAddress.Loopback;
            this.ttl = 250;
            this.dontFragment = true;
        }

        public JobPing(JobOptions jobOptions, IPAddress targetAddress, int ttl)
            : base (jobOptions)
        {
            this.targetAddress = targetAddress;
            this.ttl = ttl;
        }

        #region methodes

        public override void DoJob()
        {
            PingOptions _pingOptions = new PingOptions(ttl, dontFragment);

            try
            {
                PingReply _reply = _ping.Send(targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), _pingOptions);

                if (_reply.Status == IPStatus.Success)
                {
                    jobOutput.jobState = JobOutput.State.Success;
                }
                else
                {
                    jobOutput.jobState = JobOutput.State.Failed;
                }
            }
            catch (Exception)
            {
                jobOutput.jobState = JobOutput.State.Exception;
            }
        }

        public override string Status()
        {
            string _temp = base.Status();

            _temp += "<color><yellow>TARGET-IP: <color><white>" + targetAddress.ToString() + "\n";
            _temp += "<color><yellow>TTL: <color><white>" + ttl.ToString() + "\n";

            return _temp;
        }

        #endregion
    }
}
