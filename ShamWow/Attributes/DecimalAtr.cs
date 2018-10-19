using ShamWow.Constants;
using System;

namespace ShamWow.Attributes
{
    class DecimalAtr : Attribute
    {
        public DecimalType scrubType { get; private set; }

        public DecimalAtr(DecimalType scrubType)
        {
            this.scrubType = scrubType;
        }
    }
}
