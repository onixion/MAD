using System;
using System.Net;

namespace MAD
{
    class JobHttp : Job
    {
        public JobHttpOptions httpJobOptions;
        private WebRequest request;
        private WebResponse response;

        public JobHttp(JobHttpOptions httpJobOptions)
        {
            InitJob();
            this.httpJobOptions = httpJobOptions;
            this.jobOptions = (JobOptions)httpJobOptions;
        }

        public override void DoJob()
        {
            request = WebRequest.Create("http://" + jobOptions.targetAddress.ToString() + ":" + httpJobOptions.port);

            try
            {
                response = request.GetResponse();
                jobOutput = "True";

                response.Close();
            }
            catch (Exception e)
            {
                jobOutput = "False";
            }

            request.Abort();
        }

        public override void JobStatus()
        {
            base.JobStatus();
            Console.WriteLine(string.Format("PORT:     \t{0}", httpJobOptions.port));
        }
    }
}