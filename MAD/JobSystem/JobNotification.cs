using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Xml.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable()]
    public class JobNotification : IXmlSerializable
    {
        #region members

        public MailAddress[] mailAddr { get; set; }
        public MailPriority priority { get; set; }
        public List<JobRule> rules = new List<JobRule>();

        #endregion

        #region constructors

        public JobNotification() 
        {
            mailAddr = new MailAddress[0];
            priority = MailPriority.High;
        }

        public JobNotification(MailAddress[] mailAddr)
        {
            this.mailAddr = mailAddr;
        }

        public JobNotification(MailAddress[] mailAddr, MailPriority priority) 
        {
            this.mailAddr = mailAddr;
            this.priority = priority;
        }

        #endregion

        #region methodes

        public List<JobRule> GetBrokenRules()
        {
            List<JobRule> _buffer = new List<JobRule>();
            foreach (JobRule _temp in rules)
                if (!_temp.CheckValidity())
                    _buffer.Add(_temp);
            return _buffer;
        }

        #endregion

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
