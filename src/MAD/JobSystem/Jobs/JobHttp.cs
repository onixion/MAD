using System;
using System.Net;
using System.Collections.Specialized;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobHttp : Job
    {
        #region members

        public int port;

        [JsonIgnore]
        private WebRequest _request;
        [JsonIgnore]
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
                _response = _request.GetResponse();

                outp.outState = JobOutput.OutState.Success;
                _response.Close();
            }
            catch (Exception)
            {
                outp.outState = JobOutput.OutState.Failed;
            }
        }

        #endregion
    }
}