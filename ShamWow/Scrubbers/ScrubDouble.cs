using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubDouble : Attribute
    {
        public DoubleType type { get; private set; }

        public ScrubDouble() { }

        public ScrubDouble(DoubleType scrubType)
        {
            type = scrubType;
        }
    }
}
