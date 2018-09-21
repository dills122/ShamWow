using ShamWow.Processor;
using ShamWowTests.TestModels;
using System;
using Xunit;

namespace ShamWowTests
{
    public class ScrubberTests
    {
        [Theory]
        [InlineData("fake@test.com")]
        [InlineData("fake@email.com")]
        [InlineData("nexsysTech@faker.com")]
        public void SimpleTest(string email)
        {
            SimpleTest test = new SimpleTest
            {
                emailStr = email,
                str = "test string",
                strTwo = "another test string"
            };

            ProcessDocument process = new ProcessDocument(test, ShamWow.Constants.ScrubTypes.Marked)
                .Scrub();
            var cleanedData = (SimpleTest)process.CleanData();

            Assert.NotEqual(cleanedData.emailStr, email);
            Assert.NotNull(cleanedData);
            Assert.IsType<SimpleTest>(cleanedData);
            Assert.True(process.GetManifest().documentManifestInfos.Count == 1);
            Assert.True(process.CheckManifest());
        }

        [Theory]
        [InlineData("fake@test.com", "814-534-2342")]
        [InlineData("fakerTest@hacker.com", "444-334-2542")]
        [InlineData("scoobydoo@yahoo.com", "213-536-2642")]
        public void ComplexTest(string email, string phone)
        {
            ComplexTest complex = new ComplexTest
            {
                emailString = email,
                phoneStr = phone,
                testInt = 33,
                testStr = "strings strings",
                test = new SimpleTest
                {
                    emailStr = email,
                    str = "lorem test",
                    strTwo = "heres another string"
                }
            };

            ProcessDocument process = new ProcessDocument(complex, ShamWow.Constants.ScrubTypes.Marked)
    .Scrub();
            var cleanedData = (ComplexTest)process.CleanData();

            Assert.NotEqual(cleanedData.emailString, email);
            Assert.NotEqual(cleanedData.test.emailStr, email);
            Assert.NotEqual(cleanedData.emailString, cleanedData.test.emailStr);
            Assert.NotEqual(phone, cleanedData.phoneStr);
            Assert.NotNull(cleanedData);
            Assert.IsType<ComplexTest>(cleanedData);
            Assert.True(process.GetManifest().documentManifestInfos.Count == 3);
            Assert.True(process.CheckManifest());
        }
    }
}
