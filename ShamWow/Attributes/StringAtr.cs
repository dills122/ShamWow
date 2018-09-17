using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Attributes
{
    public class StringAtr : Attribute
    {
        private enum StringTypes
        {
            Address,
            AddressTwo,
            Phone,
            SSN,
            Email,
            City,
            State,
            Zip
        }
        public string scrubType { get; private set; }

        public StringAtr(string scrubType)
        {
            StringTypes type;
            Enum.TryParse(scrubType, out type);
            this.scrubType = scrubType;
            if (!Enum.IsDefined(typeof(StringTypes), scrubType))
            {
                //Not the best fit, but good for now
                throw new InvalidOperationException("Not Valid Scrub Type");
            }
        }
    }
}
