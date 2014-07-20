using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobServiceCheck : Job
    {
        #region members

		public string arg;
        public string username;
        public string password;
        
        private bool working; 

        #endregion

        #region constructors

        public JobServiceCheck()
            : base("NULL", JobType.ServiceCheck, new JobTime())
        {
            this.arg = "";
            this.username = "";
            this.password = "";
            //this.targetIP = IPAddress.Parse("127.0.0.1");
        }

        public JobServiceCheck(string jobName, JobType jobType, JobTime jobTime, string arg, string username, string password)
            : base(jobName, jobType, jobTime)
        {
            this.arg = arg;
            this.username = username;
            this.password = password;
        }

        // for serialization
        public JobServiceCheck(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.arg = (string)info.GetValue("SER_JOB_SERVICECHECK_ARG", typeof(string));
            this.username = (string)info.GetValue("SER_JOB_SERVICECHECK_USERNAME", typeof(string));
            this.password = (string)info.GetValue("SER_JOB_SERVICECHECK_PASSWORD", typeof(string));
        }

        #endregion

        #region methods

		#region extern

		public override void Execute(IPAddress targetAddress)
		{
			switch (arg) 
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
            // You need to pass ftpCheck the IP-Address to work.

            /*
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
             * */
        }
		#endregion 

        #region for serialization

        public override void GetObjectDataJobSpecific(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("SER_JOB_SERVICECHECK_ARG", this.arg);
            info.AddValue("SER_JOB_SERVICECHECK_USERNAME", this.username);
            info.AddValue("SER_JOB_SERVICECHECK_PASSWORD", this.password);
        }

        #endregion

        #endregion
    }
}
