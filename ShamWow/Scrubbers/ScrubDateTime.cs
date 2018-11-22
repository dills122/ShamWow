using ShamWow.Constants;
using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubDateTime : Attribute
    {
        public DateTimeType Type { get; private set; }

        public ScrubDateTime() { }

        public ScrubDateTime(DateTimeType scrubType)
        {
            Type = scrubType;
        }
    }
}
