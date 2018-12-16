using ShamWow.Processor;
using ShamWow.Tests.TestModels;
using Xunit;

namespace ShamWow.Tests
{
    public class PreserveValueTests
    {
        [Fact]
        public void ScrubProperty_WithPredefinedValue()
        {
            const int _i = 10;
            const string _str = "ACB";

            var model = new PreserveTest
            {
                str = "Fake string",
                i = 50
            };

            IShamWow processor = ShamWowEngine.GetFactory().Create(model, Constants.ScrubMode.Full);

            processor.Scrub();
            var cleanedData = (PreserveTest)processor.CleanData();

            var man = processor.GetManifest();

            Assert.Equal(_i, cleanedData.i);
            Assert.Equal(_str, cleanedData.str);
            Assert.NotNull(cleanedData);
            Assert.IsType<PreserveTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }
    }
}
