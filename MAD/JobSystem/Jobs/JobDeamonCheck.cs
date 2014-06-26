using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace MAD.jobSys
{
    class JobDeamonCheck : Job
    {
        #region members

		private string argument = "";
        private bool working; 

        #endregion

        #region constructors

        #endregion

        #region methods

		#region extern

		public override void Execute ()
		{
			switch (argument) 
			{
			case "dns":
				dnsCheck ();
				break;
			}
		}
       
		#endregion

		#region intern

		private void dnsCheck()
		{
            string name = Dns.GetHostName();
            if (name != null)
                working = true;
		}

		#endregion 

		#endregion
    }
}
