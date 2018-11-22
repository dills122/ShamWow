using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubLong : Attribute
    {
        public LongType type { get; private set; }

        public ScrubLong() { }

        public ScrubLong(LongType scrubType)
        {
            type = scrubType;
        }
    }
}
