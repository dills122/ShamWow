﻿using ShamWow.Processor;
using ShamWow.SerializationHelpers;
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

            IProcessDocument processor = ProcessDocument.GetFactory().Create(test, ShamWow.Constants.ScrubType.Full);

            processor.Scrub();

            var cleanData = processor.CleanData();

            var cleanDataString = XmlHelper.SerializeObject(cleanData);

            var manifest = processor.GetManifest();

            var checkManifest = processor.CheckManifest();

            Console.ReadKey();
        }
    }
}
