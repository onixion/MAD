using System;

namespace MAD.CLI
{
    class HelpCommand : Command
    {
        public override string Execute()
        {
            output += "<color><yellow><---- H E L P ---- P A G E ---->\n\n";
            output += "<color><darkyellow><-- General commands -->\n\n";
            output += "<color><white>  help                      <color><gray>print this help page\n\n";
            output += "<color><white>  server-version            <color><gray>show server version\n\n";
            output += "<color><white>  info                      <color><gray>informations about the cli-server\n\n";
            output += "<color><white>  cursor -t <NEW CURSOR>    <color><gray>change the CLI cursor\n\n";
            output += "<color><darkyellow><-- MadJobSystem commands -->\n\n";
            output += "<color><white>  jobsystem status          <color><gray>status of the job-system\n\n";
            output += "<color><white>  job status [-id <ID>]     <color><gray>status of the jobs\n";
            output += "<color><white>  job add ping -n <JOB NAME> -ip <IPADDRESS> [-d <DELAY>] [-ttl <TTL>]\n";
            output += "                            <color><gray>add a ping-job\n\n";
            output += "<color><white>  job add http -n <JOB NAME> -ip <IPADDRESS> [-d <DELAY>] [-p <PORT>]\n";
            output += "                            <color><gray>add a http-job\n\n";
            output += "<color><white>  job add port -n <JOB NAME> -ip <IPADDRESS> [-d <DELAY>] [-p <PORT>]\n";
            output += "                            <color><gray>add a port-job\n\n";
            output += "<color><white>  job remove -id <JOB ID>   <color><gray>remove a job\n\n";
            output += "<color><white>  job start -id <JOB ID>    <color><gray>start a job\n\n";
            output += "<color><white>  job stop -id <JOB ID>     <color><gray>stop a job\n\n";
            output += "<color><darkyellow><-- MadMemoryManagmentSystem commands -->\n\n";
            output += "<color><darkyellow><-- MadNotificationSystem commands -->\n\n";
            output += "<color><yellow><---- E N D ---->\n";

            return output;
        }
    }

    class InfoCommand : Command
    {
        public override string Execute()
        {
            output += "<color><red>MAD - Network Monitoring v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n";
            output += "Components:<color><yellow>\n";
            output += "CLI         v" + MadComponents.components.cli.version.ToString() + " (CLI-Framework v" + MadComponents.components.cli.versionFramework + ")\n";
            output += "CLI-Server  v" + MadComponents.components.cliServer.version.ToString() + "\n";
            output += "JobSystem   v" + MadComponents.components.jobSystem.version.ToString() + "\n";
            return output;
        }
    }

    class ColorTest : Command
    {
        public override string Execute()
        {
            //output += "<color><gray>" + MadComponents.components.cli.cliWriter.colors.Count + " colors supported.\n";
            /*
            foreach(object[] temp in MadComponents.components.cli.cliWriter.colors)
            {
                output += "<color>" + (string) temp[0] + (string) temp[0] + "\n";
            }
            */
            return output;
        }
    }
}