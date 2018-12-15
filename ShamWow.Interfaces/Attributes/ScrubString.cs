using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubString : Attribute
    {
        public StringScrubber scrubber { get; private set; }
        public int length { get; private set; }

        public int minValue { get; private set; }

        public int maxValue { get; private set; }

        public bool IsRedacted { get; private set; }

        public ScrubString() { }

        public ScrubString(StringScrubber scrubber)
        {
            this.scrubber = scrubber;
        }

        public ScrubString(StringScrubber scrubber, bool IsRedacted)
        {
            this.scrubber = scrubber;
            this.IsRedacted = IsRedacted;
        }

        public ScrubString(StringScrubber scrubber, int length)
        {
            if (length > 0)
            {
                this.scrubber = scrubber;
                this.length = length;
            }
            //TODO Throw error if incorrect length?
        }

        //Used for generating numbers as strings
        public ScrubString(StringScrubber scrubber, int minValue, int maxValue)
        {
            this.scrubber = scrubber;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}
