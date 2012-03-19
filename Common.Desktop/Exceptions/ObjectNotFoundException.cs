using System;

#if !SILVERLIGHT
using System.Runtime.Serialization;
#endif

namespace Easis.Common.Exceptions
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class ObjectNotFoundException: Exception
    {
        public ObjectNotFoundException()
        {
        }

        public ObjectNotFoundException(string message): base(message)
        {
        }

        public ObjectNotFoundException(string message, Exception inner): base(message, inner)
        {
        }

#if !SILVERLIGHT
        protected ObjectNotFoundException(
            SerializationInfo info,
            StreamingContext context): base(info, context)
        {
        }
#endif
    }
}