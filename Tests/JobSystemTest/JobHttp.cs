using System;
using System.Net;

namespace JobSystemTest
{
    class JobHttp : Job
    {
        WebRequest request;
        WebResponse response;

        public JobHttp(JobOptions options)
        {
            this.options = options;
            InitJob();
        }

        public override void DoJob()
        {
            while (true)
            {
                Console.Write("JobID: " + jobID + " HTTP Erfolgreich? -> ");

                request = WebRequest.Create("http://" + options.targetAddress.ToString() + ":" + options.targetPort);

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

                Wait(options.delayTime);
            }
        }
    }
}
