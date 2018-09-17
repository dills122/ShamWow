using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Attributes
{
    public class IntAtr : Attribute
    {
        private enum IntTypes
        {
            Phone,
            Zip
        }
        public string scrubType { get; private set; }

        public IntAtr(string scrubType)
        {
            IntTypes type;
            Enum.TryParse(scrubType, out type);
            this.scrubType = scrubType;
            if (!Enum.IsDefined(typeof(IntTypes), scrubType))
            {
                //Not the best fit, but good for now
                throw new InvalidOperationException("Not Valid Scrub Type");
            }
        }
    }
}
