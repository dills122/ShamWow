using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubLong : Attribute
    {
        public LongScrubber scrubber { get; set; }
        public int start { get; private set; }
        public int end { get; private set; }

        public ScrubLong() { }

        public ScrubLong(LongScrubber scrubber)
        {
            this.scrubber = scrubber;
        }

        public ScrubLong(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
