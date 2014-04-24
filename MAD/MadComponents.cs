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

        // ----------------------------------
        //     GLOBAL OBJECTS
        // ----------------------------------

        public Version version = new Version(0, 5, 3500);

        public CLIServer cliServer;
        public JobSystem jobSystem;

        // ----------------------------------
        //     DEFAULT CONSTRUCTORS
        // ----------------------------------

        private MadComponents()
        {
            jobSystem = new JobSystem();
            cliServer = new CLIServer(999);
        }

    }
}
