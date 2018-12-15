using ShamWow.Interfaces.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class ComplexStateTest
    {
        [StatefulScrub("StateOne")]
        [ScrubString(ShamWow.Constants.StringScrubber.FullName)]
        public string str { get; set; }

        [StatefulScrub("StateOne")]
        [ScrubString(ShamWow.Constants.StringScrubber.FullName)]
        public string anotherStr { get; set; }

        [StatefulScrub("StateTwo")]
        [ScrubInteger(ShamWow.Constants.IntegerScrubber.Zip)]
        public int i { get; set; }

        [StatefulScrub("StateTwo")]
        [ScrubInteger(ShamWow.Constants.IntegerScrubber.Zip)]
        public int anotherI { get; set; }
    }
}
