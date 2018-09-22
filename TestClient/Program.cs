﻿using ShamWow.Processor;
using System;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TestClass test = new TestClass
            {
                str = "Fake Address",
                testZip = 15767
            };

            var tst = Faker.Phone.Number();

            //ProcessDocument processor = new
            //    ProcessDocument(test, ShamWow.Constants.ScrubTypes.Full)
            //    .Scrub();

            ProcessDocument processor = ProcessDocument.GetFactory().Create(test, ShamWow.Constants.ScrubTypes.Full);

            processor.Scrub();

            var cleanData = processor.CleanData();

            var cleanDataString = XmlHelper.SerializeObject(cleanData);

            var manifest = processor.GetManifest();

            var checkManifest = processor.CheckManifest();

            Console.ReadKey();
        }
    }
}
