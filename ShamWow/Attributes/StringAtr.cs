using ShamWow.Constants;
using System;

namespace ShamWow.Attributes
{
    public class StringAtr : Attribute
    {
        public StringType scrubType { get; private set; }
        public int length { get; private set; }

        public StringAtr(StringType scrubType)
        {
            this.scrubType = scrubType;
        }

        public StringAtr(StringType scrubType, int length)
        {
            if (length > 0)
            {
                this.scrubType = scrubType;
                this.length = length;
            }
        }
    }
}
