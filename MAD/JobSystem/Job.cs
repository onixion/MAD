using System;
using System.Threading;
using System.Net;

namespace MAD
{
    public abstract class Job
    {
        public JobOptions jobOptions;
        private Thread jobThread;

        private static int jobsCount = 0;
        public int jobID;
        public string jobOutput = "";

        public abstract void DoJob();

        public void InitJob()
        {
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
            Thread.Sleep(jobOptions.delay);
        }

        public bool IsActive()
        {
            if (jobThread == null)
                return false;
            else
                return true;
        }

        public virtual string JobStatus()
        {
            string temp = "";

            temp += "JOB-ID:    " + jobID + "\n";
            temp += "NAME:      " + jobOptions.jobName + "\n";
            temp += "TYPE:      " + jobOptions.jobType.ToString() + "\n";
            temp += "ADDRESS:   " + jobOptions.targetAddress + "\n";
            temp += "DELAYTIME: " + jobOptions.delay + "\n";
            temp += "ACITIVE:   " + IsActive().ToString() + "\n";
            temp += "OUTPUT:    " + jobOutput + "\n";

            return temp;
        }
    }
}
