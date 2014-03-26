using System;
using System.Net;

namespace MAD
{
    class JobHttp : Job
    {
        JobHttpOptions httpOptions;

        WebRequest request;
        WebResponse response;

        public JobHttp(JobOptions jobOptions, JobHttpOptions httpOptions)
        {
            InitJob();
            jobOutput = new string[1];

            this.jobOptions = jobOptions;
            this.httpOptions = httpOptions;
        }

        public override void DoJob()
        {
            Console.Write("JobID: " + jobID + " HTTP Erfolgreich? -> ");

            request = WebRequest.Create("http://" + jobOptions.targetAddress.ToString() + ":" + httpOptions.port);

            try
            {
                response = request.GetResponse();
                jobOutput[0] = "HTTP WEBSERVER FOUND";

                response.Close();
            }
            catch (Exception e)
            {
                jobOutput[0] = "NO HTTP WEBSERVER FOUND!";
            }

            request.Abort();
        }
    }
}