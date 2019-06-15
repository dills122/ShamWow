using ShamWow.Interfaces.Attributes;

namespace ShamWow.Tests.TestModels
{
    public class RangeTestObj
    {
        [ScrubDecimal(100, 200)]
        public decimal dec { get; set; }
        [ScrubDouble(100, 200)]
        public double dub { get; set; }
        [ScrubInteger(100, 200)]
        public int ing { get; set; }
        [ScrubLong(100, 200)]
        public long lng { get; set; }
    }
}
