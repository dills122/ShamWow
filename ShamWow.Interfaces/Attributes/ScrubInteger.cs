using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubInteger : Attribute
    {
        public IntegerScrubber scrubber { get; private set; }

        public ScrubInteger() { }

        public ScrubInteger(IntegerScrubber scrubber)
        {
            this.scrubber = scrubber;
        }
    }
}
