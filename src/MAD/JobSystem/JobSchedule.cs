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
    public class JobSchedule
    {
        #region members

        private Thread _cycleThread;
        private int _cycleTime = 10;
        private int _fetchIPTime = 5;

        private object _stateLock = new object();
        /* state = 0 | inactive
         * state = 1 | active
         * state = 2 | stop-request */
        private int _state = 0;
        public int state { get { return _state; } }

        private SmartThreadPool _workerPool;
        private int _maxThreads = 10;
        private int _maxTimeToWaitForIdle = 5000;

        private List<JobNode> _jobNodes;
        private object _jsLock = new object();

        #endregion

        #region constructor

        public JobSchedule(object jsLock, List<JobNode> jobNodes)
        {
            _jsLock = jsLock;
            _jobNodes = jobNodes;
            _workerPool = new SmartThreadPool(_maxThreads);
        }

        #endregion

        #region methods

        public void Start()
        {
            lock (_stateLock)
            {
                if (_state == 0)
                {
                    _state = 1;
                    _cycleThread = new Thread(CycleJobTracker);
                    _cycleThread.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_stateLock)
            {
                if (_state == 1)
                {
                    _state = 2;

                    _cycleThread.Join();
                    _workerPool.WaitForIdle(_maxTimeToWaitForIdle);
                    _workerPool.Cancel();

                    _state = 0;
                }
            }
        }

        private void CycleJobTracker()
        {
            while(true)
            {
                DateTime _nowTime = DateTime.Now;
                try
                {
                    lock (_jsLock)
                    {
                        foreach (JobNode _node in _jobNodes)
                        {
                            if (_node.state == 1)
                            {
                                foreach (Job _job in _node.jobs)
                                {
                                    if (_job.state == 1 || _job.state == 2)
                                    {
                                        if (JobTimeCheck(_job, _nowTime))
                                        {
                                            if (_job.type != Job.JobType.NULL)
                                            {
                                                Logger.Log("(SCHEDULE) JOB [ID:" + _job.id + ", GUID:" + _job.guid + "] preparing for execution.", Logger.MessageType.INFORM);

                                                if (_node.ipRenewFlag)
                                                {
                                                    if (DateTime.Now.Subtract(_node.lastIPUpdate.Add(new TimeSpan(0, 0, 0, _fetchIPTime))).Milliseconds > 0)
                                                    {
                                                        // fetch ip or use old


                                                        // update note with new ip
                                                        Logger.Log("(SCHEDULE) NODE [ID:" + _node.id + ", GUID:" + _node.guid + "] fetched a new IP-Address: ", Logger.MessageType.INFORM);
                                                    }
                                                }

                                                _node.uWorker++;
                                                _job.state = 2;

                                                JobHolder _holder = new JobHolder();
                                                _holder.node = _node;
                                                _holder.job = _job;

                                                Logger.Log("(SCHEDULE) JOB [ID:" + _job.id + ", GUID:" + _job.guid + "] started execution.", Logger.MessageType.INFORM);

                                                JobThreadStart(_holder);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.Log("(Schedule) INTERNAL-EXECPTION: " + e.Message, Logger.MessageType.EMERGENCY);
                }

                if (_state == 2)
                    break;

                Thread.Sleep(_cycleTime);
            }
        }

        private bool JobTimeCheck(Job job, DateTime time)
        {
            if (job.time.delayed)
            {
                if (job.state == 1)
                {
                    job.time.delayed = false;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (job.time.type == JobTime.TimeMethod.Relative)
                {
                    if (job.time.jobDelay.CheckTime())
                    {
                        job.time.jobDelay.Reset();

                        if (job.state == 1)
                            return true;
                        else
                        {
                            // Job can't get executed at the moment,
                            // try at next cycle.
                            job.time.delayed = true;
                            return false;
                        }
                    }
                    else
                    {
                        job.time.jobDelay.SubtractFromDelaytime(_cycleTime);
                        return false;
                    }
                }
                else if (job.time.type == JobTime.TimeMethod.Absolute)
                {
                    JobTimeHandler _timeHandler = job.time.GetJobTimeHandler(time);

                    if (_timeHandler != null)
                    {
                        if (!_timeHandler.IsBlocked(time))
                        {
                            if (_timeHandler.CheckTime(time))
                            {
                                _timeHandler.minuteAtBlock = time.Minute;

                                if (job.state == 1)
                                    return true;
                                else
                                {
                                    // Job can't get executed at the moment,
                                    // try at next cycle.
                                    job.time.delayed = true;
                                    return false;
                                }
                            }
                        }

                        return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
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

            _job.tStart = DateTime.Now;
            try 
            {
                _job.Execute(_node.ip); 
            }
            catch (Exception)
            { 
                _job.state = 3; 
            }
            _job.tStop = DateTime.Now;
            _job.tSpan = _job.tStop.Subtract(_job.tStart);

            // Global OutDescriptors
            _job.outp.outputs[0].dataObject = _job.outp.outState.ToString();
            _job.outp.outputs[1].dataObject = _job.tSpan.Milliseconds;

            if (MadConf.conf.NOTI_ENABLE)
            {
                if (_job.notiFlag)
                {
                    List<JobRule> _bRules = GetBrokenRules(_job);
                    if (_bRules.Count != 0)
                    {
                        string _mailSubject = GenMailSubject(_job, "Job (target='" + _holder.node.ip.ToString()
                            + "') finished with a not expected result!");

                        string _mailContent = "";
                        _mailContent += "JobNode-GUID: '" + _node.guid + "'\n";
                        _mailContent += "JobNode-ID:   '" + _node.id + "'\n";
                        _mailContent += "JobNode-IP:   '" + _node.ip.ToString() + "'\n";
                        _mailContent += "JobNode-MAC:  '" + _node.mac.ToString() + "'\n\n";
                        _mailContent += GenJobInfo(_job);
                        _mailContent += GenBrokenRulesText(_job.outp, _bRules);

                        //NotificationGetParams.SetSendMail(_job.settings, _mailSubject, _mailContent);
                    }
                }
            }

            Logger.Log("(SCHEDULE) JOB [ID:" + _job.id + ", GUID:" + _job.guid + "] stopped execution.", Logger.MessageType.INFORM);

            _job.state = 1;
            _node.uWorker--;
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
            _buffer += "Job-GUID:     '" + job.guid + "'\n";
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
    }
}
