using System;
using System.IO;
using System.Xml.Linq;
using Rock.TSI.Ambassador.Contracts.V1.Order;
using System.Diagnostics;
using Rock.Nexsys.ShamWow.SerializationHelpers;
using Rock.Nexsys.ShamWow;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ////Pipeline Implementation
            //IPipeline<string, string> mrClean = new ProcessDocumentPipeline();

            //var list = new List<string>();
            //list.Add(@".\TestFiles\AMB1C100\AMB1C100.OrderV1.xml");

            //var clean = mrClean.ProcessWaitForResults(list).Result;

            //Manual Implementation
            //ProcessOrderV1();

            //TestHolderProcessor();

            //InnerTestClass test = new InnerTestClass
            //{
            //    num = 20,
            //    str = "string"
            //};

            //ProcessDocument process = new ProcessDocument(test, ShamWow.Constants.ScrubTypes.Full)
            //    .Scrub();
            //var clean = process.CleanData();
            //var man = process.GetManifest();
            //var check = process.CheckManifest();

            ProcessOrderV1Full();

            Console.ReadLine();
        }

        public static void ProcessOrderV1()
        {
            var path = @".\TestFiles\AMB1C100\AMB1C100.OrderV1.xml";
            if(File.Exists(path))
            {
                XDocument order = XDocument.Load(path);

                Order transformedOrder = TransformDocument(order);

                Rock.Nexsys.ShamWow.Processor.IShamWow processor = Rock.Nexsys.ShamWow.Processor.ShamWowEngine.GetFactory().Create(transformedOrder);

                processor.Scrub();

                var cleanData = processor.CleanData();

                var cleanDataString = XmlHelper.SerializeObject(cleanData);

                var manifest = processor.GetManifest();

            }            
        }

        public static void ProcessOrderV1Full()
        {
            var path = @".\TestFiles\AMB1C100\AMB1C100.OrderV1.xml";
            if (File.Exists(path))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                XDocument order = XDocument.Load(path);

                Order transformedOrder = TransformDocument(order);

                Rock.Nexsys.ShamWow.Processor.IShamWow processor = Rock.Nexsys.ShamWow.Processor.ShamWowEngine.GetFactory().Create(transformedOrder);

                processor.Scrub();

                var cleanData = processor.CleanData();

                var cleanDataString = XmlHelper.SerializeObject(cleanData);

                var manifest = processor.GetManifest();

                var checkManifest = processor.CheckManifest();
                stopwatch.Stop();

                Console.WriteLine("ShamWow completed the scrubbing in {0}", stopwatch.Elapsed.TotalSeconds);
            }
        }

        public static TestClass CreateClass()
        {
            return new TestClass
            {
                stringSSN = "klsadjrlkejasdfasdf",
                testInt = 5,
                phoneNumber = "124123kljsadfe",
                anotherTestClass = new AnotherTestClass
                {
                    dirtyInt = 4534523,
                    InnerTest = new InnerTestClass
                    {
                        num = 431211,
                        str = "Stringy"
                    },
                    stringStrings = "strings all around"
                },
                innerTestClass = new InnerTestClass
                {
                    num = 10, 
                    str = "teststr"
                }
            };
        }

        public static Order TransformDocument(XDocument doc)
        {
            return XmlHelper.DeserializeObject<Order>(doc.ToString());
        }

        public static string TransformDocument(Order order)
        {
            return XmlHelper.SerializeObject(order);
        }
    }
}
