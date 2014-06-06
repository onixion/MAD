using System;

using MAD.CLI;
using MAD.CLI.Server;
using MAD.JobSystem;

namespace MAD
{
    public class MadComponents
    {LOLWASGEHT
        private static MadComponents _components;
        public static MadComponents components
        {
            get
            {
                if (_components == null)
                {
                    _components = new MadComponents();
                }
  
                return _components;
            }
        }

        // ----------------------------------
        //     GLOBAL OBJECTS
        // ----------------------------------

        public const string dataPath = "data";

        public CLI.CLI cli;
        public CLIServer cliServer;
        public JobSystem.JobSystem jobSystem;

        // ----------------------------------
        //     DEFAULT CONSTRUCTORS
        // ----------------------------------

        private MadComponents()
        {
            // create data-path
            if (!System.IO.Directory.Exists(dataPath))
            {
                System.IO.Directory.CreateDirectory(dataPath);
            }

            // init components
            cli = new CLI.CLI();
            jobSystem = new JobSystem.JobSystem();
            cliServer = new CLIServer(dataPath, 999);
        }
    }
}
