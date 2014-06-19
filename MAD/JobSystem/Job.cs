using System;
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
        private Thread _cycleThread;

        private AutoResetEvent _cycleLock = new AutoResetEvent(false);
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
                if (jobOptions.jobTime.type == JobTime.TimeType.Relativ)
                {
                    _cycleThread = new Thread(CycleLockSignal);
                }
                else if (jobOptions.jobTime.type == JobTime.TimeType.Absolute)
                {
                    _cycleThread = new Thread(TimeLockSignal);
                }
                else
                {
                    throw new Exception("JOB-TIME-TYPE NULL!");
                }

                _cycleThread.Start();

                // wait for cycleThread to be finished OR get an stop-request
                _cycleLock.WaitOne();
                _cycleThread.Join();

                // check for any stop-requests
                if (jobState == State.StopRequest)
                {
                    jobState = State.Stopped;
                    break;
                }

                DoJob();
            }
        }

        private void CycleLockSignal()
        {
            int _buffer = jobOptions.jobTime.jobDelay;

            while (jobState == State.Running)
            {
                Thread.Sleep(_cycleTime);
                _buffer = _buffer - _cycleTime;

                if (_buffer <= 0)
                {
                    break;
                }
            }

            _cycleLock.Set();
        }

        private void TimeLockSignal()
        {
            bool _finished = false;

            while (jobState == State.Running)
            {
                Thread.Sleep(_cycleTime);
                DateTime _now = DateTime.Now;

                foreach (JobTimeHandler _handler in jobOptions.jobTime.jobTimes)
                {
                    if (_handler.CheckTime(_now))
                    {
                        if(!_handler.blockSignal)
                        {
                            _handler.BlockHandler();
                            _finished = true;
                            break;
                        }
                    }
                }

                if (_finished)
                {
                    break;
                }
            }

            _cycleLock.Set();
        }

        public abstract void DoJob();

        #region for CLI only

        public string Status()
        {
            string _temp = "_______________________________\n";

            _temp += "<color><yellow>ID: <color><white>" + jobID + "\n";
            _temp += "<color><yellow>NAME: <color><white>" + jobOptions.jobName + "\n";
            _temp += "<color><yellow>TYPE: <color><white>" + jobOptions.jobType.ToString() + "\n";
            
            _temp += "<color><yellow>TIME-TYPE: <color><white>" + jobOptions.jobTime.type.ToString() + "\n";

            if (jobOptions.jobTime.type == JobTime.TimeType.Relativ)
            {
                _temp += "<color><yellow>DELAY: <color><white>" + jobOptions.jobTime.jobDelay + "\n";
            }
            else if (jobOptions.jobTime.type == JobTime.TimeType.Absolute)
            {
                _temp += "<color><yellow>TIMES: <color><white>";

                foreach (JobTimeHandler _buffer in jobOptions.jobTime.jobTimes)
                {
                    _temp += _buffer.JobTimeStatus() + " ";
                }

                _temp += "\n";
            }

            _temp += "<color><yellow>STATE: <color><white>" + jobState.ToString()+ "\n";
            _temp += "<color><yellow>OUTPUT-STATE: <color><white>" + jobOutput.jobState.ToString() +"\n";
            _temp += "<color><yellow>OUTPUT-DESCRIPTOR: \n";

            foreach(JobDescriptor _buffer in jobOutput.jobOutputDescriptors)
            {
                _temp += "\n<color><yellow>Name: <color><white>" + _buffer.name + "\n";
                _temp += "<color><yellow>Type: <color><white>" + _buffer.dataType + "\n";

                if (_buffer.data != null)
                {
                    _temp += "<color><yellow>Value: <color><white>" + _buffer.data.ToString();
                }
                else
                {
                    _temp += "<color><yellow>Value: <color><white>NULL;";
                }

                _temp += "\n\n";
            }

            return _temp + JobStatus();
        }

        protected abstract string JobStatus();

        #endregion

        #endregion
    }
}
