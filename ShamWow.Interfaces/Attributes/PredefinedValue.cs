using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Interfaces.Attributes
{
    public class PredefinedValue : Attribute
    {
        public object value { get; set; }

        public PredefinedValue() { }

        public PredefinedValue(object value)
        {
            this.value = value;
        }
    }
}
