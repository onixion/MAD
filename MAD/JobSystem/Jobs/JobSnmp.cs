using System;
using System.Net;

using MAD.Helper;

using SnmpSharpNet;

namespace MAD.JobSystemCore
{
    [Serializable]
    public class JobSnmp : Job
    {
        public uint version;
        public uint expectedHostNr = 10;

        public bool interfaceParUsed; 

        public string communityString = "";
        public string auth = "";
        public string priv = "";
        public string ifEntryNr;

        public NetworkHelper.snmpProtokolls privProt;
        public NetworkHelper.snmpProtokolls authProt;

        public NetworkHelper.securityLvl security;

        private string ifEntryString = "1.3.6.1.2.1.2.2.1";
        private static uint lastRecord = 0; 

        public override void Execute(IPAddress targetAddress)
        {
            UdpTarget target = new UdpTarget(targetAddress, 161, 5000, 3);
            switch (version)
            {
                case 1:
                    //Not Supportet
                    break;
                case 2:
                    SnmpV2Handling(target);
                    break;
                case 3:
                    SnmpV3Handling(target);
                    break;
            }
            target.Close();
        }

        private void SnmpV2Handling(UdpTarget target)
        {
            OctetString community = new OctetString(communityString);
            AgentParameters param = new AgentParameters(community);

            param.Version = SnmpVersion.Ver2;

            ExecuteRequest(target, param);
        }

        private void SnmpV3Handling(UdpTarget target)
        {
            SecureAgentParameters param = new SecureAgentParameters();

            if (!target.Discovery(param))
            {										
                Console.WriteLine("fail");
            }
            else
            {
                Console.WriteLine("win");
            }

            switch (security)
            {
                case NetworkHelper.securityLvl.noAuthNoPriv:
                    param.noAuthNoPriv(communityString);

                    break;
                case NetworkHelper.securityLvl.authNoPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5)
                        param.authNoPriv(communityString, AuthenticationDigests.MD5, auth);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA)
                        param.authNoPriv(communityString, AuthenticationDigests.SHA1, auth);

                    break;
                case NetworkHelper.securityLvl.authPriv:
                    if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.AES)
                        param.authPriv(communityString, AuthenticationDigests.MD5, auth, PrivacyProtocols.AES128, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.MD5 && privProt == NetworkHelper.snmpProtokolls.DES)
                        param.authPriv(communityString, AuthenticationDigests.MD5, auth, PrivacyProtocols.DES, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.AES)
                        param.authPriv(communityString, AuthenticationDigests.SHA1, auth, PrivacyProtocols.AES128, priv);
                    else if (authProt == NetworkHelper.snmpProtokolls.SHA && privProt == NetworkHelper.snmpProtokolls.DES)
                        param.authPriv(communityString, AuthenticationDigests.SHA1, auth, PrivacyProtocols.DES, priv);

