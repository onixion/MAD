using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MAD
{
    public class JobNotification
    {
        public List<JobRule> rules = new List<JobRule>();

        public JobNotification()
        { 
        
        }
    }
}
