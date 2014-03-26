using System;
using System.Threading;
using System.Net;

namespace MAD
{
    public abstract class Job
    {
        public JobOptions jobOptions;
        private Thread jobThread;
        public static int jobsCount = 0;
        public int jobID;
        public string[] jobOutput;

        public string output;

        public abstract void DoJob();

        public void InitJob()
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
            Thread.Sleep(jobOptions.delayTime);
        }

        public bool IsRunning()
        {
            if (jobThread != null)
                return true;

            return false;
        }

        public virtual void JobStatus()
        {
            Console.WriteLine("Job-ID:           " + jobID);
            Console.WriteLine("Job-Name:         " + jobOptions.jobName);
            Console.WriteLine("Target-Address:   " + jobOptions.targetAddress);
            Console.WriteLine("Delaytime:        " + jobOptions.delayTime);
        }
    }
}
