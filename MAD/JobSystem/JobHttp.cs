using System;
using System.Net;

namespace MAD
{
    class JobHttp : Job
    {
        #region members

        public JobHttpOptions httpJobOptions;
        private WebRequest request;
        private WebResponse response;

        #endregion

        public JobHttp(JobHttpOptions httpJobOptions)
        {
            InitJob();

            this.httpJobOptions = httpJobOptions;
            this.jobOptions = (JobOptions)httpJobOptions;
        }

        #region methodes

        public override void DoJob()
        {
            request = WebRequest.Create("http://" + jobOptions.targetAddress.ToString() + ":" + httpJobOptions.port);

            try
            {
                response = request.GetResponse();
                jobOutput = "True";

                response.Close();
            }
            catch (Exception)
            {
                jobOutput = "False";
            }

            request.Abort();
        }

        public override string JobStatus()
        {
            string buffer = base.JobStatus();

            buffer += "PORT:      " + httpJobOptions.port + "\n";

            return buffer;
        }

        #endregion
    }
}