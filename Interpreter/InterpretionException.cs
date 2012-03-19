using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Easis.Wfs.Interpreting
{
    [Serializable]
    public class InterpretionException : Exception
    {
        public InterpretionException()
        {
        }

        public InterpretionException(string message) : base(message)
        {
        }

        public InterpretionException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InterpretionException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
