using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MAD.Logging;

namespace MAD.CLICore
{
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
}
