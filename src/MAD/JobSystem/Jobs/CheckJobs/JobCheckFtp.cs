using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MAD.JobSystemCore
{
    class JobCheckFtp : Job
    {
        private bool _working;

        public string username; //required parameter
        public string password; //required parameter

        public JobCheckFtp()
            : base(JobType.ServiceCheck)
        { }

        public override void Execute(IPAddress targetAddress)
        {
            FtpWebRequest _requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + targetAddress.ToString()));
            _requestDir.Credentials = new NetworkCredential(username, password);
            _requestDir.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

            try
            {
                FtpWebResponse _response = (FtpWebResponse)_requestDir.GetResponse();
                _working = true;
            }

            catch (Exception)
            {
                _working = false;
            }
        }
    }
}
