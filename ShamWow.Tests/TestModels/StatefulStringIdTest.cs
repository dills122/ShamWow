using ShamWow.Constants;
using ShamWow.Interfaces.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Tests.TestModels
{
    public class StatefulStringIdTest
    {
        [StatefulScrub("StateOne")]
        [ScrubString(StringScrubber.Number)]
        public string Id { get; set; }


        [StatefulScrub("StateOne")]
        [ScrubString(StringScrubber.Number)]
        public string IdTwo { get; set; }
    }
}
