using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public abstract class Job
    {
        #region members

        public enum JobType { NULL, Ping, PortScan, Http, HostDetect, ServiceCheck, SnmpCheck }
        public enum JobState { Inactive, Waiting, Working, Exception }

        [JsonIgnore]
        private static object _idLock = new object();
        [JsonIgnore]
        private static int _jobsCount = 0;
        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int id { get { return _id; } }
        [JsonIgnore]
        public JobOutput outp { get; set; }
        [JsonIgnore]
        public JobState state = JobState.Inactive;
        [JsonIgnore]
        public DateTime tStart { get; set; }
        [JsonIgnore]
        public DateTime tStop { get; set; }
        [JsonIgnore]
        public TimeSpan tSpan { get; set; }

        // general
        public Guid guid { get; set; }
        public string name { get; set; }
        public JobType type { get; set; }
        public JobTime time { get; set; }

        // notification
        public bool notiFlag = true;
        public JobNotificationSettings settings { get; set; }
        public List<JobRule> rules = new List<JobRule>();

        #endregion

        #region constructors

        protected Job(JobType type)
        {
            InitJob();

            this.guid = new Guid();
            this.type = type;
            this.outp = new JobOutput();
            this.time = new JobTime();
        }

        protected Job(string name, JobType type)
        {
            InitJob();

            this.guid = new Guid();
            this.name = name;
            this.type = type;
            this.outp = new JobOutput();
            this.time = new JobTime();
        }

        protected Job(string name, JobType type, JobTime time, JobOutput outp)
        {
            InitJob();

            this.guid = new Guid();
            this.name = name;
            this.type = type;
            this.outp = outp;
            this.time = time;
            this.settings = new JobNotificationSettings();
        }

        #endregion

        #region methodes

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
