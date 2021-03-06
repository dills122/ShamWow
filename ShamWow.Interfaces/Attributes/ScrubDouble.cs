﻿using ShamWow.Constants;
using System;

namespace ShamWow.Interfaces.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ScrubDouble : Attribute
    {
        public DoubleScrubber scrubber {get; private set; }
        public int start { get; private set; }
        public int end { get; private set; }

        public ScrubDouble() { }

        public ScrubDouble(DoubleScrubber scrubber)
        {
            this.scrubber = scrubber;
        }

        public ScrubDouble(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }
}
