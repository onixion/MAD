using System;
using System.Net;

namespace JobSystemTest
{
    class JobHttp : Job
    {
        JobHttpOptions options;

        WebRequest request;
        WebResponse response;

        public JobHttp(JobHttpOptions options)
        {
            Init();
            this.options = options;
        }

        public override void DoJob()
        {
            Console.Write("JobID: " + jobID + " HTTP Erfolgreich? -> ");

            request = WebRequest.Create("http://" + options.targetAddress.ToString() + ":" + options.port);

            try
            {
                response = request.GetResponse();
                success = true;
                response.Close();
            }
            catch (Exception e)
            {
                success = false;
            }

            request.Abort();

            Console.WriteLine(success);
        }
    }
}