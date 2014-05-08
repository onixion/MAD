using System;
using System.Net;
using System.Threading;

namespace MAD
{
    public abstract class Job
    {
        #region members

        private static int jobsCount = 0;
        public int jobID;

        public JobOptions jobOptions;
        private Thread jobThread;
        public bool threadStopRequest = false;

        public string jobOutput = "";

        #endregion

        #region methodes

        public void InitJob()
        {
            // set job-ID
            jobID = jobsCount;
            jobsCount++;

            // init workerthread
            jobThread = new Thread(WorkerThread);
        }

        public void Start()
        {
            if (!threadStopRequest)
            {
                threadStopRequest = true;
                jobThread.Start();
            }
        }

        public void Stop()
        {
            if (threadStopRequest)
            {
                threadStopRequest = false;
            }
        }

        public void WorkerThread()
        {
            while (true)
            {
                DoJob();
                Wait();

                if (threadStopRequest)
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
            buffer += "ACITIVE:   " + threadStopRequest.ToString() + "\n";
            buffer += "OUTPUT:    " + jobOutput + "\n";

            return buffer;
        }

        #endregion
    }
}
