using System;
using System.Threading;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;

using MAD;
using MAD.Notification;

using Amib.Threading;

namespace MAD.JobSystemCore
{
    public class JobScedule
    {
        #region members

        private Thread _cycleThread;
        private int _cycleTime = 100;
        private object _cycleThreadLock = new object();

        private SmartThreadPool _workerPool;
        private int _maxThreads = 10;
        private int _maxTimeToWaitForIdle = 5000;

        public enum State { Active, Inactive, StopRequest }
        private State _state = State.Inactive;
        public State state { get { return _state; } }

        private List<JobNode> _jobNodes;
        private object _nodesLock;

        #endregion

        #region constructor

        public JobScedule(object nodesLock, List<JobNode> jobNodes)
        {
            _nodesLock = nodesLock;
            _jobNodes = jobNodes;
            _workerPool = new SmartThreadPool(_maxThreads);
        }

        #endregion

        #region methodes

        public void Start()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Inactive)
                {
                    _state = State.Active;
                    _cycleThread = new Thread(CycleJobTracker);
                    _cycleThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_cycleThreadLock)
            {
                if (_state == State.Active)
                {
                    _state = State.StopRequest;
                    
                    _cycleThread.Join();
                    _workerPool.WaitForIdle(_maxTimeToWaitForIdle);
                    _workerPool.Cancel();

                    _state = State.Inactive;
                }
            }
        }

        private void CycleJobTracker()
        {
            JobHolder _holder = new JobHolder(); // not very nice, but works for now

            while(true)
            {
                DateTime _time = DateTime.Now;

                // Lock node-list.
                lock (_nodesLock)
                {
                    foreach (JobNode _node in _jobNodes)
                    {
                        // Lock node.
                        lock (_node.nodeLock)
                        {
                            if (_node.state == JobNode.State.Active)
                            {
                                // Lock job-list of the node.
                                lock (_node.jobsLock)
                                {
                                    foreach (Job _job in _node.jobs)
                                    {
                                        // NOT LOCK JOB -> Job need to do itself.

                                        if (_job.state == Job.JobState.Waiting)
                                        {
                                            if (_job.type != Job.JobType.NULL)
                                            {
                                                if (CheckJobTime(_job.time, _time))
                                                {
                                                    if (_job.time.type == JobTime.TimeMethod.Relative)
                                                    {
                                                        _job.state = Job.JobState.Working;
                                                        _job.time.jobDelay.Reset();

                                                        _holder.job = _job; // SEARCHING FOR BETTER SOLUTION!
                                                        _holder.targetAddress = _node.ipAddress; // SEARCHING FOR BETTER SOLUTION!

                                                        JobThreadStart(_holder);
                                                    }
                                                    else if (_job.time.type == JobTime.TimeMethod.Absolute)
                                                    {
                                                        JobTimeHandler _handler = _job.time.GetJobTimeHandler(_time);

                                                        if (!_handler.IsBlocked(_time))
                                                        {
                                                            _job.state = Job.JobState.Working;
                                                            _handler.minuteAtBlock = _time.Minute;

                                                            _holder.job = _job; // SEARCHING FOR BETTER SOLUTION!
                                                            _holder.targetAddress = _node.ipAddress; // SEARCHING FOR BETTER SOLUTION!

                                                            JobThreadStart(_holder);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (_job.time.type == JobTime.TimeMethod.Relative)
                                                        _job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (_state == State.StopRequest)
                    break;

                Thread.Sleep(_cycleTime);
            }
        }

        private bool CheckJobTime(JobTime jobTime, DateTime time)
        {
            if (jobTime.type == JobTime.TimeMethod.Relative)
                if (jobTime.jobDelay.CheckTime())
                    return true;
                else
                    return false;
            else if (jobTime.type == JobTime.TimeMethod.Absolute)
                foreach (JobTimeHandler _handler in jobTime.jobTimes)
                    if (_handler.CheckTime(time))
                        return true;
                    else
                        return false;
            return false;
        }

        private void JobThreadStart(JobHolder holder)
        {
            _workerPool.QueueWorkItem(new WorkItemCallback(JobInvoke),holder);
        }

        private object JobInvoke(object holder)
        {
            JobHolder _holder = (JobHolder)holder;
            Job _job = _holder.job;

            lock (_job)
            {
                _job.tStart = DateTime.Now;
                try { _job.Execute(_holder.targetAddress); }
                catch (Exception)
                {
                    _job.state = Job.JobState.Exception;
                }
                _job.tStop = DateTime.Now;
                _job.tSpan = _job.tStop.Subtract(_job.tStart);

                if (_job.noti.useNotification)
                {
                    List<JobRule> _bRules = _job.noti.GetBrokenRules(_job.outp);

                    // check if notification is neseccary.
                    if (_job.outp.outState == JobOutput.OutState.Exception ||
                        _job.outp.outState == JobOutput.OutState.Failed ||
                        _bRules.Count != 0)
                    {
                        string _mailSubject = GenMailSubject(_job, "Job (target='" + _holder.targetAddress.ToString() 
                            + "') finished with a not expected result!");

                        string _mailContent = GenJobInfo(_job);
                        _mailContent += GenBrokenRulesText(_job.outp, _bRules);

                        if (_job.noti.settings != null)
                        {
                            // This is not the perfect solution. Need to create a class, which
                            // can stack notifications, so we do not lose precious time here ...
                            NotificationSystem.SendMail(_job.noti.settings.mailAddr, _mailSubject, _mailContent, 2,
                                _job.noti.settings.login.smtpAddr, _job.noti.settings.login.mail,
                                _job.noti.settings.login.password, _job.noti.settings.login.port);
                        }
                        else
                        {
                            // use global notifiaction settings
                            NotificationSystem.SendMail(new MailAddress[1] {new MailAddress(MadConf.conf.defaultMailAddr) },
                                _mailSubject, _mailContent, 2);
                        }
                    }
                }
            }

            // After this line, the scedule will notice this current job again.
            _job.state = Job.JobState.Waiting;

            return null;
        }

        #region generate mail text

        private string GenMailSubject(Job job, string message)
        { 
            return "[MAD][ERROR] - Job (JOB-ID: '" + job.id + "'): " + message;
        }

        private string GenBrokenRulesText(JobOutput outp, List<JobRule> bRules)
        {
            string _buffer = "";
            int _count = 0;
            foreach (JobRule _rule in bRules)
            {
                object _data = outp.GetOutputDesc(_rule.outDescName).dataObject;
                if (_data == null)
                    _data = (string)"NULL";

                _buffer += _count + ".) Rule\n";
                _buffer += "-> Rule Equation:    " + _rule.outDescName + " <" + _rule.oper.ToString() + "> '" + _rule.compareValue.ToString() + "' = TRUE\n";
                _buffer += "-> Current Equation: '" + _data.ToString() + "' <" + _rule.oper.ToString() + "> '" + _rule.compareValue.ToString() + "' = FALSE\n\n";

                _buffer += "-> OutDescriptor: " + _rule.outDescName + "\n";
                _buffer += "-> Operation:     " + _rule.oper.ToString() + "\n";
                _buffer += "-> CompareValue:  '" + _rule.compareValue.ToString() + "'\n";
                _buffer += "=> CurrentValue:  '" + _data.ToString() + "'\n\n";
                _count++;
            }
            return _buffer;
        }
            
        private string GenJobInfo(Job job)
        {
            string _buffer = "";
            _buffer += "Job-ID:       '" + job.id + "'\n";
            _buffer += "Job-Name:     '" + job.name + "'\n";
            _buffer += "Job-Type:     '" + job.type.ToString() + "'\n";
            _buffer += "Job-OutState: '" + job.outp.outState.ToString() + "'.\n";
            _buffer += "Job-TStart:   '" + job.tStart + "'\n";
            _buffer += "Job-TStop:    '" + job.tStop + "'\n";
            _buffer += "Job-TSpan:    '" + job.tSpan + "'\n\n";
            return _buffer;
        }

        #endregion

        #endregion
    }

    public class JobHolder
    { 
        public Job job;
        public System.Net.IPAddress targetAddress;
    }
}
