using ShamWow.Constants;
using ShamWow.Processor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubString : Attribute
    {
        public StringType type { get; private set; }
        public int length { get; private set; }

        public ScrubString() { }

        public ScrubString(StringType scrubType)
        {
            type = scrubType;
        }

        public ScrubString(StringType scrubType, int length)
        {
            type = scrubType;
            if(type != StringType.Lorem)
            {
                throw new NotSupportedException($"Length is not supported by that type {type}");
            }
            this.length = length;
        }
    }
}
