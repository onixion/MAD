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

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(string.Format("JOB\t\t\t\t\t"));
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(string.Format("\tID:       \t{0}", jobID));
            Console.WriteLine(string.Format("\tNAME:     \t{0}", jobOptions.jobName));
            Console.WriteLine(string.Format("\tTYPE:     \t{0}", jobOptions.jobType.ToString()));
            Console.WriteLine(string.Format("\tADDRESS:  \t{0}", jobOptions.targetAddress));
            Console.WriteLine(string.Format("\tDELAYTIME:\t{0}", jobOptions.delay));
            Console.WriteLine(string.Format("\tACITIVE:  \t{0}", IsActive().ToString()));
            Console.WriteLine(string.Format("\tOUTPUT:   \t{0}", jobOutput));

        }
    }
}
