using System;

namespace ShamWow.Exceptions
{
    public class NotScrubbedDataException : Exception
    {
        public NotScrubbedDataException() { }

        public NotScrubbedDataException(string message) : base(message) { }

    }
}
