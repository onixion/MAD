using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MAD.JobSystemCore
{
    class JobServiceCheck : Job
    {
        #region members

		public string argument;
        public string username;
        public string password;
        public IPAddress targetIP;
        
        private bool working; 

        #endregion

        #region constructors

        public JobServiceCheck()
            : base("NULL", JobType.ServiceCheck, new JobTime())
        {
            this.argument = "";
            this.username = "";
            this.password = "";
            this.targetIP = IPAddress.Parse("127.0.0.1");
        }

        public JobServiceCheck(string jobName, JobType jobType, JobTime jobTime, string argument, IPAddress targetIP, string username, string password)
            : base(jobName, jobType, jobTime)
        {
            this.argument = argument;
            this.username = username;
            this.password = password;
            this.targetIP = targetIP;
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
            case "ftp":
                ftpCheck();
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
                IPHostEntry _tmp = Dns.GetHostEntry("www.google.com");
                working = true;
            }
            catch (Exception)
            {
                working = false;
            }
		}

        private void ftpCheck()
        {
            FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://"+targetIP.ToString()));
            requestDir.Credentials = new NetworkCredential(username, password);
            requestDir.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

            try
            {
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
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
