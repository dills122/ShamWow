using ShamWow.Attributes;
using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestClient
{
    public class TestClass
    {
        [Scrub]
        [StringAtr(StringType.Address)]
        public string str { get; set; }
        [Scrub]
        [IntAtr(IntType.Zip)]
        public int testZip { get; set; }
    }
}
