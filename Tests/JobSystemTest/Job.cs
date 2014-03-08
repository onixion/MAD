using System;
using System.Threading;
using System.Net;

namespace JobSystemTest
{
    abstract class Job : IDisposable
    {
        // variables
        public JobOptions options;
        public Thread jobThread;
        public static int jobsCount = 0;
        public int jobID;
        public bool success;
        private bool disposed = false;

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
            if (!jobThread.IsAlive)
            {
                jobThread.Start();
            }
            else
            {
                jobThread.Resume();
            }
        }

        public void Stop()
        {
            if (jobThread.IsAlive)
            {
                jobThread.Suspend();
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

        public void Dispose()
        {
            if (disposed == false)
            {
                GC.SuppressFinalize(this);
                disposed = true;
            }
        }
    }
}
