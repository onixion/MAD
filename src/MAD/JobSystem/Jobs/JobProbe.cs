using System;
using System.Collections.Generic;
using System.Net;

namespace MAD.JobSystemCore
{
    public class JobProbe : Job
    {
        public int port { get; set; }
        public string aesPass { get; set; }

        public JobProbe()
            : base(JobType.Probe, Protocol.TCP)
        {

        }

        public override void Execute(IPAddress targetAddress)
        {
            // connect to probe and fetch data
        }
    }
}
