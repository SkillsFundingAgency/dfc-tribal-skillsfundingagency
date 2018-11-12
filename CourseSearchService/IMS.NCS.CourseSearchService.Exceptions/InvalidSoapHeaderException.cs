using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IMS.NCS.CourseSearchService.Exceptions
{
    [Serializable()]
    public class InvalidSoapHeaderException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public InvalidSoapHeaderException()
            : base()
        { }

        /// <summary>
        /// Initializes with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public InvalidSoapHeaderException(string message)
            : base(message)
        { }

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
        public InvalidSoapHeaderException(string message, Exception exception) :
            base(message, exception)
        { }


        /// <summary>
        /// This constructor is called during deserialization to reconstitute the exception object transmitted over a stream
        /// </summary>
        /// <param name="info">Holds the serialized object data about the exception being thrown</param>
        /// <param name="context">Contains contextual information about the source or destination</param>
        protected InvalidSoapHeaderException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        { }
    }
}
