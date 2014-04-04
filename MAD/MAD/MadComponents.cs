using System;
using System.Reflection;

namespace MAD
{
    public class MadComponents
    {
        private static MadComponents _components;
        public static MadComponents components
        {
            get 
            {
                if (_components == null)
                    _components = new MadComponents();
                return _components;
            }
        }

        private MadComponents()
        {
            cli = new MadCLI();
            jobSystem = new MadJobSystem();
        }

        public string version { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }
        public MadCLI cli;
        public MadJobSystem jobSystem;
    }
}
