using System;
using System.Collections;
using System.Collections.Generic;
namespace JobSystemTest
{
    class JobSystem
    {
        public List<Job> jobs = new List<Job>();

        public JobSystem()
        { 
        
        
        }

        public void CreateJob(JobOptions options)
        {
            switch (options.type.ToString())
            { 
                case "PingRequest":
                    jobs.Add(new JobPing(options));
                    break;

                case "HttpReqeust":

                    break;

                case "PortScan":

                    break;
            }
        }

        public void StartJob(long JobID)
        {
            foreach(Job temp in jobs)
            {
                if (temp.jobID == JobID)
                {
                    temp.Start();
                    break;
                }

            }
        }
    }
}
