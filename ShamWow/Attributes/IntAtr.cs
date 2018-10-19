using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Attributes
{
    public class IntAtr : Attribute
    {

        public IntType scrubType { get; private set; }

        public IntAtr(IntType scrubType)
        {
            this.scrubType = scrubType;
        }
    }
}
