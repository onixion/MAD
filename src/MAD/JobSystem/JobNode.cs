using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobNode
    {
        #region members

        [JsonIgnore]
        private static object _idLock = new object();
        [JsonIgnore]
        private static int _nodesCount = 0;
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int id { get { return _id; } }
        [JsonIgnore]
        public const int MAXJOBS = 100;
        [JsonIgnore]
        public int state = 0; //active (state = 1) or inactive (state = 0)
        [JsonIgnore]
        public int uWorker = 0;

        public Guid guid { get; set; }
        public string name { get; set; }
        public string mac { get; set; }
        public IPAddress ip { get; set; } // sns
        public List<Job> jobs = new List<Job>();

        #endregion

        #region constructors

        public JobNode()
        {
            InitID();
            this.guid = Guid.NewGuid();
        }

        public JobNode(string nodeName, string macAddress, IPAddress ipAddress, List<Job> jobs)
        {
            InitID();

            this.guid = Guid.NewGuid();
            this.name = nodeName;
            this.mac = macAddress;
            this.ip = ipAddress;
            this.jobs = jobs;
        }

        #endregion

        #region methods

        private void InitID()
        {
            lock (_idLock)
            {
                _id = _nodesCount;
                _nodesCount++;
            }
        }

        #endregion
    }
}
