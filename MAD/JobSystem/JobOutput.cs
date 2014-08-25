using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace MAD.JobSystemCore
{
    public class JobOutput
    {
        public OutState outState;
        public enum OutState { NULL, Success, Failed, Exception }

        public List<OutputDescriptor> outputs;
        public string outDescription = "";

        public JobOutput()
        {
            outState = OutState.NULL;
            outputs = new List<OutputDescriptor>();
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
