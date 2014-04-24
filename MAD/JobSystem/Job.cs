using System;
using System.Net;
using System.Threading;

namespace MAD
{
    public abstract class Job
    {
        #region members

        public JobOptions jobOptions;

        private static int jobsCount = 0;
        public int jobID;
        public string jobOutput = "";

        private Thread jobThread;
        public bool threadRunning = false;

        #endregion

        #region methodes

        public void InitJob()
        {
            // get job-ID
            jobID = jobsCount;
            jobsCount++;

            // init workerthread
            jobThread = new Thread(WorkerThread);
        }

        public void Start()
        {
            if (!threadRunning)
            {
                threadRunning = true;
                jobThread.Start();
            }
        }

        public void Stop()
        {
            if (threadRunning)
            {
                threadRunning = false;
            }
        }

        public void WorkerThread()
        {
            while (true)
            {
                DoJob();
                Wait();

                if (threadRunning)
                    break;
            }
        }

        public abstract void DoJob();

        public void Wait()
        {
            Thread.Sleep(jobOptions.delay);
        }

        public virtual string JobStatus()
        {
            string buffer = "";

            buffer += "JOB-ID:    " + jobID + "\n";
            buffer += "NAME:      " + jobOptions.jobName + "\n";
            buffer += "TYPE:      " + jobOptions.jobType.ToString() + "\n";
            buffer += "ADDRESS:   " + jobOptions.targetAddress + "\n";
            buffer += "DELAYTIME: " + jobOptions.delay + "\n";
            buffer += "ACITIVE:   " + threadRunning.ToString() + "\n";
            buffer += "OUTPUT:    " + jobOutput + "\n";

            return buffer;
        }

        #endregion
    }
}
