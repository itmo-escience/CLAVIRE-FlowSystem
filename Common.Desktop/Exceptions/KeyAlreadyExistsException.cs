using System;

#if !SILVERLIGHT
using System.Runtime.Serialization;
#endif

namespace Easis.Common.Exceptions
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class KeyAlreadyExistsException : Exception
    {
        public KeyAlreadyExistsException()
        {
        }

        public KeyAlreadyExistsException(string message) : base(message)
        {
        }

        public KeyAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
        }

#if !SILVERLIGHT
        protected KeyAlreadyExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}