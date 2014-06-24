using System;

namespace MAD.jobSys
{
    public abstract class JobNotificationRule
    {
        public enum Result { NULL, RuleObserved, RuleNotObserved }

        public abstract Result CheckRule();
    }
}
