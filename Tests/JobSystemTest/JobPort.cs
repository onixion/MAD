using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobSystemTest
{
    class JobPort : Job
    {
        public JobPort(JobOptions options)
        {
            this.options = options;
            InitJob();
        }

        public override void DoJob()
        {

        }
    }
}
