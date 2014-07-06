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
        private static int _idCount = 0;
        private int _id;
        public int nodeID { get { return _id; } }

        // locks
        private object _initIDLock = new object(); // really necessesary?
        private object _jsNodesLock;

        // state
        public enum State { Active, Inactive, Exception }
        public State state = State.Inactive;

        // jobs
        public List<Job> jobs = new List<Job>();
        public const int maxJobs = 100;

        // node-name
        public string nodeName;

        // mac-address
        public PhysicalAddress macAddress;

        // ip-address
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
            lock (_initIDLock)
            {
                _id = _idCount;
                _idCount++;
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
                    if (_job.jobID == jobID)
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
