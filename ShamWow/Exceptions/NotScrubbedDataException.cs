using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Exceptions
{
    public class NotScrubbedDataException : Exception
    {
        public NotScrubbedDataException() { }

        public NotScrubbedDataException(string message) : base(message) { }

    }
}
