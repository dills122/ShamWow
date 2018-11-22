using ShamWow.Processor;
using ShamWowTests.TestModels;
using Xunit;

namespace ShamWowTests
{
    public class PreserveValueTest
    {

        [Theory]
        [InlineData("this shouldn't be touched", "this will be gone")]
        [InlineData("You have no power here", "asdfjsaldkfj")]
        [InlineData("Dont you dare", "bye")]
        public void SimplePreserve(string noTouch, string str)
        {

            PreserveValueModel model = new PreserveValueModel
            {
                noTouch = noTouch,
                scrubbedInt = 500,
                str = str
            };

            IProcessDocument processor = ProcessDocument.GetFactory().Create(model, ShamWow.Constants.ScrubType.Full);

            processor.Scrub();
            var cleanedData = (PreserveValueModel)processor.CleanData();

            Assert.NotEqual(str, cleanedData.str);
            Assert.Equal(noTouch, cleanedData.noTouch);
            Assert.NotNull(cleanedData);
            Assert.IsType<PreserveValueModel>(cleanedData);
            Assert.True(processor.CheckManifest());
        }
    }
}
