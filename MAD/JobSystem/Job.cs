﻿using System;
using System.Net;
using System.Threading;

namespace MAD.JobSystem
{
    public abstract class Job
    {
        #region members

        private static int _jobsCount = 0;
        private static object _jobInitLock = new object();

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

        #region constructor

        protected Job(JobOptions jobOptions)
        {
            this.jobOptions = jobOptions;

            lock (_jobInitLock)
            {
                jobID = _jobsCount;
                _jobsCount++;
            }
        }

        #endregion

        #region methodes

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
                _cycleThread.Start();

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
                    Thread.Sleep(_cycleTime);

                    buffer = buffer - _cycleTime;

                    if (buffer <= 0)
                        break;
                }
                else if (jobOptions.jobTime.type == JobTime.TimeType.Absolute)
                {
                    Thread.Sleep(20000);

                    int _hourNow = DateTime.Now.Hour;
                    int _minuteNow = DateTime.Now.Hour;

                    foreach (DayTime _temp in jobOptions.jobTime.jobTimes)
                    {
                        if (_hourNow == _temp.hour && _minuteNow == _temp.minute)
                        {
                            break;
                        }
                    }
                }
                else
                { 
                    // JOBTIME NULL
                }
            }

            _cycleLock.Set();
        }

        public abstract void DoJob();

        #region for CLI only

        public string Status()
        {
            string _temp = "";

            _temp += "<color><yellow>ID:\t\t<color><white>" + jobID + "\n";
            _temp += "<color><yellow>NAME:\t<color><white>" + jobOptions.jobName + "\n";
            _temp += "<color><yellow>JOB-TYPE:\t<color><white>" + jobOptions.jobType.ToString() + "\n";
            _temp += "<color><yellow>JOB-TIME-TYPE:\t<color><white>" + jobOptions.jobTime.type.ToString() + "\n";
            // HERE!!
            if (jobOptions.jobTime.type == JobTime.TimeType.Relativ)
            {
                _temp += "<color><yellow>JOB-DELAY: <color><white>" + jobOptions.jobTime.jobDelay + "\n";
            }
            else
            {
                _temp += "<color><yellow>JOB-TIMES: <color><white>";

                foreach (DayTime _buffer in jobOptions.jobTime.jobTimes)
                {
                    _temp += _buffer.hour + ":" + _buffer.minute;
                }

                _temp += "\n";
            }

            _temp += "<color><yellow>JOB-STATE: <color><white>" + jobState.ToString()+ "\n";
            _temp += "<color><yellow>OUTPUT-STATE: <color><white>" + jobOutput.jobState.ToString() +"\n";

            return _temp + JobStatus();
        }

        protected abstract string JobStatus();

        #endregion

        #endregion
    }
}
