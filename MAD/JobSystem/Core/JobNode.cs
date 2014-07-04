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
        private object _initIDLock = new object();
        private object _nodeLock;

        public enum State { Active, Inactive, Exception }
        private State _state = State.Inactive;
        public State state { get { return _state; } }

        private string _nodeName;
        public string nodeName { get { return _nodeName; } }

        private PhysicalAddress _macAddress;
        public string macAddress { get { return _macAddress.ToString(); } }

        private IPAddress _ipAddress;
        public string ipAddress { get { return _ipAddress.ToString(); } }

        public List<Job> _jobs = new List<Job>();

        #endregion

        #region constructors

        public JobNode(object nodeLock)
        {
            InitID();
            _nodeLock = nodeLock;
        }

        public JobNode(object nodeLock, string nodeName, PhysicalAddress macAddress, IPAddress ipAddress, List<Job> jobs)
        {
            InitID();
            _nodeLock = nodeLock;
            _nodeName = nodeName;
            _macAddress = macAddress;
            _ipAddress = ipAddress;
            _jobs = jobs;
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
            lock (_nodeLock)
            {
                _nodeName = newName;
            }
        }

        public void UpdateMAC(PhysicalAddress newMACAddress)
        {
            lock (_nodeLock)
            {
                _macAddress = newMACAddress;
            }
        }

        public void UpdateIP(IPAddress newIPAddress)
        {
            lock (_nodeLock)
            {
                _ipAddress = newIPAddress;
            }
        }

        public void UpdateJob(int jobID, Job newJob)
        {
            lock (_nodeLock)
            {
                Job _job = GetJob(jobID);

                if (_job != null)
                {
                    // Replace old job.
                    _job = newJob;
                }
                else
                {
                    throw new Exception("Job does not exist!");
                }
            }
        }

        private Job GetJob(int jobID)
        {
            lock (_nodeLock)
            {
                foreach (Job _job in _jobs)
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
