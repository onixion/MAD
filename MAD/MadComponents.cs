using System;

using MAD.CLI;
using MAD.CLI.Server;
using MAD.JobSystem;

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

        public JobSystem.JobSystem jobSystem;
        public CLI.CLI cli;
        public CLIServer cliServer;

        // ----------------------------------
        //     DEFAULT CONSTRUCTORS
        // ----------------------------------

        private MadComponents()
        {
            if (!System.IO.Directory.Exists(dataPath))
            {
                System.IO.Directory.CreateDirectory(dataPath);
            }

            jobSystem = new JobSystem.JobSystem();

            cli = new CLI.CLI();
            cliServer = new CLIServer(dataPath, 999);
        }
    }
}
