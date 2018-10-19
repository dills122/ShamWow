using ShamWow.Attributes;
using ShamWow.Constants;

namespace ShamWowTests.TestModels
{
    public class ComplexTest
    {
        public string testStr { get; set; }
        public int testInt { get; set; }
        [Scrub]
        [StringAtr(StringType.Email)]
        public string emailString { get; set; }
        [Scrub]
        [StringAtr(StringType.Phone)]
        public string phoneStr { get; set; }

        public SimpleTest test { get; set; }
    }

}
