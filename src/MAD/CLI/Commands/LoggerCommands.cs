using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MAD.Logging;

namespace MAD.CLICore
{
    class ChangePathFile : Command
    {
        public ChangePathFile()
        {
            rPar.Add(new ParOption("p", "PATH", "Relativ Path to the Logfile in Unix Style", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            string _additionToPath = (string) pars.GetPar("p").argValues[0];

            try
            {
                Logger.CreateNewPathFile(_additionToPath);
                Logger.Log("Changed Path to Log file successfully", Logger.MessageType.INFORM);
                return "Successfully changed path to log file";
            }
            catch (Exception)
            {
                Logger.Log("Error by changing Path File. Could Couse no logfile!", Logger.MessageType.EMERGENCY);
                return "Error! Did you use Unix Paths?";
            }
        }
    }

    class ChangeBufferSize : Command
    {
        public ChangeBufferSize()
        {
            rPar.Add(new ParOption("s", "SIZE", "The new size of the Buffer, has to be changed every reboot", false, false, new Type[] { typeof(uint) }));
        }

        public override string Execute(int consoleWidth)
        {
            uint _newSize = (uint)pars.GetPar("s").argValues[0];

            Logger.buffer = _newSize;
            Logger.Log("Successfully changed buffersize", Logger.MessageType.INFORM);

            return "Successfully changed buffer size"; 
        }
    }

    class ChangeLogFileName : Command
    {
        public ChangeLogFileName()
        {
            rPar.Add(new ParOption("n", "NAME", "The name of the Logfile, has to be changed every reboot", false, false, new Type[] { typeof(string) }));
        }

        public override string Execute(int consoleWidth)
        {
            string _newName = (string)pars.GetPar("n").argValues[0];

            Logger.logFileName = _newName;
            Logger.Log("Successfully changed name of Logfile", Logger.MessageType.INFORM);

            return "Successfully changed name of Logfile";
        }
    }
}
