﻿using System;

using System.Threading;

namespace MAD
{
    class Program
    {
        static void Main(string[] args)
        {
            // IMPORTANT!
            // WHEN YOU PUSH THIS TO GITHUB, MAKE SURE THE MAIN METHOD (THIS ON HERE) IS EMPTY.
            // IF THIS IS NOT EMPTY, THE MERGE PROCESS COULD CAUSE AN ERROR. TO AVOID THAT JUST
            // KEEP THIS METHOD HERE CLEAN. THANKS!

            MadComponents.components.cli.Start();
        }
    }
}