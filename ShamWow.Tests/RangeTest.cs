﻿using ShamWow.Processor;
using ShamWow.Tests.TestModels;
using Xunit;

namespace ShamWow.Tests
{
    public class RangeTest
    {
        const int predefinedValue = 1000;

        [Fact]
        public void SimpleTest()
        {
            var obj = new RangeTestObj
            {
                dec = predefinedValue,
                dub = predefinedValue,
                ing = predefinedValue,
                lng = predefinedValue
            };

            IShamWow processor = ShamWowEngine.GetFactory().Create(obj, Constants.ScrubMode.Marked);

            processor.Scrub();
            var cleanedData = (RangeTestObj)processor.CleanData();

            var man = processor.GetManifest();
            Assert.InRange(cleanedData.dub, 100, 200);
            Assert.InRange(cleanedData.dec, 100, 200);
            Assert.InRange(cleanedData.ing, 100, 200);
            Assert.InRange(cleanedData.lng, 100, 200);
            Assert.NotNull(cleanedData);
            Assert.IsType<RangeTestObj>(cleanedData);
            Assert.True(processor.CheckManifest());
        }
    }
}
