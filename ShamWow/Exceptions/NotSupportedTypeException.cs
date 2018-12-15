using System;

namespace ShamWow.Exceptions
{
    public class NotSupportedTypeException : Exception
    {
        public NotSupportedTypeException() : base("Type not supported by scrubber yet")
        {

        }

        public NotSupportedTypeException(string message) : base(message) { }
    }
}
