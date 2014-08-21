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
