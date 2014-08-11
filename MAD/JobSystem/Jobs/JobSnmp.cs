using System;
using System.Net;

using SnmpSharpNet;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobSnmp : Job
    {
        public uint version;
        public uint ifEntryNr;

        public bool interfaceParUsed; 

        public string communityString = "";
        public string auth = "";
        public string priv = "";

        public enum snmpProtokolls
        {
            MD5,
            SHA,
            AES,
            DES
        }

        public enum securityLvl
        {
            noAuthNoPriv,
            authNoPriv,
            authPriv
        }

        private string ifEntryString = "1.3.6.1.2.1.2.2.1";

        public override void Execute(IPAddress targetAddress)
        {
            UdpTarget target = new UdpTarget(targetAddress, 161, 5000, 3);
            switch (version)
            {
                case 1:
                    SnmpV1Handling(target);
                    break;
                case 2:
                    SnmpV2Handling(target);
                    break;
                case 3:
                    SnmpV3Handling(target);
                    break;
            }

        }

        private void SnmpV1Handling(UdpTarget target)
        {
            OctetString community = new OctetString(communityString);
            AgentParameters param = new AgentParameters(community);

            param.Version = SnmpVersion.Ver1;

            if (interfaceParUsed)
            {
                Oid ifIndex = new Oid(ifEntryString + ".1");
                Oid ifDescr = new Oid(ifEntryString + ".2");
                Oid ifType = new Oid(ifEntryString + ".3");

                Pdu index = new Pdu(PduType.GetBulk);
                index.VbList.Add(ifIndex);

                Pdu descr = new Pdu(PduType.GetBulk);
                descr.VbList.Add(ifDescr);

                Pdu type = new Pdu(PduType.GetBulk);
                type.VbList.Add(ifType);
                try
                {
                    SnmpV1Packet indexResult = (SnmpV1Packet)target.Request(index, param);
                    SnmpV1Packet descrResult = (SnmpV1Packet)target.Request(descr, param);
                    SnmpV1Packet typeResult = (SnmpV1Packet)target.Request(type, param);

                    if (indexResult.Pdu.ErrorStatus != 0 || descrResult.Pdu.ErrorStatus != 0 || typeResult.Pdu.ErrorStatus != 0)
                    {
                        Console.WriteLine("An Error Occured");
                    }
                    else
                    {
                        for (int i = 0; i < indexResult.Pdu.VbCount; i++)
                        {
                            Console.WriteLine("Interface {0}: Type {1} -> {2}", indexResult.Pdu.VbList[i].Value.ToString(), typeResult.Pdu.VbList[i].Value.ToString(), descrResult.Pdu.VbList[i].Value.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                }

               
            }

        }

        private void SnmpV2Handling(UdpTarget target)
        { 
        
        }

        private void SnmpV3Handling(UdpTarget target)
        {
        
        }
    }
}
