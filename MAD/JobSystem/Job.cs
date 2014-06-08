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
        }

        public bool Start()
        {
            if (jobState == State.Stopped)
            {
                jobState = State.Running;

                _jobThread = new Thread(WorkerThread);

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
                _cycleThread = new Thread(CycleLockSignal);

                // execute cycleThread and start decreasing delayTime
                _cycleThread.Start();

                // do job
                DoJob();

                // wait for cycleThread to be finished OR get an stop-request
                _cycleLock.WaitOne();
                _cycleThread.Join();

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
            int buffer = jobOptions.jobTime.jobDelay;

            while (jobState == State.Running)
            {
                if (jobOptions.jobTime.type == JobTime.TimeType.Relativ)
                {
                    // check every _cycleTime for
                    Thread.Sleep(_cycleTime);
                    buffer = buffer - _cycleTime;

                    if (buffer <= 0)
                        break;
                }
                else
                {
                    if (DateTime.Now.Minute == jobOptions.jobTime.jobTimes[0].Minute && DateTime.Now.Hour == jobOptions.jobTime.jobTimes[0].Hour)
                        break;
                }
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
            _temp += "<color><yellow>JOB-TIME-TYPE: <color><white>" + jobOptions.jobTime.type.ToString() + "\n";

            if (jobOptions.jobTime.type == JobTime.TimeType.Relativ)
            {
                _temp += "<color><yellow>JOB-DELAY: <color><white>" + jobOptions.jobTime.jobDelay + "\n";
            }
            else
            {
                _temp += "<color><yellow>JOB-TIMES: <color><white>";

                foreach (DateTime _buffer in jobOptions.jobTime.jobTimes)
                {
                    _temp += _buffer.ToString("HH:mm ");
                }

                _temp += "\n";
            }

            _temp += "<color><yellow>JOB-STATE: <color><white>" + jobState.ToString()+ "\n";
            _temp += "<color><yellow>OUTPUT-STATE: <color><white>" + jobOutput.jobState.ToString() +"\n";

            return _temp;
        }

        #endregion

        #endregion
    }
}
