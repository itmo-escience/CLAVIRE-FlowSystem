﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Easis.Wfs.EasyFlow
{
    /// <summary>
    /// Базовый класс для ошибок работы парсера.
    /// </summary>
    public class ParserException : SystemException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.SystemException"/> class.
        /// </summary>
        public ParserException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.SystemException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error. </param>
        public ParserException(string message): base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.SystemException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param><param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
        public ParserException(string message, Exception innerException): base(message, innerException)
        {
        }

#if !SILVERLIGHT
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.SystemException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data. </param><param name="context">The contextual information about the source or destination. </param>
        protected ParserException(SerializationInfo info, StreamingContext context): base(info, context)
        {
        }
#endif
    }
}
