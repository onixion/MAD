﻿using System;
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
        [JsonIgnore]
        public bool dontFragment = true;

        public int ttl { get; set; }
        public int timeout { get; set; }

        #endregion

        #region constructors

        public JobPing()
            : base(JobType.Ping)
        {
            ttl = 200;
            timeout = 5000;

            outp.outputs.Add(new OutputDescriptor("IPStatus", typeof(string)));
            outp.outputs.Add(new OutputDescriptor("TTL", typeof(int)));
            outp.outputs.Add(new OutputDescriptor("RoundtripTime", typeof(long)));
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            PingOptions _pingOptions = new PingOptions(ttl, dontFragment);
            PingReply _reply = _ping.Send(targetAddress, timeout, Encoding.ASCII.GetBytes("1111111111111111"), _pingOptions);

            outp.GetOutputDesc("IPStatus").dataObject = _reply.Status.ToString();
            outp.GetOutputDesc("TTL").dataObject = _reply.Options.Ttl;
            outp.GetOutputDesc("RoundtripTime").dataObject = _reply.RoundtripTime;

            if (_reply.Status == IPStatus.Success)
                outp.outState = JobOutput.OutState.Success;
            else
                outp.outState = JobOutput.OutState.Failed;
        }

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TTL: <color><white>" + ttl.ToString() + "\n";
            _temp += "<color><yellow>Timeout: <color><white>" + timeout.ToString() + "\n";

            return _temp;
        }

        #endregion
    }
}
