using System;

#if !SILVERLIGHT
using System.Runtime.Serialization;
#endif

namespace Easis.Common.Exceptions
{
#if !SILVERLIGHT
    [Serializable]
#endif
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException()
        {
        }

        public InvalidFormatException(string message) : base(message)
        {
        }

        public InvalidFormatException(string message, Exception inner) : base(message, inner)
        {
        }
#if !SILVERLIGHT
        protected InvalidFormatException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
#endif
    }
}