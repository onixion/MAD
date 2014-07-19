using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace MAD.JobSystemCore
{
    public struct JobNodeInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public PhysicalAddress mac { get; set; }
        public IPAddress ipAddress { get; set; }
        public List<JobInfo> jobs { get; set; }
    }

    public struct JobInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public Job.JobType type { get; set; }
        public Job.JobState state { get; set; }
        public Job.OutState outpute { get; set; }
    }
}
