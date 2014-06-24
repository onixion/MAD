using System;

namespace MAD.jobSys
{
    public class JobNotificationRuleSTRING : JobNotificationRule
    {
        private string _trackValue;
        private string _expectedValue;

        private Operation _operation;
        public enum Operation { Equal, NotEqual }

        public JobNotificationRuleSTRING(string trackValue, Operation operation, string expectedValue)
        {
            _trackValue = trackValue;
            _operation = operation;
            _expectedValue = expectedValue;
        }

        public override Result CheckRule()
        {
            switch (_operation)
            {
                case Operation.Equal:

                    if (_trackValue == _expectedValue)
                        return Result.RuleObserved;
                    else
                        return Result.RuleNotObserved;

                case Operation.NotEqual:

                    if (_trackValue != _expectedValue)
                        return Result.RuleObserved;
                    else
                        return Result.RuleNotObserved;

                default:
                    return Result.NULL;
            }
        }
    }
}
