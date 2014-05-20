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
        public string jobOutput = "";

        public Thread jobThread;
        public bool threadStopRequest = false;
        public AutoResetEvent cycleLock = new AutoResetEvent(false);
        public Thread cycleThread;

        #endregion

        #region methodes

        public void InitJob()
        {
            // set job-ID
            jobID = jobsCount;
            jobsCount++;

            // init threads
            jobThread = new Thread(WorkerThread);
            cycleThread = new Thread(CycleLockSignal);
        }

        public bool Start()
        {
            if (!threadStopRequest)
            {
                threadStopRequest = true;
                jobThread.Start();

                return true;
            }

            return false;
        }

        public bool Stop()
        {
            if (threadStopRequest)
            {
                threadStopRequest = false;
                cycleLock.Set();

                return true;
            }

            return false;
        }

        public bool Active()
        {
            return jobThread.IsAlive;
        }

        public void WorkerThread()
        {
            while (true)
            {
                // execute cycleThread and start counting cycleTime
                cycleThread.Start();

                // do job
                DoJob();

                // wait for cycletime
                cycleLock.WaitOne();

                // check for any stop-requests
                if (threadStopRequest)
                    break;
            }
        }

        public abstract void DoJob();

        public void CycleLockSignal()
        {
            Thread.Sleep(jobOptions.delay);
            cycleLock.Set();
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
