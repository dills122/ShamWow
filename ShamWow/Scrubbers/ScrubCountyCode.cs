using System;
using System.IO;
using System.Linq;

namespace ShamWow.Scrubbers
{
    public static class ScrubCountyCode
    {
        private static readonly string _countyCodePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "CountyFIPS.csv");
        private static Random rand;

        public static string ScrubCountyFIPS()
        {
            rand = new Random();
            var lines = File.ReadLines(_countyCodePath).ToList();
            var line = lines.ElementAtOrDefault(rand.Next(lines.Count));

            return line.Split(',').FirstOrDefault();
        }

        public static string ScrubCountyName()
        {
            rand = new Random();
            var lines = File.ReadLines(_countyCodePath).ToList();
            var line = lines.ElementAtOrDefault(rand.Next(lines.Count));

            return line.Split(',').ElementAtOrDefault(1);
        }
    }
}
