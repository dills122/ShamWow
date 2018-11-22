using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubSbyte : Attribute
    {
        public SbyteType Type { get; private set; }

        public ScrubSbyte() { }

        public ScrubSbyte(SbyteType scrubType)
        {
            Type = scrubType;
        }
    }
}
