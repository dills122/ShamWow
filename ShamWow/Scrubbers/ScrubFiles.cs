using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShamWow.Scrubbers
{
    public static class ScrubFiles
    {

        public static byte[] ScrubFile()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".txt");

            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("Hello World");
            }

            return File.ReadAllBytes(path);
        }
    }
}
