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

        private Ping _ping;
        private PingOptions _pingOptions;
        private PingReply _reply;

        #endregion

        public JobPing(JobOptions jobOptions, IPAddress targetAddress, int ttl)
        {
            InitJob(jobOptions);

            this.targetAddress = targetAddress;
            this.ttl = ttl;

            _ping = new Ping();
            _pingOptions = new PingOptions(ttl, true);
        }

        #region methodes

        public override void DoJob()
        {
            try
            {
                _reply = _ping.Send(targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), _pingOptions);

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
