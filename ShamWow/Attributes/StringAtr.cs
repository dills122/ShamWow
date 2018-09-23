using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Attributes
{
    public class StringAtr : Attribute
    {
        public string scrubType { get; private set; }
        public int length { get; private set; }

        public StringAtr(string scrubType)
        {
            if (CheckAttr(scrubType))
            {
                this.scrubType = scrubType;
            }
        }

        public StringAtr(string scrubType, int length)
        {
            if (CheckAttr(scrubType) && length > 0)
            {
                this.scrubType = scrubType;
                this.length = length;
            }
        }

        private bool CheckAttr(string scrubType)
        {
            StringTypes type;
            Enum.TryParse(scrubType, out type);

            if (!Enum.IsDefined(typeof(StringTypes), scrubType))
            {
                //Not the best fit, but good for now
                throw new InvalidOperationException("Not Valid Scrub Type");
            }
            return true;
        }
    }
}
