using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubUint : Attribute
    {
        public UintType Type { get; private set; }

        public ScrubUint() { }

        public ScrubUint(UintType scrubType)
        {
            Type = scrubType;
        }
    }
}
