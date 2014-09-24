using System;
using MAD.DHCPReader;
using MAD.MacFinders;

namespace MAD.CLICore
{
    #region DHCPReader
    public class CatchBasicInfoStartCommand : Command
    {
        private MACFeeder _feeder;

        public CatchBasicInfoStartCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
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
        private MACFeeder _feeder;

        public CatchBasicInfoStopCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
            description = "Stops the listener for basic information like MAC and IP, and how they are assigend.";
        }

        public override string Execute(int consoleWidth)
        {
            string _tmp = _feeder.Stop();
            if (_tmp == null)
                return "<color><blue>Ends";
            else
                return "<color><red>" + _tmp;
        }
    }

    public class CatchBasicInfoSetTimeIntervallCommand : Command
    {
        private MACFeeder _feeder;
        private string paramDescripe = "Intervall in which the System should check. In seconds";

        public CatchBasicInfoSetTimeIntervallCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
            rPar.Add(new ParOption("t", "INTERVALL-TIME IN SECONDS", paramDescripe, false, false, new Type[] { typeof(int) }));
            description = "Sets the time intevall in which the programm checks if the MACs still have the assigned IP or if they changed.";
        }

        public override string Execute(int consoleWidth)
        {
            int _time = 1000 * (int)pars.GetPar("t").argValues[0];
            uint _utime = Convert.ToUInt32(_time);
            _feeder.ChangeCheckIntervall(_utime);
            return "<color><blue>Changed";
        }
    }

    public class CatchBasicInfoPrintHostsCommand : Command
    {
        private MACFeeder _feeder;

        public CatchBasicInfoPrintHostsCommand(object[] args)
            : base()
        {
            _feeder = (MACFeeder)args[0];
            description = "Prints all hosts which are currently in the List";
        }

        public override string Execute(int consoleWidth)
        {
            string _tmp = _feeder.PrintLists();

            return "<color><blue>" + _tmp;
        }
    }
    #endregion

    #region ARPReader
    public class PrintArpReadyInterfaces : Command
    {
        private ARPReader _reader;                                          //alin fragen wegen erben von macfeeder warum und wieso und waht the fak 

    }
    #endregion 
}
