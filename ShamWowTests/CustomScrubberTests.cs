using ShamWow.Processor;
using ShamWow.Scrubbers;
using ShamWowTests.TestModels;
using System;
using Xunit;

namespace ShamWowTests
{
    public class CustomScrubberTests
    {
        [Fact]
        public void TestVIN()
        {
            //TODO add more test cases
            var vin = ScrubBasicTypes.ScrubVIN();

            Assert.NotNull(vin);
            //Tests build location
            Assert.True(int.TryParse(vin[0].ToString(), out int val));
            Assert.IsType<int>(val);
            Assert.IsType<string>(vin);
        }
    }
}
