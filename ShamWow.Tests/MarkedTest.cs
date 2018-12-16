using ShamWow.Processor;
using ShamWowTests.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShamWow.Tests
{
    public class MarkedTest
    {
        [Fact]
        public void MarkedScrubModeTest()
        {
            const string email = "this really isn't an email right?";
            const string stayTheSame = "This should be..";
            var model = new SimpleTest
            {
                emailStr = email,
                KeepMeTheSame = stayTheSame
            };

            IShamWow processor = ShamWowEngine.GetFactory().Create(model, Constants.ScrubMode.Marked);
            processor.Scrub();

            var cleanData = (SimpleTest)processor.CleanData();
            var man = processor.GetManifest();
            var IsSuccessful = processor.CheckManifest();

            Assert.IsType<SimpleTest>(cleanData);
            Assert.True(IsSuccessful);
            Assert.Equal(stayTheSame, cleanData.KeepMeTheSame);
            Assert.NotEqual(email, cleanData.emailStr);
        }
    }
}
