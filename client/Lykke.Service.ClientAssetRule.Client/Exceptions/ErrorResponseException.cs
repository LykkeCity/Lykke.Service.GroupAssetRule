using System;
using System.Runtime.Serialization;

namespace Lykke.Service.ClientAssetRule.Client.Exceptions
{
    /// <summary>
    /// Represents errors that occur during call service API.
    /// </summary>
    [Serializable]
    public class ErrorResponseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class.
        /// </summary>
        public ErrorResponseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
        protected ErrorResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ErrorResponseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponseException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.</param>
        public ErrorResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
