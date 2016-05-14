using System;
using System.Runtime.Serialization;

namespace SaveAsOcr.Exceptions
{
    public class RegexException : ApplicationException
    {
        public RegexException()
        {
        }

        public RegexException(string message) : base(message)
        {
        }

        public RegexException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RegexException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}