using System;
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
