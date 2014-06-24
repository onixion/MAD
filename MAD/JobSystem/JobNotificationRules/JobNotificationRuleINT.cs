using System;

namespace MAD.jobSys
{
    public class JobNotificationRuleINT : JobNotificationRule
    {
        private int _trackValue;
        private int _expectedValue;

        private Operation _operation;
        public enum Operation { BiggerThan, SmallerThan, Equal, NotEqual }

        public JobNotificationRuleINT(int trackValue, Operation operation, int expectedValue)
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

                case Operation.BiggerThan:

                    if (_trackValue > _expectedValue)
                        return Result.RuleObserved;
                    else
                        return Result.RuleNotObserved;

                case Operation.SmallerThan:

                    if (_trackValue < _expectedValue)
                        return Result.RuleObserved;
                    else
                        return Result.RuleNotObserved;

                default:
                    return Result.NULL;
            }
        }
    }
}
