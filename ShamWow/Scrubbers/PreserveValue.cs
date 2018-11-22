using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PreserveValue : Attribute
    {
        public PreserveValue() { }
    }
}
