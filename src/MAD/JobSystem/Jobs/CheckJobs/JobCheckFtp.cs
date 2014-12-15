using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using MAD.Logging;

namespace MAD.JobSystemCore
{
    class JobCheckFtp : Job
    {
        public string username; //required parameter
        public string password; //required parameter

        public JobCheckFtp()
            : base(JobType.ServiceCheck)
        {
            outp.outputs.Add(new OutputDescriptor("Additional Information", typeof(string), false));
        }

        public override void Execute(IPAddress targetAddress)
        {
            FtpWebRequest _requestDir = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + targetAddress.ToString()));
            _requestDir.Credentials = new NetworkCredential(username, password);
            _requestDir.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

            try
            {
                FtpWebResponse _response = (FtpWebResponse)_requestDir.GetResponse();
                Logger.Log("FTP Service seems to work", Logger.MessageType.INFORM);
                outp.outState = JobOutput.OutState.Success;
                outp.GetOutputDesc("Additional Information").dataObject = _response.StatusDescription;
            }

            catch (Exception ex)
            {
                Logger.Log("FTP Service seems to be dead", Logger.MessageType.ERROR);
                outp.outState = JobOutput.OutState.Exception;
                outp.GetOutputDesc("Additional Information").dataObject = ex.Data.ToString();
            }
        }
    }
}
