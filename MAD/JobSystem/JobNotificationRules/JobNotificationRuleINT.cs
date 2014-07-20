using System;

namespace MAD.JobSystemCore
{
    public class JobNotificationRuleINT : JobNotificationRule
    {
        public Operation operation;
        public enum Operation { Equal, NotEqual, BiggerThan, SmallerThan };

        public int trackObject;
        public int operationValue;

        public JobNotificationRuleINT(int trackObject, Operation operation, int operationValue)
            : base(ObjectType.Int32)
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

                case Operation.BiggerThan:

                    if (trackObject > operationValue)
                        return true;
                    else
                        return false;

                case Operation.SmallerThan:

                    if (trackObject < operationValue)
                        return true;
                    else
                        return false;
                default:
                    return true;
            }
        }
    }
}
