﻿using System;
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
            : base(JobType.Ping)
        {
            ttl = 200;
            timeout = 5000;

            outp.outputs.Add(new OutputDescriptor("IPStatus", typeof(string)));
            outp.outputs.Add(new OutputDescriptor("TTL", typeof(int)));
        }

        #endregion

        #region methods

        public override void Execute(IPAddress targetAddress)
        {
            PingOptions _pingOptions = new PingOptions(ttl, dontFragment);
            PingReply _reply = _ping.Send(targetAddress, timeout, new byte[8] {1,2,3,4,5,6,7,8} , _pingOptions);

            outp.GetOutputDesc("IPStatus").dataObject = _reply.Status.ToString();

            if (_reply.Options != null)
                outp.GetOutputDesc("TTL").dataObject = _reply.Options.Ttl;
            else
                outp.GetOutputDesc("TTL").dataObject = null;

            if (_reply.Status == IPStatus.Success)
                outp.outState = JobOutput.OutState.Success;
            else
                outp.outState = JobOutput.OutState.Failed;
        }

        #endregion
    }
}
