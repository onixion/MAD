using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.NetworkInformation;

using MAD.JobSystem;

namespace MAD.HostDetect
{
    public class HostDetect : Job
    {
        public List<IPAddress> Detect(string Net, string Subnetmask)
        {
            IPAddress toDetect = IPAddress.Parse(Net);
            IPAddress Mask = IPAddress.Parse(Subnetmask);

            //Needs NetworkHelper
        }

    }
}
