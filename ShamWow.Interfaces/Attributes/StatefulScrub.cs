using System;

namespace ShamWow.Interfaces.Attributes
{
    public class StatefulScrub : Attribute
    {
        public string valueName { get; set; }

        public StatefulScrub() { }
        /// <summary>
        /// Sets the Key for the stateful value
        /// </summary>
        /// <param name="valueName">Unique key for retreiving stateful value</param>
        public StatefulScrub(string valueName)
        {
            this.valueName = valueName;
        }
    }
}
