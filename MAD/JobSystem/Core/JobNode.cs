﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace MAD.JobSystemCore
{
    public class JobNode
    {
        private static int _idCount = 0;
        private object _initIDLock = new object();

        private int _id;
        public int nodeID { get { return _id; } }

        public enum State { Active, Inactive, Exception }
        private State _state = State.Inactive;
        public State state { get { return _state; } }

        private PhysicalAddress _macAddress;
        private IPAddress _ipAddress;

        public List<Job> _jobs;
        public object jobsLock = new object();

        private void InitID()
        {
            lock (_initIDLock)
            {
                _id = _idCount;
                _idCount++;
            }
        }

        public void InvokeJobs()
        {
            lock (jobsLock)
            {
                foreach (Job _job in _jobs)
                {
                    if (!_job.jobLocked)
                    {
                        if (_job.jobState == Job.JobState.Waiting)
                        {
                            _job.jobState = Job.JobState.Running;

                            _job.LaunchJob();

                            _job.jobState = Job.JobState.Stopped;
                        }
                    }
                }
            }
        }
    }
}
