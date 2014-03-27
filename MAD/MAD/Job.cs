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
        public string jobOutput;

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
            Thread.Sleep(jobOptions.delay);
        }

        public bool IsActive()
        {
            if (jobThread == null)
                return false;
            else
                return true;
        }

        public virtual void JobStatus()
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("JOB-ID:           " + jobID);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("JOB-NAME:         " + jobOptions.jobName);
            Console.WriteLine("JOB-OUTPUT:       " + jobOutput);
            Console.WriteLine("JOB-ACITIVE:      " + IsActive().ToString());
            Console.WriteLine("JOB-TYPE:         " + jobOptions.jobType.ToString());
            Console.WriteLine("TARGET-ADDRESS:   " + jobOptions.targetAddress);
            Console.WriteLine("DELAYTIME:        " + jobOptions.delay);
        }
    }
}
