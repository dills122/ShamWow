using System;

namespace ShamWow.Scrubbers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class Scrub : Attribute { }
}
