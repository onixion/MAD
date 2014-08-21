using System;
using System.Xml.Serialization;

namespace MAD
{
    [Serializable()]
    public class JobRule : IXmlSerializable
    {
        public object obj;

        public object obj2;
        public Type type;
        public enum Operation { Equal, NotEqual, Bigger, Smaller}
        public Operation oper;

        public JobRule() { }

        public JobRule(Type type, Operation oper , object obj2)
        {
            this.type = type;
            this.oper = oper;
            this.obj2 = obj2;
        }

        public bool IsOperatorSupported()
        {
            if (type == typeof(string))
                if (oper == Operation.Equal || oper == Operation.NotEqual)
                    return true;
                else
                    return false;

            return true;
        }

        public bool CheckValidity()
        {
            if (type == typeof(Int32))
                return CheckValidityInt();
            else if (type == typeof(string))
                return CheckValidityString();
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
                    break;
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
                    break;
            }

            return false; 
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {

        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {

        }
    }
}
