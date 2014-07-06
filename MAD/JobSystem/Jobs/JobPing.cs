using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace MAD.JobSystemCore
{
    public class JobPing : Job
    {
        #region members

        public IPAddress targetAddress;
        public int ttl;
        public bool dontFragment;

        private Ping _ping = new Ping();

        #endregion

        #region constructor

        public JobPing()
            : base("NULL", JobType.Ping, new JobTime())
        {
            this.targetAddress = IPAddress.Loopback;
            this.ttl = 250;
            this.dontFragment = true;
        }

        public JobPing(string jobName, JobType jobType, JobTime jobTime, IPAddress targetAddress, int ttl)
            : base (jobName, jobType, jobTime)
        {
            this.targetAddress = targetAddress;
            this.ttl = ttl;
            this.dontFragment = true;
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            PingOptions _pingOptions = new PingOptions(ttl, dontFragment);

            try
            {
                PingReply _reply = _ping.Send(targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), _pingOptions);

                if (_reply.Status == IPStatus.Success)
                {
                    outState = OutState.Success;
                }
                else
                {
                    outState = OutState.Failed;
                }
            }
            catch (Exception)
            {
                outState = OutState.Exception;
            }
        }

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TARGET-IP: <color><white>" + targetAddress.ToString() + "\n";
            _temp += "<color><yellow>TTL: <color><white>" + ttl.ToString() + "\n";

            return _temp;
        }

        #endregion
    }
}
