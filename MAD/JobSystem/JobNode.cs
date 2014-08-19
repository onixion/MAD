using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Xml.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable()]
    public class JobNode : IXmlSerializable
    {
        #region members

        public object nodeLock = new object();

        private static int _nodesCount = 0;
        private static object _idLock = new object();
        private int _id;
        public int id { get { return _id; } }

        public enum State { Active, Inactive, Exception }
        public State state = State.Inactive;

        public List<Job> jobs = new List<Job>();
        public const int MAX_JOBS = 100;
        
        public string name { get; set; }
        public PhysicalAddress macAddress { get; set; } // sns
        public IPAddress ipAddress { get; set; } // sns
        public JobNotification defaultNoti { get; set; }

        #endregion

        #region constructors

        public JobNode()
        {
            InitID();
        }

        public JobNode(string nodeName, PhysicalAddress macAddress, IPAddress ipAddress, List<Job> jobs, JobNotification defaultNoti)
        {
            InitID();
            this.name = nodeName;
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            this.jobs = jobs;
            this.defaultNoti = defaultNoti;
        }

        #endregion

        #region methodes

        private void InitID()
        {
            lock (_idLock)
            {
                _id = _nodesCount;
                _nodesCount++;
            }
        }

        #endregion

        #region for xml-serialization only

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            ipAddress = IPAddress.Parse(reader.GetAttribute("IPAddress"));
            macAddress = PhysicalAddress.Parse(reader.GetAttribute("MacAddress"));
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("IPAddress", ipAddress.ToString());
            writer.WriteAttributeString("MacAddress", macAddress.ToString());
        }

        #endregion
    }
}
