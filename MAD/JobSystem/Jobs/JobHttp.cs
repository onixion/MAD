using System;
using System.Net;

namespace MAD.jobSys
{
    public class JobHttp : Job
    {
        #region members

        public IPAddress targetAddress;
        public int port;

        private WebRequest _request;
        private WebResponse _response;

        #endregion

        #region constructors

        public JobHttp() 
            : base(new JobOptions("NULL", new JobTime(), JobOptions.JobType.HttpRequest))
        {
            this.targetAddress = IPAddress.Loopback;
            this.port = 80;
        }

        public JobHttp(JobOptions jobOptions, IPAddress targetAddress, int port) 
            : base (jobOptions)
        {
            this.targetAddress = targetAddress;
            this.port = port;
        }

        #endregion

        #region methodes

        public override void DoJob()
        {
            try
            {
                _request = WebRequest.Create("http://" + targetAddress.ToString() + ":" + port);

                try
                {
                    _response = _request.GetResponse();
                    jobOutput.jobState = JobOutput.State.Success;

                    _response.Close();
                }
                catch (Exception)
                {
                    jobOutput.jobState = JobOutput.State.Failed;
                }

                _request.Abort();
            }
            catch (Exception)
            {
                jobOutput.jobState = JobOutput.State.Exception;
            }
        }

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TARGET-IP: <color><white>" + targetAddress.ToString() + "\n";
            _temp += "<color><yellow>TARGET-PORT: <color><white>" + port.ToString() + "\n";

            return _temp;
        }

        #endregion
    }
}