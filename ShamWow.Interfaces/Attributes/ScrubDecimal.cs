using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubDecimal : Attribute
    {
        public DecimalScrubber scrubber { get; private set; }
        public int start { get; private set; }
        public int end { get; private set; }

        public ScrubDecimal() { }

        public ScrubDecimal(DecimalScrubber scrubber)
        {
            this.scrubber = scrubber;
        }

        public ScrubDecimal(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
