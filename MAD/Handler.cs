using System;

using MAD.CLI;
using MAD.CLI.Server;
using MAD.JobSystem;

namespace MAD
{
    public class Handler
    {
        private static Handler _components;
        public static Handler components
        {
            get
            {
                if (_components == null)
                {
                    _components = new Handler();
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

        private Handler()
        {
            // create data-folder
            if (!System.IO.Directory.Exists(dataPath))
            {
                System.IO.Directory.CreateDirectory(dataPath);
            }

            jobSystem = new JobSystem.JobSystem(dataPath, 20);
            cli = new CLI.CLI();
            cliServer = new CLIServer(dataPath, 999);
        }
    }
}
