using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubFloat : Attribute
    {
        public FloatType Type { get; private set; }

        public ScrubFloat() { }

        public ScrubFloat(FloatType scrubType)
        {
            Type = scrubType;
        }
    }
}
