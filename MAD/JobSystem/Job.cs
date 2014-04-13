﻿using System;
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

        public virtual void JobStatus()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(string.Format("JOB-ID: " + jobID));
            Console.ForegroundColor = MadComponents.components.cli.textColor;
            Console.WriteLine(string.Format("NAME:     \t{0}", jobOptions.jobName));
            Console.WriteLine(string.Format("TYPE:     \t{0}", jobOptions.jobType.ToString()));
            Console.WriteLine(string.Format("ADDRESS:  \t{0}", jobOptions.targetAddress));
            Console.WriteLine(string.Format("DELAYTIME:\t{0}", jobOptions.delay));
            Console.WriteLine(string.Format("ACITIVE:  \t{0}", IsActive().ToString()));
            Console.WriteLine(string.Format("OUTPUT:   \t{0}", jobOutput));
        }
    }
}