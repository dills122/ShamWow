using ShamWow.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class ErrorTest
    {
        [StringAtr("Failed")]
        [Scrub]
        public string failString { get; set; }
    }
}
