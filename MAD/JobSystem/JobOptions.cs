﻿using System;
using System.Net;

namespace MAD
{
    public class JobOptions
    {
        public string jobName;
        public int jobDelay;

        // jobType is needed for the jobSystem to create the right job.
        public JobType jobType;
        public enum JobType { PingRequest, PortRequest, HttpRequest }

        public JobOptions(string jobName, int jobDelay, JobType jobType)
        {
            this.jobName = jobName;
            this.jobDelay = jobDelay;
            this.jobType = jobType; 
        }
    }
}
