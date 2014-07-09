using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace MAD.JobSystemCore
{
    public class JobNode
    {
        #region member

        // id
        private static int _nodesCount = 0;
        private object _nodeInitLock = new object();
        private int _id;
        public int id { get { return _id; } }

        // state
        public enum State { Active, Inactive, Exception }
        public State state = State.Inactive;

        // jobs
        public List<Job> jobs = new List<Job>();
        public object jobsLock = new object();
        public const int maxJobs = 100;

        public string nodeName;
        public PhysicalAddress macAddress;
        public IPAddress ipAddress;

        #endregion

        #region constructors

        public JobNode()
        {
            InitID();
        }

        public JobNode(string nodeName, PhysicalAddress macAddress, IPAddress ipAddress, List<Job> jobs)
        {
            InitID();
            this.nodeName = nodeName;
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            this.jobs = jobs;
        }

        #endregion

        #region methodes

        private void InitID()
        {
            lock (_nodeInitLock)
            {
                _id = _nodesCount;
                _nodesCount++;
            }
        }

        private Job GetJob(int jobID)
        {
            lock (jobsLock)
            {
                foreach (Job _job in jobs)
                {
                    if (_job.id == jobID)
                    {
                        return _job;
                    }
                }

                return null;
            }
        }

        #endregion
    }
}
