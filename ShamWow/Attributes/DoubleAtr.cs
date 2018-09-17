using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Attributes
{
    public class DoubleAtr : Attribute
    {
        private enum DoubleTypes
        {

        }
        public string scrubType { get; private set; }

        public DoubleAtr(string scrubType)
        {
            DoubleTypes type;
            Enum.TryParse(scrubType, out type);
            this.scrubType = scrubType;
            if (!Enum.IsDefined(typeof(DoubleTypes), scrubType))
            {
                //Not the best fit, but good for now
                throw new InvalidOperationException("Not Valid Scrub Type");
            }
        }
    }
}
