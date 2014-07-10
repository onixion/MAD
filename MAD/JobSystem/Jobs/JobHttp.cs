using System;
using System.Net;

namespace MAD.JobSystemCore
{
    public class JobHttp : Job
    {
        #region members

        public int port;

        private WebRequest _request;
        private WebResponse _response;

        #endregion

        #region constructors

        public JobHttp() 
            : base("NULL", JobType.Http, new JobTime())
        {
            this.port = 80;
        }

        public JobHttp(string jobName, JobType jobType, JobTime jobTime, int port) 
            : base (jobName, jobType, jobTime)
        {
            this.port = port;
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            try
            {
                _request = WebRequest.Create("http://" + targetAddress.ToString() + ":" + port);

                try
                {
                    _response = _request.GetResponse();
                    outState = OutState.Success;

                    _response.Close();
                }
                catch (Exception)
                {
                    outState = OutState.Failed;
                }

                _request.Abort();
            }
            catch (Exception)
            {
                outState = OutState.Exception;
            }
        }

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TARGET-PORT: <color><white>" + port.ToString() + "\n";

            return _temp;
        }

        #endregion
    }
}