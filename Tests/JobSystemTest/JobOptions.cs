using System;
using System.Net;

namespace JobSystemTest
{
    abstract class JobOptions
    {
        public string jobType;
        public string jobName;
        public int delayTime;
        public IPAddress targetAddress;
        public bool jobSuccsess;
    }
}
