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

        public void WorkerThread()
        {
            while (true)
            {
                DoJob();
                Wait();
            }
        }

        public void Start()
        {
            if (jobThread == null)
            {
                jobThread = new Thread(new ThreadStart(WorkerThread));
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

        public void Wait()
        {
            Thread.Sleep(options.delayTime);
        }

        public bool IsRunning()
        {
            if (jobThread != null)
                return true;

            return false;
        }
    }
}
