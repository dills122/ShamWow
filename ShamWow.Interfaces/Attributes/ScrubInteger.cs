using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubInteger : Attribute
    {
        public IntegerScrubber scrubber { get; private set; }
        public int start { get; private set; }
        public int end { get; private set; }

        public ScrubInteger() { }

        public ScrubInteger(IntegerScrubber scrubber)
        {
            this.scrubber = scrubber;
        }

        public ScrubInteger(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
