using System;
using System.Collections.Generic;
using System.Text;
using Faker;

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
    }
}
