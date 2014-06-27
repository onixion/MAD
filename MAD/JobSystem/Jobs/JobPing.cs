using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace MAD.jobSys
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
            : base(new JobOptions("NULL", new JobTime(), JobOptions.JobType.Ping))
        {
            this.targetAddress = IPAddress.Loopback;
            this.ttl = 250;
            this.dontFragment = true;

            InitOutDescriptors();
        }

        public JobPing(JobOptions jobOptions, IPAddress targetAddress, int ttl)
            : base (jobOptions)
        {
            this.targetAddress = targetAddress;
            this.ttl = ttl;
            this.dontFragment = true;

            InitOutDescriptors();
        }

        #region methodes

        private void InitOutDescriptors()
        {
            outDescriptors.Add(new OutDescriptor("TTL-Left", typeof(int), null));
        }

        public override void Execute()
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
