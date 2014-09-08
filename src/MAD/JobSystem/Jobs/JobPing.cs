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
            outp.outputs.Add(new OutputDescriptor("TestString", typeof(string)));
            outp.outputs.Add(new OutputDescriptor("TestInt", typeof(int)));
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            PingOptions _pingOptions = new PingOptions(ttl, dontFragment);
            PingReply _reply = _ping.Send(targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), _pingOptions);

            outp.GetOutputDesc("TestString").dataObject = "test";
            outp.GetOutputDesc("TestInt").dataObject = 10;

            if (_reply.Status == IPStatus.Success)
                outp.outState = JobOutput.OutState.Success;
            else
                outp.outState = JobOutput.OutState.Failed;
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
