using ShamWow.Constants;
using ShamWow.Scrubbers;

namespace TestClient
{
    public class TestClass
    {
        [Scrub]
        [ScrubString(StringType.Address)]
        public string str { get; set; }
        [Scrub]
        [ScrubInteger(IntegerType.Zip)]
        public int testZip { get; set; }
    }
}
