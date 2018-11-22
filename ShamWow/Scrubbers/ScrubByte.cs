using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubByte : Attribute
    {
        public ByteType type { get; private set; }

        public ScrubByte() { }

        public ScrubByte(ByteType scrubType)
        {
            type = scrubType;
        }
    }
}
