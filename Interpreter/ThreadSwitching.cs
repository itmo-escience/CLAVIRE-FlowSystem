using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Easis.Wfs.Interpreting
{
    public interface IThreadConnectable
    {
        /// <summary>
        /// throws ThreadDisconnectedException
        /// </summary>
        void ThreadReConnect();
    }

    [Serializable]
    public class ThreadDisconnectedException : Exception
    {
        public ThreadDisconnectedException()
        {
        }

        public ThreadDisconnectedException(string message)
            : base(message)
        {
        }

        public ThreadDisconnectedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ThreadDisconnectedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
