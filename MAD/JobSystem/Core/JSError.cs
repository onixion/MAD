using System;

namespace MAD.JobSystemCore
{
    public static class JSError
    {
        public const string _colorTag = "red";
        public enum Type
        {
            JobNotExist,
            NodeNotExist
        }

        public static string Error(Type type, string text, bool colorEnable)
        {
            string _buffer = "";

            if (colorEnable)
                _buffer = "<color><" + _colorTag + ">";

            switch (type)
            {
                case Type.JobNotExist:
                    return _buffer + "Job does not exist! " + text;

                case Type.NodeNotExist:
                    return _buffer + "Node does not exist! " + text;

                default:
                    return "";
            }
        }
    }
}
