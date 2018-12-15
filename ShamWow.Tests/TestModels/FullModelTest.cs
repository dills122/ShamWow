using ShamWow.Interfaces.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class FullModelTest
    {
        public StatefulTest statefulTest { get; set; }
        public SimpleTest SimpleTest { get; set; }
        public ComplexStateTest ComplexStateTest { get; set; }

        [StatefulScrub("StateOne")]
        [ScrubString(ShamWow.Constants.StringScrubber.FullName)]
        public string str { get; set; }

        [StatefulScrub("emailStatfulScrub")]
        [ScrubString(ShamWow.Constants.StringScrubber.Email)]
        public string anotherStateful { get; set; }
    }
}
