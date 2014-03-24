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
                            Console.WriteLine();
                            Console.WriteLine("ID:           " + temp.jobID);
                            Console.WriteLine("Name:         " + temp.options.jobName);
                            Console.WriteLine("Type:         " + temp.options.jobType);
                            Console.WriteLine("Delaytime:    " + temp.options.delayTime);
                            Console.WriteLine("IpAddress:    " + temp.options.targetAddress);
                            Console.WriteLine();

                            // it does not show job specific parameters yet :(
                        }
                        break;
                    
                    case "add":
                        Console.Write("Name: ");
                        list.Add(Console.ReadLine());
                        Console.Write("Type (p = ping, h = http, s = portscan): ");
                        list.Add(Console.ReadLine());
                        Console.Write("Delaytime: ");
                        list.Add(Console.ReadLine());
                        Console.Write("IP-Address: ");
                        list.Add(Console.ReadLine());

                        switch(list[1])
                        {
                            default:
                                break;
                            case "p":
                                Console.Write("TTL: ");
                                list.Add(Console.ReadLine());
                                system.AddJob(new JobOptions);
                                break;
                            case "h":
                                Console.Write("PORT: ");
                                list.Add(Console.ReadLine());
                                system.AddJob(new JobHttpOptions(list[0], Int32.Parse(list[2]), IPAddress.Parse(list[3]), Int32.Parse(list[4])));
                                break;
                            case "s":
                                Console.Write("PORT: ");
                                list.Add(Console.ReadLine());
                                //system.AddJob(new JobOptions(list[0], JobOptions.JobTypes.PortScan, Int32.Parse(list[2]), IPAddress.Parse(list[3]), Int32.Parse(list[4])));
                                break;
                        }
                        list.Clear();
                        break;

                    case "remove":
                        Console.Write("ID: ");
                        input = Console.ReadLine();
                        system.RemoveJob(Int32.Parse(input));
                        break;

                    case "clear":
                        system.ClearJobs();
                        break;

                    case "start":
                        Console.Write("ID: ");
                        input = Console.ReadLine();
                        system.StartJob(Int32.Parse(input));
                        break;

                    case "stop":
                        Console.Write("ID: ");
                        input = Console.ReadLine();
                        system.StopJob(Int32.Parse(input));
                        break;
                }
            }
        }
    }
}
