using System;
using System.Net;
using System.Net.NetworkInformation;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobPing : Job
    {
        #region members

        [JsonIgnore]
        private Ping _ping = new Ping();
        [JsonIgnore]
        public bool dontFragment = true;

        public int ttl { get; set; }
        public int timeout { get; set; }

        #endregion

        #region constructors

        public JobPing()
            : base(JobType.Ping, Protocol.ICMP)
        {
            ttl = 200;
            timeout = 500;

            outp.outputs.Add(new OutputDescriptor("IPStatus", typeof(string), false));
            //outp.outputs.Add(new OutputDescriptor("TTL", typeof(int)));
            // Useless, does not work ...
        }

        #endregion

        #region methods

        public override void Execute(IPAddress targetAddress)
        {
            PingOptions _pingOptions = new PingOptions(ttl, dontFragment);
            PingReply _reply = _ping.Send(targetAddress, timeout, new byte[8] {1,2,3,4,5,6,7,8} , _pingOptions);

            outp.GetOutputDesc("IPStatus").dataObject = _reply.Status.ToString();

            if (_reply.Status == IPStatus.Success)
                outp.outState = JobOutput.OutState.Success;
            else
                outp.outState = JobOutput.OutState.Failed;
        }

        #endregion
    }
}
