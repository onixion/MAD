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

        public int ttl = 200;
        public bool dontFragment = true;

        private Ping _ping = new Ping();

        #endregion

        #region constructor

        public JobPing(string name)
            : base(name, JobType.Ping) 
        {
            this.outDesc.Add(new OutputDesc("DeltaTime", typeof(string)));
        }

        public JobPing(string name, JobType type, int ttl)
            : base (name, type)
        {
            this.ttl = ttl;
            this.outDesc.Add(new OutputDesc("DeltaTime", typeof(string)));
        }

        public JobPing(string name, JobType type, JobTime time, JobNotification noti, int ttl)
            : base(name, type, time, noti)
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
                    SetOutput("DeltaTime", _reply.RoundtripTime.ToString("ssss"));
                }
                else
                {
                    outState = OutState.Failed;
                    SetOutput("DeltaTime", _reply.RoundtripTime.ToString("----"));
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
