using ShamWow.Interfaces.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWowTests.TestModels
{
    public class FileTest
    {
        [ScrubString(ShamWow.Constants.StringScrubber.FullName)]
        public string str { get; set; }

        [ScrubString(ShamWow.Constants.StringScrubber.Email)]
        public string emailString { get; set; }
        public byte[] orderFile {get; set;}
    }
}
