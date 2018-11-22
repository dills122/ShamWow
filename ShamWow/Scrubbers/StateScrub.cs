using System;
using System.Collections.Generic;
using System.Text;

namespace ShamWow.Scrubbers
{
    public class StateScrub : Attribute
    {
        public string valueName { get; set; }

        public StateScrub() { }
        /// <summary>
        /// Sets the Key for the stateful value
        /// </summary>
        /// <param name="valueName">Unique key for retreiving stateful value</param>
        public StateScrub(string valueName)
        {
            this.valueName = valueName;
        }
    }
}
