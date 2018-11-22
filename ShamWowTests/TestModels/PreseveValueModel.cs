using ShamWow.Scrubbers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class PreserveValueModel
    {
        public string str { get; set; }
        [PreserveValue]
        public string noTouch { get; set; }
        public int scrubbedInt { get; set; }
    }
}
