using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubInteger : Attribute
    {
        public IntegerType type { get; private set; }

        public ScrubInteger() { }

        public ScrubInteger(IntegerType scrubType)
        {
            type = scrubType;
        }
    }
}
