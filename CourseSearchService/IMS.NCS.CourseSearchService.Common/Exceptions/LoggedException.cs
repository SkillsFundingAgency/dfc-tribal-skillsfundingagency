using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IMS.NCS.CourseSearchService.Common.Exceptions
{
    /// <summary>
    /// Exception class used to replace System Exceptions and indicate that the exception has been logged (so we
    /// don't log it in every layer of the application)
    /// </summary>
    [Serializable]
    public class LoggedException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public LoggedException()
        : base()
        {
        }

        /// <summary>
        /// Initializes with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public LoggedException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes with a specified error 
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.
        /// </param>
        /// <param name="exception">The exception that is the cause of the current exception. 
        /// If the innerException parameter is not a null reference, the current exception 
        /// is raised in a catch block that handles the inner exception.
        /// </param>
        public LoggedException(string message, Exception exception) :
        base(message, exception)
        {
        }

        /// <summary>
        /// Initializes with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.
        /// </param>
        protected LoggedException(SerializationInfo info, StreamingContext context) :
        base(info, context)
        {
        }
    }
}