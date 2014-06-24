using System;

namespace MAD.jobSys
{
    public abstract class JobNotificationRule
    {
        public Result result = Result.NULL;
        public enum Result { NULL, RuleObserved, RuleNotObserved, Exception }

        public ObjectType type;
        public enum ObjectType { Int32, String, IPAddress }

        public JobNotificationRule(ObjectType type)
        {
            this.type = type;
        }

        public abstract bool CheckRuleValidity();
    }
}
