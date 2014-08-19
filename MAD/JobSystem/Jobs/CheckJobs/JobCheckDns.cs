using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MAD.JobSystemCore
{
    class JobCheckDns : Job
    {
        private bool working;

        //keine Parameter benötigt 

        protected override string JobStatus()
        {
            string _tmp = "";

            if (working)
            {
                _tmp += "DNS is working";
                outState = OutState.Success;
            }
            else
            {
                _tmp += "DNS seems to be dead";
                outState = OutState.Failed;
            }

            return (_tmp);
        }

        public override void Execute(System.Net.IPAddress targetAddress)
        {
            try
            {
                IPHostEntry _tmp = Dns.GetHostEntry("www.google.com");
                working = true;
            }
            catch (Exception)
            {
                working = false;
            }
        }
    }
}
