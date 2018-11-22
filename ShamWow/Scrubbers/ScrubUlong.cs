using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubUlong : Attribute
    {
        public UlongType Type { get; private set; }

        public ScrubUlong() { }

        public ScrubUlong(UlongType scrubType)
        {
            Type = scrubType;
        }
    }
}
