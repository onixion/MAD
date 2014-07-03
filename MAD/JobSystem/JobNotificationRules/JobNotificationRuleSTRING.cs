using System;

namespace MAD.JobSystemCore
{
    public class JobNotificationRuleSTRING : JobNotificationRule
    {
        public Operation operation;
        public enum Operation { Equal, NotEqual, BiggerThan, SmallerThan }

        public string trackObject;
        public string operationValue;

        public JobNotificationRuleSTRING(string trackObject, Operation operation, string operationValue)
            : base(ObjectType.String)
        {
            this.trackObject = trackObject;
            this.operation = operation;
            this.operationValue = operationValue;
        }

        public override bool CheckRuleValidity()
        {
            switch (operation)
            {
                case Operation.Equal:

                    if (trackObject == operationValue)
                        return true;
                    else
                        return false;

                case Operation.NotEqual:

                    if (trackObject != operationValue)
                        return true;
                    else
                        return false;
                default:
                    return true;
            }
        }
    }
}