                    break;
            }

            ExecuteRequest(target, param);
        }

        private void ExecuteRequest(UdpTarget target, AgentParameters param)
        {
            if (interfaceParUsed)
            {
                Oid ifIndex = new Oid(ifEntryString + ".1");
                Oid ifDescr = new Oid(ifEntryString + ".2");
                Oid ifType = new Oid(ifEntryString + ".3");

                Pdu index = new Pdu(PduType.GetBulk);
                index.VbList.Add(ifIndex);
                index.MaxRepetitions = (int)expectedHostNr;

                Pdu descr = new Pdu(PduType.GetBulk);
                descr.VbList.Add(ifDescr);
                descr.MaxRepetitions = (int)expectedHostNr;

                Pdu type = new Pdu(PduType.GetBulk);
                type.VbList.Add(ifType);
                type.MaxRepetitions = (int)expectedHostNr;

                try
                {
                    SnmpV2Packet indexResult = (SnmpV2Packet)target.Request(index, param);
                    SnmpV2Packet descrResult = (SnmpV2Packet)target.Request(descr, param);
                    SnmpV2Packet typeResult = (SnmpV2Packet)target.Request(type, param);

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
                catch (Exception)
                { }
            }
            else
            {
                string outOctetsString = "1.3.6.1.2.1.2.2.1.16";

                Oid outOctets = new Oid(outOctetsString + "." + ifEntryNr);

                Pdu octets = new Pdu(PduType.Get);
                octets.VbList.Add(outOctets);

                try
                {
                    SnmpV2Packet octetsResult = (SnmpV2Packet)target.Request(octets, param);

                    uint result = (uint)Convert.ToUInt64(octetsResult.Pdu.VbList[0].Value.ToString()) - lastRecord;
                    lastRecord = (uint)Convert.ToUInt64(octetsResult.Pdu.VbList[0].Value.ToString());

                    if (octetsResult.Pdu.ErrorStatus != 0)
                    {
                        Console.WriteLine("An Error Occured");
                    }
                    else
                    {
                        Console.WriteLine("Output since the last run: {0}", result.ToString());
                    }
                }
                catch (Exception)
                { }
            }
        }

        private void ExecuteRequest(UdpTarget target, SecureAgentParameters param)
        {
            if (interfaceParUsed)
            {
                Oid ifIndex = new Oid(ifEntryString + ".1");
                Oid ifDescr = new Oid(ifEntryString + ".2");
                Oid ifType = new Oid(ifEntryString + ".3");

                Pdu index = new Pdu(PduType.GetBulk);
                index.VbList.Add(ifIndex);
                index.MaxRepetitions = (int)expectedHostNr;

                Pdu descr = new Pdu(PduType.GetBulk);
                descr.VbList.Add(ifDescr);
                descr.MaxRepetitions = (int)expectedHostNr;

                Pdu type = new Pdu(PduType.GetBulk);
                type.VbList.Add(ifType);
                type.MaxRepetitions = (int)expectedHostNr;

                try
                {
                    SnmpV3Packet indexResult = (SnmpV3Packet)target.Request(index, param);
                    SnmpV3Packet descrResult = (SnmpV3Packet)target.Request(descr, param);
                    SnmpV3Packet typeResult = (SnmpV3Packet)target.Request(type, param);

                    if (indexResult.ScopedPdu.ErrorStatus != 0 || descrResult.ScopedPdu.ErrorStatus != 0 || typeResult.ScopedPdu.ErrorStatus != 0)
                    {
                        Console.WriteLine("An Error Occured");
                    }
                    else
                    {
                        for (int i = 0; i < indexResult.ScopedPdu.VbCount; i++)
                        {
                            Console.WriteLine("Interface {0}: Type {1} -> {2}", indexResult.ScopedPdu.VbList[i].Value.ToString(), typeResult.ScopedPdu.VbList[i].Value.ToString(), descrResult.ScopedPdu.VbList[i].Value.ToString());
                        }
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                string outOctetsString = "1.3.6.1.2.1.2.2.1.16";

                Oid outOctets = new Oid(outOctetsString + "." + ifEntryNr);

                Pdu octets = new Pdu(PduType.Get);
                octets.VbList.Add(outOctets);

                try
                {
                    SnmpV3Packet octetsResult = (SnmpV3Packet)target.Request(octets, param);

                    uint result = (uint)Convert.ToUInt64(octetsResult.ScopedPdu.VbList[0].Value.ToString()) - lastRecord;
                    lastRecord = (uint)Convert.ToUInt64(octetsResult.ScopedPdu.VbList[0].Value.ToString());

                    if (octetsResult.ScopedPdu.ErrorStatus != 0)
                    {
                        Console.WriteLine("An Error Occured");
                    }
                    else
                    {
                        Console.WriteLine("Output since the last run: {0}", result.ToString());
                    }
                }
                catch (Exception)
                { }
            }
        }
    }
}
