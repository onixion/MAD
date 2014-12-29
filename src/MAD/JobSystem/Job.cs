using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public abstract class Job
    {
        #region members

        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck, SnmpCheck, Probe }
        public enum Protocol { NULL, ICMP, TCP, UDP, RAW }

        [JsonIgnore]
        private static object _idLock = new object();
        [JsonIgnore]
        private static int _jobsCount = 0;
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int id { get { return _id; } }
        [JsonIgnore]
        public int state = 0;
        /* This INT represents the state of the job.
         * state = 0 | stopped
         * state = 1 | waiting
         * state = 2 | working */
        [JsonIgnore]
        public JobOutput outp { get; set; }
        [JsonIgnore]
        public DateTime tStart { get; set; }
        [JsonIgnore]
        public DateTime tStop { get; set; }
        [JsonIgnore]
        public TimeSpan tSpan { get; set; }

        public Guid guid { get; set; }
        public string name { get; set; }
        public JobType type { get; set; }
        public JobTime time { get; set; }

        public Protocol prot { get; set; }

        public bool notiFlag = true;
        public JobNotificationSettings settings { get; set; }
        public List<JobRule> rules = new List<JobRule>();

        #endregion

        #region constructors

        protected Job(JobType type, Protocol protocol)
        {
            InitJob();

            this.guid = Guid.NewGuid();
            this.type = type;
            this.prot = prot;
            this.outp = new JobOutput();
            this.time = new JobTime();
        }

        protected Job(string name, JobType type, Protocol protocol)
        {
            InitJob();

            this.guid = Guid.NewGuid();
            this.name = name;
            this.type = type;
            this.prot = protocol;
            this.outp = new JobOutput();
            this.time = new JobTime();
        }

        protected Job(string name, JobType type, Protocol protocol, JobTime time, JobOutput outp)
        {
            InitJob();

            this.guid = Guid.NewGuid();
            this.name = name;
            this.type = type;
            this.prot = protocol;
            this.outp = outp;
            this.time = time;
            this.settings = new JobNotificationSettings();
        }

        #endregion

        #region methods

        private void InitJob()
        {
            lock (_idLock)
            {
                _id = _jobsCount;
                _jobsCount++;
            }
        }

        public abstract void Execute(IPAddress targetAddress);

        #endregion
    }
}
