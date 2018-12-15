using ShamWow.Scrubbers;
using System;
using Xunit;

namespace ShamWow.Tests
{
    public class CustomScrubberTests
    {
        [Fact]
        public void CountyFIPSScrubberTest()
        {
            var code = ScrubCountyCode.ScrubCountyFIPS();

            Int32.TryParse(code, out int result);

            Assert.NotNull(code);
            Assert.True(result > 0);

        }

        [Fact]
        public void CountyNamecrubberTest()
        {
            var code = ScrubCountyCode.ScrubCountyName();

            Assert.NotNull(code);
            Assert.IsType<string>(code);
        }

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
