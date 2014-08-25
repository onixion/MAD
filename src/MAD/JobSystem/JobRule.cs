using System;
using System.Collections.Generic;

namespace MAD.JobSystemCore
{
    public class JobRule
    {
        private static int _idCount = 0;
        private object _idLock = new object();
        private int _id;
        public int id { get { return _id; } }

        public string outDescName { get; set; }
        public object compareValue;

        public enum Operation { Equal, NotEqual, Bigger, Smaller}
        public Operation oper;

        public JobRule() { }   

        public JobRule(string outDescName, object compareValue)
        {
            this.outDescName = outDescName;
            this.compareValue = compareValue;
        }

        public bool IsOperatorSupported(Type type)
        {
            if (type == typeof(string))
                if (oper == Operation.Equal || oper == Operation.NotEqual)
                    return true;
                else
                    return false;
            return true;
        }

        public bool CheckRuleValidity(JobOutput outp)
        {
            OutputDescriptor _desc = outp.GetOutputDesc(outDescName);
            if (_desc == null)
                throw new Exception("OutputDescriptor '" + outDescName + "' does not exist!");

            if(!IsOperatorSupported(_desc.dataType))
                throw new Exception("OutputDescriptor-Type not supported!");

            if (_desc.dataType == typeof(Int32))
                return CheckValidityInt((Int32)_desc.dataObject);
            else if (_desc.dataType == typeof(string))
                return CheckValidityString((string)_desc.dataObject);
            else
                return false;
        }

        private bool CheckValidityInt(int currentValue)
        {
            switch (oper)
            {
                case Operation.Equal:
                    if (currentValue == (Int32)compareValue)
                        return true;
                    break;
                case Operation.NotEqual:
                    if (currentValue != (Int32)compareValue)
                        return true;
                    break;
                case Operation.Bigger:
                    if (currentValue > (Int32)compareValue)
                        return true;
                    break;
                case Operation.Smaller:
                    if (currentValue < (Int32)compareValue)
                        return true;
                    break;
                default:
                    break;
            }
            return false;
        }

        private bool CheckValidityString(string currentValue)
        {
            switch (oper)
            { 
                case Operation.Equal:
                    if (currentValue == (string)compareValue)
                        return true;
                    break;
                case Operation.NotEqual:
                    if (currentValue != (string)compareValue)
                        return true;
                    break;
                default:
                    break;
            }

            return false; 
        }
    }
}
