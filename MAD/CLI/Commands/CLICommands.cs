using System;
using System.Net;

namespace MAD
{
    class HelpCommand : Command
    {
        public HelpCommand() {}

        public override string Execute()
        {
            output = " <---- H E L P ---- P A G E ---->\n";
            
            /*Console.WriteLine();
            Headline(" <-- General commands -->");
            Console.WriteLine();
            Console.WriteLine(" > help                      print this help page");
            Console.WriteLine(" > versions                  show versions");
            Console.WriteLine(" > clear                     clear console");
            Console.WriteLine(" > logo                      print the MAD logo");
            Console.WriteLine(" > exit/close                exit program");
            Console.WriteLine(" > info                      informations about the program");
            Console.WriteLine(" > cursor -t <NEW CURSOR>    change the CLI cursor");
            Console.WriteLine(" > gui                       start the GUI (NIY)");                
            Console.WriteLine();
            Headline(" <-- MadJobSystem commands -->");
            Console.WriteLine();
            Console.WriteLine(" > jobsystem status          status of the job-system");
            Console.WriteLine(" > job status [-id <ID>]     status of the jobs");
            Console.WriteLine(" > job add ping -n <JOB NAME> -ip <IPADDRESS> -d <DELAY> -ttl <TTL>");
            Console.WriteLine("                             add a ping-job");
            Console.WriteLine(" > job add http -n <JOB NAME> -ip <IPADDRESS> -d <DELAY> -p <PORT>");
            Console.WriteLine("                             add a http-job");
            Console.WriteLine(" > job add port -n <JOB NAME> -ip <IPADDRESS> -d <DELAY> -p <PORT>");
            Console.WriteLine("                             add a port-job");
            Console.WriteLine(" > job remove -id <JOB ID>   remove a job");
            Console.WriteLine(" > job start -id <JOB ID>    start a job");
            Console.WriteLine(" > job stop -id <JOB ID>     stop a job");
            Console.WriteLine();
            Headline(" <-- MadMemoryManagmentSystem commands -->");
            Console.WriteLine();
            Headline(" <-- MadNotificationSystem commands -->");
            Console.WriteLine();
            StartEndHeadline(" <---- E N D ---->");
            Console.WriteLine();*/

            return output;
        }


    }

    class ExitCommand : Command
    {
        public ExitCommand() {}

        public override string Execute()
        {
            Environment.Exit(0);

            return output;
        }
    }

    class VersionsCommand : Command
    {
        public VersionsCommand() {}

        public override string Execute()
        {
            output += "Mad-Project                  VERSION " + MadComponents.components.version + "\n";
            output += "MadNotificationSystem        VERSION " + "unknown\n";
            output += "MadJobSystem                 VERSION " + MadComponents.components.jobSystem.version + "\n";
            output += "MadMemoryManagmentSysystem   VERSION " + "unknown\n";

            return output;
        }
    }

    class InfoCommand : Command
    {
        public InfoCommand() { }

        public override string Execute()
        {
            return output;
        }
    }

    class CursorCommand : Command
    {
        public CursorCommand()
        {
            requiredParameter.Add(new ParameterOption("t", false, typeof(String)));
        }

        public override string Execute()
        {
            //MadComponents.components.cli.cursor = (String)parameters.GetParameter("t").value;

            return output;
        }
    }
}