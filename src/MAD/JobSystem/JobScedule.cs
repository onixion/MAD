using System;
using System.Threading;
using System.Collections.Generic;

using System.Net;
using System.Net.Mail;

using MAD;
using MAD.Notification;
using MAD.Logging;

using Amib.Threading;

namespace MAD.JobSystemCore
{
    public class JobScedule
    {
        #region members

        private Thread _cycleThread;
        private int _cycleTime = 100;

        private SmartThreadPool _workerPool;
        private int _maxThreads = 10;
        private int _maxTimeToWaitForIdle = 5000;

        public enum State { Active, Inactive, StopRequest }
        private State _state = State.Inactive;
        public State state { get { return _state; } }

        private List<JobNode> _jobNodes;
        private ReaderWriterLockSlim _nodesLock;

        private ManualResetEvent _lockSwitch = new ManualResetEvent(false);

        #endregion

        #region constructor

        public JobScedule(ReaderWriterLockSlim nodesLock, List<JobNode> jobNodes)
        {
            _nodesLock = nodesLock;
            _jobNodes = jobNodes;
            _workerPool = new SmartThreadPool(_maxThreads);
        }

        #endregion

        #region methodes

        public void Start()
        {
            if (_state == State.Inactive)
            {
                _state = State.Active;
                _cycleThread = new Thread(CycleJobTracker);
                _cycleThread.Start();
            }
        }

        public void Stop()
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

        private void CycleJobTracker()
        {
            JobHolder _holder = new JobHolder(); // not very nice, but works for now

            while(true)
            {
                DateTime _nowTime = DateTime.Now;

                _nodesLock.EnterReadLock();

                foreach (JobNode _node in _jobNodes)
                {
                    _node.nodeLock.EnterReadLock();

                    if (_node.state == JobNode.State.Active)
                    {
                        foreach (Job _job in _node.jobs)
                        {
                            _job.jobLock.EnterWriteLock();

                            if (_job.state == Job.JobState.Waiting)
                            {
                                if (_job.type != Job.JobType.NULL)
                                {
                                    if (_job.time.type == JobTime.TimeMethod.Relative)
                                    {
                                        if (_job.time.jobDelay.CheckTime())
                                        {
                                            _job.time.jobDelay.Reset();

                                            // Job is ready to be executed.

                                            _job.state = Job.JobState.Working;

                                            _holder.node = _node;
                                            _holder.job = _job;
                                            _holder.targetAddress = _node.ipAddress;

                                            JobThreadStart(_holder);

                                            // wait for Job to lock necessary locks.
                                            _lockSwitch.WaitOne();
                                            _lockSwitch.Reset();

                                            _job.jobLock.ExitWriteLock();
                                        }
                                        else
                                        {
                                            _job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
                                            _job.jobLock.ExitWriteLock();
                                        }
                                    }
                                    else if (_job.time.type == JobTime.TimeMethod.Absolute)
                                    {
                                        JobTimeHandler _timeHandler = _job.time.GetJobTimeHandler(_nowTime);

                                        if (_timeHandler != null)
                                        {
                                            if (!_timeHandler.IsBlocked(_nowTime))
                                            {
                                                if (_timeHandler.CheckTime(_nowTime))
                                                {
                                                    _timeHandler.minuteAtBlock = _nowTime.Minute;

                                                    // Job is ready to be executed.

                                                    _job.state = Job.JobState.Working;

                                                    _holder.node = _node;
                                                    _holder.job = _job;
                                                    _holder.targetAddress = _node.ipAddress;

                                                    JobThreadStart(_holder);

                                                    // wait for Job to lock necessary locks.
                                                    
                                                    _job.jobLock.ExitWriteLock();

                                                    _lockSwitch.WaitOne();
                                                    _lockSwitch.Reset();
                                                }
                                                else
                                                    _job.jobLock.ExitWriteLock();
                                            }
                                            else
                                                _job.jobLock.ExitWriteLock();
                                        }
                                        else
                                            _job.jobLock.ExitWriteLock();
                                    }
                                    else
                                        _job.jobLock.ExitWriteLock();
                                }
                                else
                                    _job.jobLock.ExitWriteLock();
                            }
                            else
                                _job.jobLock.ExitWriteLock();
                        }
                    }

                    _node.nodeLock.ExitReadLock();
                }

                _nodesLock.ExitReadLock();

                if (_state == State.StopRequest)
                    break;

                Thread.Sleep(_cycleTime);
            }
        }

