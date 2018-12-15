using ShamWow.Interfaces.Attributes;
using ShamWow.Constants;

namespace ShamWowTests.TestModels
{
    public class ComplexTest
    {
        public string testStr { get; set; }
        public int testInt { get; set; }
        [ScrubString(StringScrubber.Email)]
        public string emailString { get; set; }
        [ScrubString(StringScrubber.Phone)]
        public string phoneStr { get; set; }

        public SimpleTest test { get; set; }
    }

}
