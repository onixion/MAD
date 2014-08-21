using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Serialization;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobPing : Job
    {
        #region members

        public int ttl = 200;
        public bool dontFragment = true;

        private Ping _ping = new Ping();

        #endregion

        #region constructor

        public JobPing()
            : base(JobType.Ping)
        { }

        public JobPing(string name)
            : base(name, JobType.Ping) 
        {
            this.outDesc.Add(new OutputDesc("DeltaTime", typeof(string)));
        }

        public JobPing(string name, JobType type, int ttl)
            : base (name, type)
        {
            this.ttl = ttl;
            this.outDesc.Add(new OutputDesc("DeltaTime", typeof(string)));
        }

        public JobPing(string name, JobType type, JobTime time, JobNotification noti, int ttl)
            : base(name, type, time, noti)
        {
            this.ttl = ttl;
        }

        #endregion

        #region methodes

        public override void Execute(IPAddress targetAddress)
        {
            PingOptions _pingOptions = new PingOptions(ttl, dontFragment);

            try
            {
                PingReply _reply = _ping.Send(targetAddress, 5000, Encoding.ASCII.GetBytes("1111111111111111"), _pingOptions);

                if (_reply.Status == IPStatus.Success)
                {
                    outState = OutState.Success;
                    SetOutput("DeltaTime", _reply.RoundtripTime.ToString("ssss"));
                }
                else
                {
                    outState = OutState.Failed;
                    SetOutput("DeltaTime", _reply.RoundtripTime.ToString("----"));
                }
            }
            catch (Exception)
            {
                outState = OutState.Exception;
            }
        }

        protected override string JobStatus()
        {
            string _temp = "";

            _temp += "<color><yellow>TTL: <color><white>" + ttl.ToString() + "\n";

            return _temp;
        }

        public override void ReadXmlJobSpec(System.Xml.XmlReader reader)
        {
            if (reader.Read() && reader.Name == "TTL")
                ttl = Int32.Parse(reader.GetAttribute("value"));
        }

        public override void WriteXmlJobSpec(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("TTL");
            writer.WriteAttributeString("value", ttl.ToString());
            writer.WriteEndElement();
        }

        #endregion
    }
}
