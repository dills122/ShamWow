using ShamWow.Interfaces.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class StatefulTest
    {
        public string str { get; set; }
        public int i { get; set; }
        [StatefulScrub("emailStatfulScrub")]
        [ScrubString(ShamWow.Constants.StringScrubber.Email)]
        public string statefulString { get; set; }

        public InnerStatefulTest InnerStatefulTest { get; set; }
    }

    public class InnerStatefulTest
    {
        [StatefulScrub("emailStatfulScrub")]
        [ScrubString(ShamWow.Constants.StringScrubber.Email)]
        public string anotherStateful { get; set; }
        public int j { get; set; }
    }
}
