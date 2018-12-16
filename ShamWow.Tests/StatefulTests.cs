using ShamWow.Processor;
using ShamWow.Tests.TestModels;
using ShamWowTests.TestModels;
using Xunit;

namespace ShamWow.Tests
{
    public class StatefulTests
    {
        [Theory]
        [InlineData("A random string that will be scrubbed", "statefulness")]
        [InlineData("A random string that should be scrubbed","state ful ness")]
        public void TestStatefulAttribute(string randomStr, string otherStatful)
        {
            var model = new StatefulTest
            {
                i = 5,
                statefulString = randomStr,
                str = "strings",
                InnerStatefulTest = new InnerStatefulTest
                {
                    anotherStateful = otherStatful,
                    j = 50
                }
            };


            IShamWow processor = ShamWowEngine.GetFactory().Create(model, Constants.ScrubMode.Marked);

            processor.Scrub();
            var cleanedData = (StatefulTest)processor.CleanData();


            var man = processor.GetManifest();

            Assert.NotEqual(randomStr, cleanedData.statefulString);
            Assert.NotEqual(otherStatful, cleanedData.InnerStatefulTest.anotherStateful);
            Assert.Equal(cleanedData.statefulString, cleanedData.InnerStatefulTest.anotherStateful);
            Assert.NotNull(cleanedData);
            Assert.IsType<StatefulTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }

        [Theory]
        [InlineData("A random string that will be scrubbed", "statefulness", 50, 500)]
        [InlineData("A random string that should be scrubbed", "state ful ness", 30, 300)]
        public void TestComplexStatefulModel(string randomStr, string anotherRandomStr, int num, int anotherNum)
        {
            var model = new ComplexStateTest
            {
                str = randomStr,
                anotherStr = anotherRandomStr,
                anotherI = anotherNum,
                i = num
            };


            ShamWow.Processor.IShamWow processor = ShamWow.Processor.ShamWowEngine.GetFactory().Create(model, Constants.ScrubMode.Marked);

            processor.Scrub();
            var cleanedData = (ComplexStateTest)processor.CleanData();


            var man = processor.GetManifest();

            Assert.NotEqual(randomStr, cleanedData.str);
            Assert.NotEqual(anotherRandomStr, cleanedData.anotherStr);
            Assert.NotEqual(num, model.i);
            Assert.NotEqual(anotherNum, cleanedData.anotherI);
            Assert.Equal(cleanedData.str, cleanedData.anotherStr);
            Assert.Equal(cleanedData.i, cleanedData.anotherI);
            Assert.NotNull(cleanedData);
            Assert.IsType<ComplexStateTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }

        [Theory]
        [InlineData("A random string that will be scrubbed", "statefulness", 50, 500)]
        [InlineData("A random string that should be scrubbed", "state ful ness", 30, 300)]
        public void FullModelTest(string randomStr, string anotherRandomStr, int num, int anotherNum)
        {

            var model = new FullModelTest
            {
                anotherStateful = randomStr,
                str = anotherRandomStr,
                ComplexStateTest = new ComplexStateTest
                {
                    str = randomStr,
                    anotherStr = anotherRandomStr,
                    anotherI = anotherNum,
                    i = num
                },
                SimpleTest = new SimpleTest
                {
                    str = "strings"
                },
                statefulTest = new StatefulTest
                {
                    i = num,
                    InnerStatefulTest = new InnerStatefulTest
                    {
                        anotherStateful = anotherRandomStr,
                        j = anotherNum
                    },
                    statefulString = randomStr,
                    str = "strings"
                }
            };

            ShamWow.Processor.IShamWow processor = ShamWow.Processor.ShamWowEngine.GetFactory().Create(model, Constants.ScrubMode.Marked);

            processor.Scrub();
            var cleanedData = (FullModelTest)processor.CleanData();


            var man = processor.GetManifest();
            //StateOne Asserts
            Assert.Equal(cleanedData.str, cleanedData.ComplexStateTest.str);
            Assert.Equal(cleanedData.ComplexStateTest.str, cleanedData.ComplexStateTest.anotherStr);
            Assert.NotEqual(anotherRandomStr, cleanedData.str);
            Assert.NotEqual(randomStr, cleanedData.ComplexStateTest.str);
            Assert.NotEqual(anotherRandomStr, cleanedData.ComplexStateTest.anotherStr);

            //emailStatfulScrub Asserts
            Assert.Equal(cleanedData.anotherStateful, cleanedData.statefulTest.statefulString);
            Assert.Equal(cleanedData.statefulTest.statefulString, cleanedData.statefulTest.InnerStatefulTest.anotherStateful);

            Assert.NotNull(cleanedData);
            Assert.IsType<FullModelTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }

        [Fact]
        public void GivenString_ScrubToNumber_SaveState()
        {
            const string Id = "String";
            const string IdTwo = "Another String";

            var model = new StatefulStringIdTest
            {
                Id = Id,
                IdTwo = IdTwo
            };

            IShamWow processor = ShamWowEngine.GetFactory().Create(model, Constants.ScrubMode.Marked);

            processor.Scrub();
            var cleanedData = (StatefulStringIdTest)processor.CleanData();

            var man = processor.GetManifest();
            //StateOne Asserts
            Assert.Equal(cleanedData.Id, cleanedData.IdTwo);
            Assert.NotEqual(cleanedData.Id, Id);
            Assert.NotEqual(cleanedData.IdTwo, IdTwo);

            Assert.NotNull(cleanedData);
            Assert.IsType<StatefulStringIdTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }
    }
}
