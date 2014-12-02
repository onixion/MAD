using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobOutput
    {
        public OutState outState = OutState.NULL;
        public enum OutState { NULL, Success, Failed, Exception }

        public List<OutputDescriptor> outputs = new List<OutputDescriptor>();

        public JobOutput()
        {
            // Global OutDescriptors (= every Job need those)
            outputs.Add(new OutputDescriptor("OutState", typeof(string)));
            outputs.Add(new OutputDescriptor("ExDuration", typeof(int)));
        }

        public OutputDescriptor GetOutputDesc(string name)
        {
            foreach (OutputDescriptor _desc in outputs)
                if (_desc.name == name)
                    return _desc;
            return null;
        }
    }
}
