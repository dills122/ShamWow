using ShamWow.Constants;
using ShamWow.Scrubbers;

namespace ShamWowTests.TestModels
{
    public class ComplexTest
    {
        public string testStr { get; set; }
        public int testInt { get; set; }
        [Scrub]
        [ScrubString(StringType.Email)]
        public string emailString { get; set; }
        [Scrub]
        [ScrubString(StringType.Phone)]
        public string phoneStr { get; set; }

        public SimpleTest test { get; set; }
    }

}
