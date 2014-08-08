using System;

namespace MAD
{
    public class JobRule
    {
        public object obj;
        public Type type;
        public enum Operation { Equal, NotEqual, Bigger, Smaller}
        public Operation oper;
        public object obj2;

        public JobRule(ref object obj, Type type, Operation oper , object obj2)
        {
            this.obj = obj;
            this.type = type;
            this.oper = oper;
            this.obj2 = obj2;
        }

        public bool CheckValidity()
        {
            if (type == typeof(Int32))
            {
                return CheckValidityInt();
            }
            else if (type == typeof(string))
            {
                return CheckValidityString();
            }
            else
                throw new Exception("Type not supported!");
        }

        private bool CheckValidityInt()
        {
            switch (oper)
            {
                case Operation.Equal:
                    if ((Int32)obj == (Int32)obj2)
                        return true;
                    break;
                case Operation.NotEqual:
                    if ((Int32)obj != (Int32)obj2)
                        return true;
                    break;
                case Operation.Bigger:
                    if ((Int32)obj > (Int32)obj2)
                        return true;
                    break;
                case Operation.Smaller:
                    if ((Int32)obj < (Int32)obj2)
                        return true;
                    break;
                default:
                    throw new Exception("Operation not supported for this type!");
            }
            return false; 
        
        
        }

        private bool CheckValidityString()
        {
            switch (oper)
            { 
                case Operation.Equal:
                    if ((string)obj == (string)obj2)
                        return true;
                    break;
                case Operation.NotEqual:
                    if ((string)obj != (string)obj2)
                        return true;
                    break;
                default:
                    throw new Exception("Operation not supported for this type!");
            }
            return false; 
        }
    }
}
