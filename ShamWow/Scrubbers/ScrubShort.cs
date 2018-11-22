using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubShort : Attribute
    {
        public ShortType type { get; private set; }

        public ScrubShort() { }

        public ScrubShort(ShortType scrubType)
        {
            type = scrubType;
        }
    }
}
