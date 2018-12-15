using ShamWow.Interfaces.Attributes;
using ShamWow.Constants;
using System;

namespace ShamWowTests.TestModels
{
    public class SimpleTest
    {
        public string str { get; set; }
        [ScrubString(StringScrubber.Email)]
        public string emailStr { get; set; }
        public string[] strTwo { get; set; }
        public decimal Decimal { get; set; }
        public double Double { get; set; }
        public long MyLong { get; set; }
        public int Int { get; set; }
        public short Short { get; set; }
        public byte[] Byte { get; set; }
        public DateTime Date { get; set; }

        [PreserveValue]
        public string KeepMeTheSame { get; set; }
    }
}
