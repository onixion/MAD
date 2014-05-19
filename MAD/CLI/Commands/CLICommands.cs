using System;

namespace MAD.CLI
{
    class HelpCommand : Command
    {
        public override string Execute()
        {
            output += " <---- H E L P ---- P A G E ---->\n\n";
            output += " <-- General commands -->\n\n";
            output += " > help                      print this help page\n";
            output += " > server-version            show server version\n";
            output += " > info                      informations about the cli-server\n";
            output += " > DISCONNECT                disconnect from server\n"; 
            output += " > cursor -t <NEW CURSOR>    change the CLI cursor\n\n";
            output += " <-- MadJobSystem commands -->\n";
            output += " > jobsystem status          status of the job-system\n";
            output += " > job status [-id <ID>]     status of the jobs\n";
            output += " > job add ping -n <JOB NAME> -ip <IPADDRESS> [-d <DELAY>] [-ttl <TTL>]\n";
            output += "                             add a ping-job\n";
            output += " > job add http -n <JOB NAME> -ip <IPADDRESS> [-d <DELAY>] [-p <PORT>]\n";
            output += "                             add a http-job\n";
            output += " > job add port -n <JOB NAME> -ip <IPADDRESS> [-d <DELAY>] [-p <PORT>]\n";
            output += "                             add a port-job\n";
            output += " > job remove -id <JOB ID>   remove a job\n";
            output += " > job start -id <JOB ID>    start a job\n";
            output += " > job stop -id <JOB ID>     stop a job\n";
            output += " <-- MadMemoryManagmentSystem commands -->\n";
            output += " <-- MadNotificationSystem commands -->\n";
            output += " <---- E N D ---->\n";

            return output;
        }
    }

    class VersionCommand : Command
    {
        public override string Execute()
        {
            output += "CLI-SERVER VERSION " + MadComponents.components.cliServer.version;
            return output;
        }
    }

    class InfoCommand : Command
    {
        public override string Execute()
        {
            output += "INFO TEXT COMING SOON";
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