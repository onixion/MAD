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
            ipAddress = IPAddress.Parse(reader.GetAttribute("IP"));
            macAddress = PhysicalAddress.Parse(reader.GetAttribute("MAC"));
            name = reader.GetAttribute("Name");

            if (reader.Read() && reader.Name == "Jobs")
                while (reader.Read() && reader.Name == "Job")
                {
                    // not nice but works for now ...

                    Job _job = null;
                    Job.JobType _type = (Job.JobType)Enum.Parse(typeof(Job.JobType), reader.GetAttribute("Type"));

                    switch (_type)
                    { 
                        case Job.JobType.Ping:
                            _job = new JobPing();
                            _job.ReadXml(reader);
                            break;

                            // ...

                        default:
                            break;
                    }
                    _job.type = _type;

                    jobs.Add(_job);
                }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("IP", ipAddress.ToString());
            writer.WriteAttributeString("MAC", macAddress.ToString());
            writer.WriteAttributeString("Name", name);
            writer.WriteStartElement("Jobs");
            foreach (Job _job in jobs)
                _job.WriteXml(writer);
            writer.WriteEndElement();
        }

        #endregion
    }
}
