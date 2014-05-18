using System;

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

        public const string dataPath = "data";

        public CLIServer cliServer;
        public JobSystem jobSystem;

        // ----------------------------------
        //     DEFAULT CONSTRUCTORS
        // ----------------------------------

        private MadComponents()
        {
            // create data-path
            if (System.IO.Directory.Exists(dataPath))
            {
                System.IO.Directory.CreateDirectory(dataPath);
            }

            // init components
            jobSystem = new JobSystem(dataPath);
            cliServer = new CLIServer(dataPath, 999);
        }

        // ----------------------------------
        //     GLOBAL METHODES
        // ----------------------------------
    }
}