        private void JobThreadStart(JobHolder holder)
        {
            _workerPool.QueueWorkItem(new WorkItemCallback(JobInvoke), holder);
        }

        private object JobInvoke(object holder)
        {
            JobHolder _holder = (JobHolder)holder;
            JobNode _node = _holder.node;
            Job _job = _holder.job;

            _nodesLock.EnterReadLock();
            _node.nodeLock.EnterReadLock();

            _lockSwitch.Set();

            _job.jobLock.EnterWriteLock();

            _job.tStart = DateTime.Now;
            try 
            {
                _job.Execute(_holder.targetAddress);
            }
            catch (Exception)
            {
                _job.state = Job.JobState.Exception;
            }
            _job.tStop = DateTime.Now;
            _job.tSpan = _job.tStop.Subtract(_job.tStart);

            _job.outp.outputs[0].dataObject = _job.outp.outState.ToString();

            if (_job.notiFlag)
            {
                List<JobRule> _bRules = GetBrokenRules(_job);

                // check if notification is neseccary.
                if (_bRules.Count != 0)
                {
                    string _mailSubject = GenMailSubject(_job, "Job (target='" + _holder.targetAddress.ToString()
                        + "') finished with a not expected result!");

                    string _mailContent = "";
                    _mailContent += "JobNode-ID:  '" + _node.id + "'\n";
                    _mailContent += "JobNode-IP:  '" + _node.ipAddress.ToString() + "'\n";
                    _mailContent += "JobNode-MAC: '" + _node.macAddress.ToString() + "'\n\n";
                    _mailContent += GenJobInfo(_job);
                    _mailContent += GenBrokenRulesText(_job.outp, _bRules);

                    if (_job.settings != null)
                    {
                        // This is not the perfect solution. Need to create a class, which
                        // can stack notifications, so we do not lose precious time here ...
                        NotificationSystem.SendMail(_job.settings.mailAddr, _mailSubject, _mailContent, 2,
                            _job.settings.login.smtpAddr, _job.settings.login.mail,
                            _job.settings.login.password, _job.settings.login.port);
                    }
                    else
                    {
                        if (MadConf.conf.defaultMailAddr != null || MadConf.conf.defaultMailAddr != "")
                        {
                            // Same like above.
                            // Here we use global-settings instead of job-settings.
                            NotificationSystem.SendMail(new MailAddress[1] { new MailAddress(MadConf.conf.defaultMailAddr) },
                                _mailSubject, _mailContent, 2);
                        }
                        else
                        { 
                            // No notification is possible, because the Job has no settings and the global settings are empty.
                            Logger.Log("No notification possible! Job has no settings and no global settings set!", Logger.MessageType.ERROR);
                        }
                    }
                }
            }

            _job.state = Job.JobState.Waiting;

            _job.jobLock.ExitWriteLock();
            _node.nodeLock.ExitReadLock();
            _nodesLock.ExitReadLock(); 
            return null;
        }

        #region generate mail text

        private string GenMailSubject(Job job, string message)
        { 
            return "[MAD][ERROR] - Job (JOB-ID: '" + job.id + "'): " + message;
        }

        private List<JobRule> GetBrokenRules(Job job)
        {
            List<JobRule> _bRules = new List<JobRule>();

            foreach (JobRule _rule in job.rules)
                if (!_rule.CheckRuleValidity(job.outp))
                    _bRules.Add(_rule);

            return _bRules;
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

                _buffer += "#" + _count + ".) Broken-Rule\n";
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
            _buffer += "Job-TStart:   '" + job.tStart.ToString("dd.MM.yyyy HH:mm:ss") + "'\n";
            _buffer += "Job-TStop:    '" + job.tStop.ToString("dd.MM.yyyy HH:mm:ss") + "'\n";
            _buffer += "Job-TSpan:    '" + job.tSpan.Seconds + "s" + job.tSpan.Milliseconds + "ms'\n\n";
            return _buffer;
        }

        #endregion

        #endregion
    }

    public class JobHolder
    {
        public JobNode node;
        public Job job;
        public System.Net.IPAddress targetAddress;
    }
}
