﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using MAD.Logging;

namespace MAD.JobSystemCore
{
    class JobCheckDns : Job
    {
        private string _hostname = MadConf.conf.defaultHostname; 

        //keine Parameter benötigt 
        public JobCheckDns(string hostname)
            : base(JobType.ServiceCheck, Protocol.UDP)
        {
            _hostname = hostname;
        }

        public JobCheckDns()
            : base(JobType.ServiceCheck, Protocol.UDP)
        { }

        public override void Execute(System.Net.IPAddress targetAddress)
        {
            try
            {
                IPHostEntry _tmp = Dns.GetHostEntry(_hostname);
                Logger.Log("DNS Service seems to work", Logger.MessageType.INFORM);
                outp.outState = JobOutput.OutState.Success;
            }
            catch (Exception)
            {
                Logger.Log("DNS Service seems to be dead", Logger.MessageType.ERROR);
                outp.outState = JobOutput.OutState.Failed;
            }
        }
    }
}
