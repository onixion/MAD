using System;
using System.Net;

namespace MAD.JobSystem
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
        {
            InitJob(JobDefaultValues.defaultValues.defaultJobOptions);
            jobOptions.jobType = JobOptions.JobType.HttpRequest;

            this.targetAddress = JobDefaultValues.defaultValues.defaultTargetAddress;
            this.port = JobDefaultValues.defaultValues.defaultPort;
        }

        public JobHttp(JobOptions jobOption, IPAddress targetAddress, int port)
        {
            InitJob(jobOption);

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

        public override string Status()
        {
            string _temp = base.Status();

            _temp += "<color><yellow>TARGET-IP: <color><white>" + targetAddress.ToString() + "\n";
            _temp += "<color><yellow>TARGET-PORT: <color><white>" + port.ToString() + "\n";

            return _temp;
        }

        #endregion
    }
}