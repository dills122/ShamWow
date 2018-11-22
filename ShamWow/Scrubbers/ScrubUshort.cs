using ShamWow.Constants;
using System;


namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubUshort : Attribute
    {
        public UshortType Type { get; private set; }

        public ScrubUshort() { }

        public ScrubUshort(UshortType scrubType)
        {
            Type = scrubType;
        }
    }
}
