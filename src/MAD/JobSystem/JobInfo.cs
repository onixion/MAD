using System;

namespace MAD.JobSystemCore
{
    public class JobInfo
    {
        private int _id;
        public int id { get { return _id; } }

        private Guid _guid;
        public Guid guid { get { return _guid; } }

        private string _name;
        public string name { get { return _name; } }

        private string _type;
        public string type { get { return _type; } }

        private string _outstate;
        public string outstate { get { return _outstate; } }

        public JobInfo(int id, Guid guid, string name, string type, string outstate)
        {
            _id = id;
            _guid = guid;
            _name = name;
            _type = type;
            _outstate = outstate;
        }
    }
}
