using ShamWow.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class SimpleTest
    {
        public string str { get; set; }
        [Scrub]
        [StringAtr("Email")]
        public string emailStr { get; set; }
        public string strTwo { get; set; }

    }
}
