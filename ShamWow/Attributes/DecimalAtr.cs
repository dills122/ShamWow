using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Attributes
{
    class DecimalAtr : Attribute
    {

        public string scrubType { get; private set; }

        public DecimalAtr(string scrubType)
        {
            DecimalTypes type;
            Enum.TryParse(scrubType, out type);
            this.scrubType = scrubType;
            if (!Enum.IsDefined(typeof(DecimalTypes), scrubType))
            {
                //Not the best fit, but good for now
                throw new InvalidOperationException("Not Valid Scrub Type");
            }
        }
    }
}
