using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubShort : Attribute
    {

        public ShortScrubber scrubber { get; set; }

        public ScrubShort() { }

        public ScrubShort(ShortScrubber scrubber)
        {
            this.scrubber = scrubber;
        }
    }
}
