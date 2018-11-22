using ShamWow.Constants;
using ShamWow.Scrubbers;

namespace ShamWowTests.TestModels
{
    public class SimpleTest
    {
        public string str { get; set; }
        [Scrub]
        [ScrubString(StringType.Email)]
        public string emailStr { get; set; }
        public string strTwo { get; set; }

    }
}
