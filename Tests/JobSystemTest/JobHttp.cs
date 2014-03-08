using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystemTest
{
    class JobHttp : Job
    {
        public JobHttp(JobOptions options)
        {
            this.options = options;
            InitJob();
        
        }

        public override void DoJob()
        {
            
        }
    }
}
