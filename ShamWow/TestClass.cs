using ShamWow.Interfaces.Attributes;
using ShamWow.Constants;

namespace ShamWow
{
    public class TestClass
    {
        [ScrubString(StringScrubber.Phone)]
        public string phoneNumber { get; set; }
        public int testInt { get; set; }

        [ScrubString(StringScrubber.SSN)]
        public string stringSSN { get; set; }

        public InnerTestClass innerTestClass { get; set; }

        public AnotherTestClass anotherTestClass { get; set; }

        public TestClass()
        {
            innerTestClass = new InnerTestClass();
        }
    }

    public class InnerTestClass
    {
        [ScrubString(StringScrubber.Email)]
        public string str { get; set; }
        public int num { get; set; }
    }

    public class AnotherTestClass
    {
        public string stringStrings { get; set; }
        [ScrubInteger()]
        public int dirtyInt { get; set; }

        public InnerTestClass InnerTest { get; set; }
    }
}
