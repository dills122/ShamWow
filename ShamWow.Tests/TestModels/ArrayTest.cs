using ShamWow.Interfaces.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class ArrayTest
    {
        public string str { get; set; }
        [ScrubString(ShamWow.Constants.StringScrubber.FullName)]
        public string[] arrayStr { get; set; }
        [ScrubInteger()]
        public int[] arrayInt { get; set; }
    }
}
