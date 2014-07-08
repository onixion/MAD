using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace MAD.JobSystemCore
{
    public class JobNode
    {
        #region member

        private static int _nodesCount = 0;
        private object _nodeInitLock = new object();
        private int _id;
        public int id { get { return _id; } }

        public enum State { Active, Inactive, Exception }
        public State state = State.Inactive;

        public List<Job> jobs = new List<Job>();
        public const int maxJobs = 100;

        private object _jsNodesLock;

        public string nodeName;
        public PhysicalAddress macAddress;
        public IPAddress ipAddress;

        #endregion

        #region constructors

        public JobNode(object jsNodesLock)
        {
            InitID();
            _jsNodesLock = jsNodesLock;
        }

        public JobNode(object jsNodesLock, string nodeName, PhysicalAddress macAddress, IPAddress ipAddress, List<Job> jobs)
        {
            InitID();
            _jsNodesLock = jsNodesLock;
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
                _id++;
            }
        }

        public void UpdateName(string newName)
        {
            lock (_jsNodesLock)
            {
                nodeName = newName;
            }
        }

        public void UpdateMAC(PhysicalAddress newMACAddress)
        {
            lock (_jsNodesLock)
            {
                macAddress = newMACAddress;
            }
        }

        public void UpdateIP(IPAddress newIPAddress)
        {
            lock (_jsNodesLock)
            {
                ipAddress = newIPAddress;
            }
        }

        public bool UpdateJob(int jobID, Job newJob) // return bool is much faster than throwing exceptions ..
        {
            lock (_jsNodesLock)
            {
                Job _job = GetJob(jobID);

                if (_job != null)
                {
                    // Replace old job.
                    _job = newJob;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private Job GetJob(int jobID)
        {
            lock (_jsNodesLock)
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
