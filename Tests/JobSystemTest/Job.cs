using System;
using System.Threading;
using System.Net;

namespace JobSystemTest
{
    abstract class Job
    {
        // variables
        public JobOptions options;
        public Thread jobThread;
        public static int jobsCount = 0;
        public int jobID;
        public bool success;

        // methodes
        public abstract void DoJob();

        public void InitJob()
        {
            // set/get JobID
            jobID = jobsCount;
            jobsCount++;

            // create thread
            jobThread = new Thread(new ThreadStart(DoJob));
        }

        public void Start()
        {
            if (jobThread.ThreadState == ThreadState.Unstarted)
                jobThread.Start();
            else
                if (jobThread.ThreadState == ThreadState.Suspended)
                    jobThread.Resume();
        }

        public void Stop()
        {
            if (jobThread.ThreadState == ThreadState.WaitSleepJoin || jobThread.ThreadState == ThreadState.Running)
                jobThread.Suspend();
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
