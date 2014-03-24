using System;
using System.Collections.Generic;

namespace MAD
{
    class MadCLI
    {
        public string cursor = "=> ";
        private string cliInput;

        public string windowTitle = "MAD - Network Monitoring";

        public MadCLI()
        { 

        }

        public void UpdateWindowTitle()
        {
            Console.Title = windowTitle;
        }

        /// <summary>
        /// Start CLI
        /// </summary>

        public void Start()
        {
            while (true)
            {
                Console.Write(cursor);
                cliInput = Console.ReadLine();

                GetArgs(cliInput);
                // get args

                // check args

                // execute command
            }
        }

        /// <summary>
        /// Get Arguments
        /// </summary>
        /// <param name="inputCLI"></param>
        /// <returns>Returns all arguments in a list with string[]</returns>

        public List<string[]> GetArgs(string inputCLI)
        {
            List<string[]> temp = new List<string[]>();
            string[] temp2 = inputCLI.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < temp2.Length; i++)
            {
                if (temp2[i].Split('-').Length == 2)
                {
                    temp.Add(new String[] { temp2[i], temp2[i + 1] });
                }
            }
            return temp;
        }

        /// <summary>
        /// Print the MAD-Logo on the CLI
        /// </summary>

        public void PrintHeader()
        { 
        
        }
    }
}
