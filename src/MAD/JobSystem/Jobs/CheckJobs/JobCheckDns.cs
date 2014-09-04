﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MAD.JobSystemCore
{
    class JobCheckDns : Job
    {
        private bool _working;

        //keine Parameter benötigt 

        public JobCheckDns()
            : base(JobType.ServiceCheck)
        { }


        protected override string JobStatus()
        {
            string _tmp = "";

            if (_working)
            {
                _tmp += "DNS is working";
                outp.outState = JobOutput.OutState.Success;
            }
            else
            {
                _tmp += "DNS seems to be dead";
                outp.outState = JobOutput.OutState.Failed;
            }

            return (_tmp);
        }

        public override void Execute(System.Net.IPAddress targetAddress)
        {
            try
            {
                IPHostEntry _tmp = Dns.GetHostEntry("www.google.com");
                _working = true;
            }
            catch (Exception)
            {
                _working = false;
            }
        }
    }
}
