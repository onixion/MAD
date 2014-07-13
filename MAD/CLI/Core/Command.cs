using System;
using System.Collections.Generic;

namespace MAD.CLICore
{
    public abstract class Command
    {
        #region members

        private List<ParOption> _rPar;
        public List<ParOption> rPar { get { return _rPar; } }
       
        private List<ParOption> _oPar;
        public List<ParOption> oPar { get { return _oPar; } }

        public string description { get; set; }
        protected string output = "";

        // This object contains all pars given by the cli.
        public ParInput pars = new ParInput();

        #endregion

        #region constructor

        protected Command()
        {
            _rPar = new List<ParOption>();
            _oPar = new List<ParOption>();
        }

        #endregion

        #region methodes

        public abstract string Execute(int consoleWidth);

        public Type GetArgType(string par)
        {
            foreach (Parameter _temp in pars.pars)
            {
                if (_temp.par == par)
                {
                    return _temp.argValues[0].GetType();
                }
            }

            return null;
        }

        protected bool OParUsed(string par)
        {
            foreach (Parameter _temp in pars.pars)
            {
                if ((string)_temp.par == par)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
