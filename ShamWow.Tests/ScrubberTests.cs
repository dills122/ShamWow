using ShamWow.Processor;
using ShamWowTests.TestModels;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace ShamWow.Tests
{
    public class ScrubberTests
    {
        [Theory]
        [InlineData("fake@test.com")]
        [InlineData("fake@email.com")]
        [InlineData("nexsysTech@faker.com")]
        public void SimpleTest(string email)
        {

            string[] str = { "test", "testing", "test" };

            SimpleTest test = new SimpleTest
            {
                emailStr = email,
                str = "test string",
                strTwo = str,
                Byte = Encoding.ASCII.GetBytes("Test STring")
            };

            IShamWow processor = ShamWowEngine.GetFactory().Create(test);

            processor.Scrub();
            var cleanedData = (SimpleTest)processor.CleanData();


            var man = processor.GetManifest();

            Assert.NotEqual(cleanedData.emailStr, email);
            Assert.NotNull(cleanedData);
            Assert.IsType<SimpleTest>(cleanedData);
            Assert.True(processor.CheckManifest());
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
                    //strTwo = "heres another string"
                }
            };

            ShamWow.Processor.IShamWow processor = ShamWow.Processor.ShamWowEngine.GetFactory().Create(complex);

            processor.Scrub();
            var cleanedData = (ComplexTest)processor.CleanData();

            Assert.NotEqual(cleanedData.emailString, email);
            Assert.NotEqual(cleanedData.test.emailStr, email);
            Assert.NotEqual(cleanedData.emailString, cleanedData.test.emailStr);
            Assert.NotEqual(phone, cleanedData.phoneStr);
            Assert.NotNull(cleanedData);
            Assert.IsType<ComplexTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }

        [Theory]
        [InlineData("tester122@testing.com", "starasdaerlk", "aflaksh askhdfal aslk")]
        [InlineData("fakeralasd@teasdfting.com", "asdlkfas askldfj", "aflaksh aslk")]
        [InlineData("tester122@testiasdfng.com", "buy sell maybe", "testing fact pool aslk")]
        public void FullScrubModeTest(string email, string randString, string randStringTwo)
        {
            SimpleTest test = new SimpleTest
            {
                emailStr = email,
                str = randString,
                Short = 200,
                Date = new System.DateTime(2018, 1, 1),
                Decimal = 10.01m,
                Double = 10.01d,
                Int = 15,
                MyLong = 323456789
            };


            ShamWow.Processor.IShamWow processor = ShamWow.Processor.ShamWowEngine.GetFactory().Create(test);

            processor.Scrub();
            var cleanedData = (SimpleTest)processor.CleanData();

            Assert.NotEqual(cleanedData.emailStr, email);
            Assert.NotNull(cleanedData);
            Assert.IsType<SimpleTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }

        [Fact]
        public void PreserveValue_DoesNotScrubTheMarkedValue()
        {
            const string expectedValue = "Keep me the same!";
            var simpleTest = new SimpleTest
            {
                KeepMeTheSame = expectedValue
            };

            ShamWow.Processor.IShamWow processor = ShamWow.Processor.ShamWowEngine.GetFactory().Create(simpleTest);
            processor.Scrub();
            var cleanedData = (SimpleTest)processor.CleanData();

            Assert.Equal(expectedValue, cleanedData.KeepMeTheSame);
        }
        [Fact]
        public void ArrayScrub_EnsureSameTypeArrayReturnedScrubbed()
        {
            var testIntArray = new int[] { 50, 10, 25, 30 };

            var testStrArray = new string[] { "string", "strings", "stringys", "stringy strings" };

            var model = new ArrayTest
            {
                arrayInt = testIntArray,
                arrayStr = testStrArray,
                str = "Test string"
            };

            IShamWow processor = ShamWowEngine.GetFactory().Create(model);
            processor.Scrub();
            var cleanedData = (ArrayTest)processor.CleanData();

            Assert.NotEqual(testIntArray[1], cleanedData.arrayInt[1]);
            Assert.NotEqual(testStrArray[1], cleanedData.arrayStr[1]);
            Assert.Equal(testIntArray.Length, cleanedData.arrayInt.Length);
            Assert.Equal(testStrArray.Length, cleanedData.arrayStr.Length);
            Assert.NotNull(cleanedData);
            Assert.IsType<ArrayTest>(cleanedData);
            Assert.True(processor.CheckManifest());
        }

        [Fact]
        public void FileScrub_EnsureFileIsScrubbed() 
        {
            const string fileStr = "This is a string to fake a file";
            const string expectedFileText = "Hello World";

            var model = new FileTest {
                str = "strings",
                emailString = "Email@email.com",
                orderFile = Encoding.ASCII.GetBytes(fileStr) 
            };

            IShamWow processor = ShamWowEngine.GetFactory().Create(model);
            processor.Scrub();

            var cleanData = (FileTest)processor.CleanData();

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");

            File.WriteAllBytes(path, cleanData.orderFile);

            var fileText = File.ReadAllText(path).Where(c => !char.IsControl(c)).ToArray();

            Assert.NotNull(cleanData.orderFile);
            Assert.Equal(expectedFileText, fileText);
            
        }
    }
}
