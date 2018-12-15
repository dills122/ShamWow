using System;
using System.Linq;

namespace ShamWow.Scrubbers
{
    public static class ScrubBasicTypes
    {
        private static Random rand;

        public static string ScrubSSN()
        {
            rand = new Random();
            int iThree = rand.Next(132, 921);
            int iTwo = rand.Next(12, 83);
            int iFour = rand.Next(1423, 9211);
            return iThree.ToString() + "-" + iTwo.ToString() + "-" + iFour.ToString();
        }

        public static string ScrubVIN()
        {
            rand = new Random();
            var vin = $"{rand.Next(9)}{RandomString(2)}{RandomAlphaNumStr(5)}{RandomString(1)}{RandomAlphaNumStr(1)}{RandomString(1)}{rand.Next(999999)}";
            //TODO add verification or improve the generation
            return vin;
        }

        public static string RandomString(int length)
        {
            rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        public static string RandomAlphaNumStr(int length)
        {
            rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var str = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rand.Next(s.Length)]).ToArray());

            //For VIN Calculation
            if (str.Contains('Z'))
            {
                return RandomAlphaNumStr(length);
            }
            return str;
        }
    }
}
