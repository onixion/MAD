using System;
using System.Threading;
using System.Net;

namespace JobSystemTest
{
    abstract class Job
    {
        public JobOptions options;
        private Thread jobThread;
        public static int jobsCount = 0;
        public int jobID;
        public bool success;

        public abstract void DoJob();

        public void Init()
        {
            // set/get JobID
            jobID = jobsCount;
            jobsCount++;
        }

        public void Start()
        {
            if (jobThread == null)
            {
                jobThread = new Thread(new ThreadStart(DoJob));
                jobThread.Start();
            }
        }

        public void Stop()
        {
            if (jobThread != null)
            {
                jobThread.Abort();
                jobThread.Join();
                jobThread = null;
            }
        }

        public void Wait(int delayTime)
        {
            Thread.Sleep(delayTime);
        }

        public bool IsRunning()
        {
            if (jobThread.ThreadState.Equals(ThreadState.Running))
                return true;

            return false;
        }
    }
}
