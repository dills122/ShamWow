using ShamWow.Interfaces.Attributes;

namespace ShamWow.Tests.TestModels
{
    public class PreserveTest
    {
        [PredefinedValue("ACB")]
        public string str { get; set; }

        [PredefinedValue(10)]
        public int i { get; set; }
    }
}
