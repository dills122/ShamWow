using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Exceptions
{
    public class NotSupportedTypeException : Exception
    {
        string message = "Type not supported by scrubber yet";

        public NotSupportedTypeException()
        {

        }

        public NotSupportedTypeException(string message) : base(message) { }
    }
}
