using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubDouble : Attribute
    {
        public DoubleScrubber scrubber {get; private set; }

        public ScrubDouble() { }

        public ScrubDouble(DoubleScrubber scrubber)
        {
            this.scrubber = scrubber;
        }
    }
}
