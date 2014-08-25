﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MAD.JobSystemCore
{
    class JobCheckFtp : Job
    {
        private bool working;

        public string username; //required parameter
        public string password; //required parameter

        protected override string JobStatus()
        {
            string _tmp = "";

            if (working)
            {
                _tmp += "FTP is working";
                outState = OutState.Success;
            }
            else
            {
                _tmp += "FTP seems to be dead";
                outState = OutState.Failed;
            }

            return (_tmp);
        }

        public override void Execute(IPAddress targetAddress)
        {
            FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + targetAddress.ToString()));
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
    }
}
