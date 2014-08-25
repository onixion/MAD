using System;
using System.Net;
using System.Runtime.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobHttp : Job
    {
        #region members

        public int port;

        private WebRequest _request;
        private WebResponse _response;

        #endregion

        #region constructors

        public JobHttp() 
            : base(JobType.Http)
        {
            this.port = 80;
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
                    outp.outState = JobOutput.OutState.Success;

                    _response.Close();
                }
                catch (Exception)
                {
                    outp.outState = JobOutput.OutState.Failed;
                }

                _request.Abort();
            }
            catch (Exception)
            {
                outp.outState = JobOutput.OutState.Exception;
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