using ShamWow.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class ComplexTest
    {
        public string testStr { get; set; }
        public int testInt { get; set; }
        [Scrub]
        [StringAtr("Email")]
        public string emailString { get; set; }
        [Scrub]
        [StringAtr("Phone")]
        public string phoneStr { get; set; }

        public SimpleTest test { get; set; }
    }

}
