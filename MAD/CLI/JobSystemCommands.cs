using System;
using System.Net;

namespace MAD
{
    public class JobSystemStatusCommand : Command
    {
        ConsoleTable jobTable;
        string[] tableTitle = new string[] { "ID", "Name", "Type", "Active", "IP-Address", "Delay", "Output" };

        public JobSystemStatusCommand() { }

        public override void Execute()
        {
            jobTable = new ConsoleTable(tableTitle);
            tableTitle = jobTable.FormatStringArray(tableTitle);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("MadJobSystem v" + MadComponents.components.jobSystem.version);
            Console.ForegroundColor = MadComponents.components.cli.textColor;
            Console.WriteLine();
            Console.WriteLine("Jobs initialized: " + MadComponents.components.jobSystem.jobs.Count);
            Console.WriteLine("Jobs active:      " + MadComponents.components.jobSystem.JobsActive());
            Console.WriteLine("Jobs inactive:    " + MadComponents.components.jobSystem.JobsInactive());
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            jobTable.WriteColumnes(tableTitle);
            Console.WriteLine();
            Console.WriteLine(jobTable.splitline);
            Console.ForegroundColor = MadComponents.components.cli.textColor;

            foreach (Job job in MadComponents.components.jobSystem.jobs)
            {
                string[] array = new string[tableTitle.Length];
                array[0] = job.jobID.ToString();
                array[1] = job.jobOptions.jobName;
                array[2] = job.jobOptions.jobType.ToString();
                array[3] = job.IsActive().ToString();
                array[4] = job.jobOptions.targetAddress.ToString();
                array[5] = job.jobOptions.delay.ToString();
                array[6] = job.jobOutput;
                array = jobTable.FormatStringArray(array);

                jobTable.WriteColumnes(array);
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }

    public class JobListCommand : Command
    {
        public JobListCommand()
        {

        }

        public override void Execute()
        {

        }
    }

    public class JobSystemAddPingCommand : Command
    {
        public JobSystemAddPingCommand()
        {

        }

        public override void Execute()
        {

        }
    }

    public class JobSystemAddHttpCommand : Command
    {
        public JobSystemAddHttpCommand()
        {

        }

        public override void Execute()
        {

        }
    }

    public class JobSystemAddPortCommand : Command
    {
        public JobSystemAddPortCommand()
        {

        }

        public override void Execute()
        {

        }
    }

    public class JobSystemRemoveCommand : Command
    {
        public JobSystemRemoveCommand()
        {

        }

        public override void Execute()
        {

        }
    }

    public class JobSystemStartCommand : Command
    {
        public JobSystemStartCommand()
        {
            
        }

        public override void Execute()
        {

        }
    }

    public class JobSystemStopCommand : Command
    {
        public JobSystemStopCommand()
        {
            
        }

        public override void Execute()
        {

        }
    }
}
