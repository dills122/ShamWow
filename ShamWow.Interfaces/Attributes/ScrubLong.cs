using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubLong : Attribute
    {
        public LongScrubber scrubber { get; set; }

        public ScrubLong() { }

        public ScrubLong(LongScrubber scrubber)
        {
            this.scrubber = scrubber;
        }
    }
}
