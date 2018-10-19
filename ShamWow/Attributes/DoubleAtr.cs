using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Attributes
{
    public class DoubleAtr : Attribute
    {
        public DoubleType scrubType { get; private set; }

        public DoubleAtr(DoubleType scrubType)
        {
            this.scrubType = scrubType;
        }
    }
}
