using System;

namespace ShamWow.Exceptions
{
    class StatefulKeyException : Exception
    {
        public StatefulKeyException() : base("Not a valid key or key already in use by another value")
        {

        }

        public StatefulKeyException(string message) : base(message) { }
    }
}
