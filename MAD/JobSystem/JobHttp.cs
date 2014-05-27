using System;
using System.Net;

namespace MAD
{
    class JobHttp : Job
    {
        #region members

        public JobHttpOptions httpJobOptions;
        private WebRequest _request;
        private WebResponse _response;

        #endregion

        public JobHttp(JobOptions jobOption, JobHttpOptions httpJobOptions)
        {
            InitJob(jobOption);
            this.httpJobOptions = httpJobOptions;
        }

        #region methodes

        public override void DoJob()
        {
            _request = WebRequest.Create("http://" + httpJobOptions.targetAddress.ToString() + ":" + httpJobOptions.port);

            try
            {
                _response = _request.GetResponse();
                jobOutput = "True";

                _response.Close();
            }
            catch (Exception)
            {
                jobOutput = "False";
            }

            _request.Abort();
        }

        #endregion
    }
}