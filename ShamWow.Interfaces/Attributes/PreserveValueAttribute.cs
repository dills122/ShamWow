using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PreserveValueAttribute : Attribute
    {
        public PreserveValueAttribute() { }
    }
}
