using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubDecimal : Attribute
    {
        public DecimalType type { get; private set; }

        public ScrubDecimal() { }

        public ScrubDecimal(DecimalType scrubType)
        {
            type = scrubType;
        }
    }
}
