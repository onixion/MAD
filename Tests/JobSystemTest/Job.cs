using System;
using System.Threading;
using System.Net;

namespace JobSystemTest
{
    abstract class Job : IDisposable
    {
        // variables
        public Thread jobThread;
        public static long jobsCount = 0;
        public long jobID;
        public bool success;
        private bool disposed = false;

        public JobOptions options;

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
            if (!jobThread.ThreadState.Equals(ThreadState.Running))
            {
                jobThread.Start();
            }
        }

        public void Stop()
        {
            if (!jobThread.ThreadState.Equals(ThreadState.Stopped))
            {
                jobThread.Abort();
            }
        }

        public bool IsRunning()
        {
            if (jobThread.ThreadState.Equals(ThreadState.Running))
                return true;

            return false;
        }

        public void WriteConsole()
        {
            Console.WriteLine("JobID: " + jobID);
        }

        public void Dispose()
        {

        }
    }
}
