﻿using System;

namespace MAD
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            { 
                //---------------------------------------------
                // TEST YOUR SHIT IN HERE
                MadComponents.components.cli.Start();
                //---------------------------------------------
            }
            else if (args.Length >= 1)
            {
                switch (args[0])
                {
                    case "-console":
                        MadComponents.components.cli.Start();
                        break;
                    default:
                        Console.WriteLine("ERROR! Argument '" + args[0] + "' not known!");
                        Console.WriteLine("Press any key to close ...");
                        Console.ReadKey();
                        return 1;
                }
            }
            LOLWASGEHT
            return 0;
        }
    }
}
