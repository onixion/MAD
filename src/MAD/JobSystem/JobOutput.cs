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
