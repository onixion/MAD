using System;
using System.Threading;
using System.Collections.Generic;

namespace MAD.jobSys
{
    public class JobScedule
    {
        private List<Job> _jobs;

        private Thread _cycleThread;
        private object _cycleThreadLock = new object();
        private int _cycleTime = 100;

        public enum State { Running, Stopped, StopRequest }
        private State _state = State.Stopped;
        public State state { get { return _state; } }

        public JobScedule(List<Job> jobs)
        {
            _jobs = jobs;
        }

        public void Start()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Stopped)
                {
                    _state = State.Running;
                    _cycleThread = new Thread(CycleJobTracker);
                    _cycleThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Running)
                {
                    _state = State.StopRequest;
                    _cycleThread.Join();
                    _state = State.Stopped;
                }
            }
        }

        public string GetState()
        {
            return _state.ToString();
        }

        private void CycleJobTracker()
        {
            while(true)
            {
                DateTime _time = DateTime.Now;
                Thread.Sleep(_cycleTime);
               
                foreach(Job _job in _jobs)
                {
                    CheckJobTimeAndExecute(_time, _job);
                }

                if (_state == State.StopRequest)
                {
                    break;
                }
            }
        }

        private void CheckJobTimeAndExecute(DateTime time, Job job)
        {
            JobTime _jobTime = job.jobOptions.jobTime;

            if (_jobTime.type == JobTime.TimeType.Relativ)
            {
                _jobTime.jobDelayRemaining = _jobTime.jobDelayRemaining - _cycleTime;

                if (_jobTime.jobDelayRemaining <= 0)
                {
                    _jobTime.jobDelayRemaining = _jobTime.jobDelay;
                    job.TryExecute();
                }
            }
            else if (_jobTime.type == JobTime.TimeType.Absolute)
            {
                foreach (JobTimeHandler _handler in _jobTime.jobTimes)
                {
                    if (_handler.CheckTime(time))
                    { 
                        // execute job
                        job.TryExecute();
                    }
                }
            }
            else
            {
                throw new Exception("JOBTIME-Type is NULL!");
            }
        }
    }
}
