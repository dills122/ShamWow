using ShamWow.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient
{
    public class TestClass
    {
        [Scrub]
        [StringAtr("Address")]
        public string str { get; set; }
        [Scrub]
        [IntAtr("Zip")]
        public int testZip { get; set; }
    }
}
