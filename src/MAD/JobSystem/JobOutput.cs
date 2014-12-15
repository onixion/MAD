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
            outputs.Add(new OutputDescriptor("OutState", typeof(string), true));
            outputs.Add(new OutputDescriptor("ExDuration", typeof(int), true));
        }

        public OutputDescriptor GetOutputDesc(string name)
        {
            foreach (OutputDescriptor _desc in outputs)
                if (_desc.name == name)
                    return _desc;
            return null;
        }

        public string GetStrings()
        { 
            string _buffer = "";
            foreach (OutputDescriptor _desc in outputs)
                if (!_desc.dbIgnore)
                    _buffer += _desc.GetString() + " ";
            return _buffer;
        }
    }
}
