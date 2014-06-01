using System;
using System.Net;
using System.Threading;

namespace MAD
{
    public abstract class Job
    {
        #region members

        private static int _jobsCount = 0;
        private Thread _jobThread;
        private bool _threadStopRequest = false;
        
        private AutoResetEvent _cycleLock = new AutoResetEvent(false);
        private Thread _cycleThread;
        private static int _cycleTime = 100;

        public int jobID;
        public string jobOutput = "";
        public JobOptions jobOptions;

        #endregion

        #region methodes

        protected void InitJob(JobOptions jobOptions)
        {
            // set job-ID
            jobID = _jobsCount;
            _jobsCount++;

            // set jobName, jobDelay and jobType
            this.jobOptions = jobOptions;

            // init threads
            _jobThread = new Thread(WorkerThread);
            _cycleThread = new Thread(CycleLockSignal);
        }

        public bool Start()
        {
            if (!_threadStopRequest)
            {
                _threadStopRequest = true;
                _jobThread.Start();

                return true;
            }

            return false;
        }

        public bool Stop()
        {
            if (_threadStopRequest)
            {
                _threadStopRequest = false;
                _cycleLock.Set();

                // wait for thread to close
                _jobThread.Join();

                return true;
            }

            return false;
        }

        public bool Active()
        {
            return _jobThread.IsAlive;
        }

        private void WorkerThread()
        {
            while (true)
            {
                // execute cycleThread and start counting cycleTime
                _cycleThread.Start();

                // do job
                DoJob();
                _cycleLock.WaitOne();

                // check for any stop-requests
                if (_threadStopRequest)
                    break;
            }
        }

        private void CycleLockSignal()
        {
            // check every _cycleTime for 
            while (!_threadStopRequest)
            {
                Thread.Sleep(_cycleTime);
            }

            _cycleLock.Set();
        }

        public abstract void DoJob();

        #endregion
    }
}
