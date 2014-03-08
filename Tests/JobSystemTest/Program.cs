using System;
using System.Net;
using System.Collections.Generic;

namespace JobSystemTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            JobOptions options1 = new JobOptions("Verbindung 1", 2000, JobOptions.JobTypes.PingRequest, IPAddress.Parse("127.0.0.1"));
            JobOptions options2 = new JobOptions("Verbindung 2", 505, JobOptions.JobTypes.PingRequest, IPAddress.Parse("192.0.0.1"));

            // create jobs
            system.CreateJob(options1);
            system.CreateJob(options2);

            // start jobs
            system.StartJob(0);
            system.StartJob(1);

            */

            // MIT CONSOLE
            Program p = new Program();
            p.vConsole();
        }

        // __________________________________________________________

        JobSystem system = new JobSystem();
        private string input;
        private List<string> list = new List<string>();

        public void vConsole()
        {
            while (true)
            {
                Console.Write("=> ");
                input = Console.ReadLine();

                switch (input)
                { 
                    default:
                        break;

                    case "list":
                        Console.WriteLine("JOB LIST:");
                        foreach (Job temp in system.jobs)
                        {
                            Console.WriteLine("ID:        " + temp.jobID);
                            Console.WriteLine("Name:      " + temp.options.jobName);
                            Console.WriteLine("Type:      " + temp.options.type);
                            Console.WriteLine("Delaytime: " + temp.options.delayTime);
                            Console.WriteLine("Target:    " + temp.options.target);
                            Console.WriteLine();
                        }
                        break;
                    
                    case "create":
                        Console.Write("Name: ");
                        list.Add(Console.ReadLine());
                        Console.Write("Delaytime: ");
                        list.Add(Console.ReadLine());
                        Console.Write("Target: ");
                        list.Add(Console.ReadLine());
                        Console.Write("Type (p = ping, h = http, s = portscan): ");
                        list.Add(Console.ReadLine());
                        switch(list[3])
                        {
                            default:
                                break;
                            case "p":
                                system.CreateJob(new JobOptions(list[0], Int32.Parse(list[1]), JobOptions.JobTypes.PingRequest, IPAddress.Parse(list[2])));
                                break;
                            case "h":
                                system.CreateJob(new JobOptions(list[0], Int32.Parse(list[1]), JobOptions.JobTypes.HttpRequest, IPAddress.Parse(list[2])));
                                break;
                        }

                        list.Clear();
                        break;

                    case "start":
                        Console.Write("ID: ");
                        input = Console.ReadLine();
                        system.StartJob(Convert.ToInt64(input));
                        break;
                }
            }
        }
    }
}
