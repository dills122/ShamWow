using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Exceptions
{
    public class NotSupportedTypeException : Exception
    {
        public NotSupportedTypeException() : base("Type not supported by scrubber") { }

        public NotSupportedTypeException(string message) : base(message) { }
    }
}
