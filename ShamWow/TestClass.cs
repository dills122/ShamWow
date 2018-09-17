using ShamWow.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow
{
    public class TestClass
    {
        [StringAtr("Phone")]
        [Scrub]
        public string phoneNumber { get; set; }
        public int testInt { get; set; }

        [StringAtr("SSN")]
        [Scrub]
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
        [Scrub]
        public string str { get; set; }
        public int num { get; set; }
    }

    public class AnotherTestClass
    {
        public string stringStrings { get; set; }
        [Scrub]
        public int dirtyInt { get; set; }

        public InnerTestClass InnerTest { get; set; }
    }
}
