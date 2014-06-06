using System;
using System.Net;
using System.Threading;

namespace MAD.JobSystem
{
    public abstract class Job
    {
        #region members

        private static int _jobsCount = 0;
        private Thread _jobThread;
        
        private AutoResetEvent _cycleLock = new AutoResetEvent(false);
        private Thread _cycleThread;
        private static int _cycleTime = 100;

        public int jobID;
        public JobOptions jobOptions;
        public JobOutput jobOutput = new JobOutput();

        public State jobState = State.Stopped;
        public enum State { Running, StopRequest, Stopped, Exception }

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
            if (jobState == State.Stopped)
            {
                jobState = State.Running;
                _jobThread.Start();

                return true;
            }

            return false;
        }

        public bool Stop()
        {
            if (jobState == State.Running)
            {
                jobState = State.StopRequest;

                // wait for threads to close
                _jobThread.Join();
                _cycleThread.Join();

                return true;
            }

            return false;
        }

        private void WorkerThread()
        {
            while (true)
            {
                // execute cycleThread and start decreasing delayTime
                _cycleThread.Start();

                // do job
                DoJob();

                // wait for cycleThread to be finished OR get an stop-request
                _cycleLock.WaitOne();

                // check for any stop-requests
                if (jobState == State.StopRequest)
                {
                    jobState = State.Stopped;
                    break;
                }
            }
        }

        private void CycleLockSignal()
        {
            int buffer = jobOptions.jobDelay;

            // check every _cycleTime for 
            while (jobState == State.Running)
            {
                Thread.Sleep(_cycleTime);
                buffer = buffer - _cycleTime;

                if (buffer <= 0)
                    break;
            }

            _cycleLock.Set();
        }

        public abstract void DoJob();

        #region for CLI only

        public virtual string Status()
        {
            string _temp = "";

            _temp += "<color><yellow>ID: <color><white>" + jobID + "\n";
            _temp += "<color><yellow>NAME: <color><white>" + jobOptions.jobName + "\n";
            _temp += "<color><yellow>JOB-TYPE: <color><white>" + jobOptions.jobType.ToString() + "\n";
            _temp += "<color><yellow>DELAY(ms): <color><white>" + jobOptions.jobDelay + "\n";
            _temp += "<color><yellow>JOB-STATE: <color><white>" + jobState.ToString()+ "\n";
            _temp += "<color><yellow>OUTPUT-STATE: <color><white>" + jobOutput.jobState.ToString() +"\n";

            return _temp;
        }

        #endregion

        #endregion
    }
}
