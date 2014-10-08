﻿using System;
using System.Net;

using MAD.MacFinders;

namespace MAD.CLICore
{
    #region DHCPReader
    public class CatchBasicInfoStartCommand : Command
    {
        private DHCPReader _feeder;

        public CatchBasicInfoStartCommand(object[] args)
            : base()
        {
            _feeder = (DHCPReader)args[0];
            description = "Starts the Listener for Basic Information like MAC and IP, and how they are assigned.";
        }

        public override string Execute(int consoleWidth)
        {
            string _tmp = _feeder.Start();
            if (_tmp != null)
                return "<color><red>" + _tmp;
            else
                return "<color><blue>Start listening for Information";
        }
    }

    public class CatchBasicInfoStopCommand : Command
    {
        private DHCPReader _reader;

        public CatchBasicInfoStopCommand(object[] args)
            : base()
        {
            _reader = (DHCPReader)args[0];
            description = "Stops the listener for basic information like MAC and IP, and how they are assigend.";
        }

        public override string Execute(int consoleWidth)
        {
            string _tmp = _reader.Stop();
            if (_tmp == null)
                return "<color><blue>Ends";
            else
                return "<color><red>" + _tmp;
        }
    }

    public class CatchBasicInfoSetTimeIntervallCommand : Command
    {
        private DHCPReader _reader;
        private string paramDescripe = "Intervall in which the System should check. In seconds";

        public CatchBasicInfoSetTimeIntervallCommand(object[] args)
            : base()
        {
            _reader = (DHCPReader)args[0];
            rPar.Add(new ParOption("t", "INTERVALL-TIME IN SECONDS", paramDescripe, false, false, new Type[] { typeof(int) }));
            description = "Sets the time intevall in which the programm checks if the MACs still have the assigned IP or if they changed.";
        }

        public override string Execute(int consoleWidth)
        {
            int _time = 1000 * (int)pars.GetPar("t").argValues[0];
            uint _utime = Convert.ToUInt32(_time);
            _reader.ChangeCheckIntervall(_utime);
            return "<color><blue>Changed";
        }
    }

    public class CatchBasicInfoPrintHostsCommand : Command
    {
        private DHCPReader _reader;

        public CatchBasicInfoPrintHostsCommand(object[] args)
            : base()
        {
            _reader = (DHCPReader)args[0];
            description = "Prints all hosts which are currently in the List";
        }

        public override string Execute(int consoleWidth)
        {
            string _tmp = ModelHost.PrintLists();

            return "<color><blue>" + _tmp;
        }
    }
    #endregion

    #region ARPReader
    public class PrintArpReadyInterfaces : Command
    {
        private ARPReader _reader = new ARPReader();                              

        public PrintArpReadyInterfaces()
            : base()
        {
            description = "Prints all the interfaces who are ready for arp requesting (needed for the actual command)";
        }

        public override string Execute(int consoleWidth)
        {
            string _tmp = _reader.ListInterfaces();

            return "<color><blue>" + _tmp;
        }
    }

    public class ArpReaderStart : Command
    {
        private ARPReader _reader = new ARPReader();

        public ArpReaderStart()
            : base()
        {
            rPar.Add(new ParOption("i", "INTERFACE", "The Networkinterface to use, check with arp reader list interfaces for the right one", false, false, new Type[] { typeof(uint) }));
            rPar.Add(new ParOption("l", "SOURCE-IP", "The IPAddress of the used Network Interface", false, false, new Type[] { typeof(IPAddress) }));
            rPar.Add(new ParOption("s", "SUBNETMASK", "The Subnetmask of the Network. ie 16 for a /16 subnet", false, false, new Type[] { typeof(uint) }));
            rPar.Add(new ParOption("n", "NETWORK", "The Network Address. Something like 192.168.1.0", false, false, new Type[] { typeof(IPAddress) }));

            description = "Starts looking for hosts via ARP";
        }

        public override string Execute(int consoleWidth)
        {
            _reader.srcAddress = (IPAddress) pars.GetPar("l").argValues[0];
            _reader.networkInterface = (uint)pars.GetPar("i").argValues[0] - 1;
            _reader.netAddress = (IPAddress)pars.GetPar("n").argValues[0];
            _reader.subnetMask = (uint)pars.GetPar("s").argValues[0];

            _reader.Start();
            return "<color><blue>Successfully performed Scan";
        }
    }

    public class CheckList : Command
    {
        public CheckList()
            : base()
        {
            description = "Pings through the list of the Hosts manually. You only have to use this if didn't use dhcp reader command";
        }

        public override string Execute(int consoleWidth)
        {
            ModelHost.PingThroughList();
            return "<color><blue>Updated list";
        }
    }
    #endregion 
}
