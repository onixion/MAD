using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobPing : Job
    {
        #region members

        public int ttl;
        public bool dontFragment = true;

        private Ping _ping = new Ping();

        #endregion

        #region constructor

        public JobPing()
            : base("NULL", JobType.Ping, new JobTime())
        {
            this.ttl = 250;
        }

        public JobPing(string jobName, JobType jobType, JobTime jobTime, int ttl)
            : base (jobName, jobType, jobTime)
        {
            this.ttl = ttl;
        }

        public JobPing(SerializationInfo info, StreamingContext context)
            : base (info, context)
        {
            this.ttl = (int)info.GetValue("SER_JOB_PING_TTL", typeof(int));
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

            _temp += "<color><yellow>TTL: <color><white>" + ttl.ToString() + "\n";

            return _temp;
        }

        #region for serialization

        public override void GetObjectDataJobSpecific(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_JOB_PING_TTL", this.ttl);
        }

        #endregion

        #endregion
    }
}
