using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobPing : Job
    {
        #region members

        [JsonIgnore]
        private Ping _ping = new Ping();
        public int ttl { get; set; }
        [JsonIgnore]
        public bool dontFragment = true;

        #endregion

        #region constructors

        public JobPing()
            : base(JobType.Ping)
        {
            ttl = 200;
        }

        public JobPing(string name)
            : base(name, JobType.Ping) 
        {
            ttl = 200;
        }

        public JobPing(string name, JobType type, int ttl)
            : base (name, type)
        {
            this.ttl = ttl;
        }

        public JobPing(string name, JobType type, JobTime time, JobNotification noti, int ttl)
            : base(name, type, time, noti)
        {
            this.ttl = ttl;
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
                    outState = OutState.Success;
                else
                    outState = OutState.Failed;
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

        #endregion
    }
}
