using System;
using System.Runtime.Serialization;

namespace Easis.Wfs.Interpreting.Utils
{
    [Serializable]
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException()
        {}
        public ObjectNotFoundException(string message) : base(message)
        {}

        public ObjectNotFoundException(string message, Exception inner) : base(message, inner)
        {}

        protected ObjectNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {}
    }


}