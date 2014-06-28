using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace MAD.jobSys
{
    class JobServiceCheck : Job
    {
        #region members

		public string argument;
        private bool working; 

        #endregion

        #region constructors

        public JobServiceCheck()
            : base("NULL", JobType.ServiceCheck, new JobTime())
        {
            this.argument = "";
        }

        public JobServiceCheck(string jobName, JobType jobType, JobTime jobTime, string argument)
            : base(jobName, jobType, jobTime)
        {
            this.argument = argument;
        }

        #endregion

        #region methods

		#region extern

		public override void Execute ()
		{
			switch (argument) 
			{
			case "dns":
				dnsCheck ();
				break;
			}
		}

        protected override string JobStatus()
        {
            string _tmp = "";

            if (working)
            {
                _tmp += "Requestet service is working";
                outState = OutState.Success;
            }
            else
            {
                _tmp += "Requestet service seems to be dead";
                outState = OutState.Failed;
            }

            return (_tmp);
        }

		#endregion

		#region intern

		private void dnsCheck()
		{
            try
            {
                IPHostEntry _tmp = Dns.GetHostEntry("www.google.at");
                working = true;
            }
            catch (Exception)
            {
                working = false;
            }
		}

		#endregion 

		#endregion
    }
}
