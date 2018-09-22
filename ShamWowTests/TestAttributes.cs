using ShamWow.Processor;
using ShamWowTests.TestModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShamWowTests
{
    public class TestAttributes
    {
        [Fact]
        public void TestBadPOCO()
        {
            ErrorTest test = new ErrorTest
            {
                failString = "string"
            };

            ProcessDocument processDocument = ProcessDocument.GetFactory().Create(test, ShamWow.Constants.ScrubTypes.Marked);

            processDocument.Scrub();
                
            var result = processDocument.CleanData();

            Assert.NotNull(result);
        }    
    }
}
