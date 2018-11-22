using ShamWow.Constants;
using ShamWow.Exceptions;
using ShamWow.Scrubbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ShamWow.Processor
{
    public class Router
    {
        private Dictionary<string, object> _stateValues = new Dictionary<string, object>();
        public DocumentManifestInfo _manifestItem;
        private PropertyInfo _currentProperty;

        /// <summary>
        /// For new router instance, base router
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dataInstance"></param>
        public Router(Type type, object dataInstance)
        {
            FillStateValues(type, dataInstance);
        }

        /// <summary>
        /// Sends in already existing state values from parent instance
        /// </summary>
        /// <param name="stateValues"></param>
        public Router(Dictionary<string, object> stateValues)
        {
            _stateValues = stateValues;
        }

        public Dictionary<string, object> GetValues()
        { return _stateValues; }


        public void FillStateValues(Type type, object dataInstance)
        {
            var props = type.GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(StateScrub)));

            foreach (var prop in props)
            {
                var val = prop.GetValue(dataInstance, null);
                _currentProperty = prop;
                var scrubbedValue = RouteValue(val);
                var keyValue = GetStateKeyValue(prop);

                if (_stateValues.ContainsKey(keyValue))
                {
                    throw new StatefulKeyException("Key alredy in use, check POCOs state attributes");
                }

                _stateValues.Add(keyValue, scrubbedValue);
                //Reset to avoid issues with multiples
                _currentProperty = null;
            }
        }

        private string GetStateKeyValue(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute(typeof(StateScrub));

            var stateAttr = attr as StateScrub;

            if (String.IsNullOrEmpty(stateAttr.valueName))
            {
                return string.Empty;
            }
            return stateAttr.valueName;
        }

        public void ScrubProperty(PropertyInfo property, ref object dataInstance)
        {
            _currentProperty = property;

            var dirtyValue = property.GetValue(dataInstance, null);

            var cleanValue = RouteValue(dirtyValue);

            property.SetValue(dataInstance, cleanValue);

            CreateManifestItem(property, dirtyValue, cleanValue);

            _currentProperty = null;
        }

        /// <summary>
        /// Creates the mainest item for the data
        /// </summary>
        /// <param name="property"></param>
        private void CreateManifestItem(PropertyInfo property, object dirtyValue, object cleanValue)
        {
            if (!object.Equals(cleanValue, dirtyValue) && cleanValue != null && dirtyValue != null)
            {
                _manifestItem = ManifestBuilder.CreateManifestInfo(property, dirtyValue, cleanValue);
            }
        }

        public object RouteValue(object value)
        {
            switch (value)
            {
                case int i:
                    return RouteIntegerType();
                case byte[] bs:
                case byte b:
                    return RouteByteType();
                case short s:
                    return RouteShortType();
                case double d:
                    return RouteDoubleType();
                case decimal dec:
                    return RouteDecimalType();
                case string s:
                    return RouteStringType();
                case long l:
                    return RouteLongType();
                case int[] i:
                case short[] s:
                case double[] d:
                case decimal[] dc:
                case string[] strs:
                case long[] lng:
                    return RouteArrayType(value);
                case bool b:
                case DateTime d:
                    // Don't scrub these types, so just break here
                    break;

                default:
                    throw new NotSupportedTypeException($"Type not supported by scrubber yet: {value.GetType()}");
            }
            return null;
        }

        /// <summary>
        /// Routes Integers by scrubbing types
        /// </summary>
        /// <param name="property"></param>
        /// <param name="cleanDataInstance"></param>
        /// <param name="scrubType"></param>
        /// <returns></returns>
        private object RouteIntegerType()
        {
            var propAttribute = _currentProperty.GetCustomAttribute(typeof(ScrubInteger));
            if (propAttribute != null)
            {
                ScrubInteger atr = propAttribute as ScrubInteger;
                IntegerType attrScrubType = atr.type;

                switch (attrScrubType)
                {
                    case IntegerType.Zip:
                        return Convert.ToInt32(Faker.Address.ZipCode().Replace("-", string.Empty));
                    case IntegerType.VIN:
                        //TODO improve
                        return Faker.RandomNumber.Next(10000000, 99999999);
                    default:
                        return Faker.RandomNumber.Next(10000000, 99999999);
                }
            }
            else
            {
                return Faker.RandomNumber.Next(10000000, 99999999);
            }
        }

        private object RouteByteType()
        {
            //Documents
            if (_currentProperty.PropertyType.IsArray)
            {
                return ScrubFiles.ScrubFile();
            }
            else
            {

            }

            return null;
        }

        private object RouteShortType()
        {
            var scrubber = _currentProperty.GetCustomAttribute(typeof(ScrubShort));
            if (scrubber != null)
            {
                ScrubShort atr = scrubber as ScrubShort;
                ShortType attrScrubType = atr.type;

                switch (attrScrubType)
                {
                    default:
                        var defaultValue = Faker.RandomNumber.Next(short.MaxValue - 1);
                        return Convert.ToInt16(defaultValue);
                }
            }
            else
            {
                var defaultValue = Faker.RandomNumber.Next(short.MaxValue - 1);
                return Convert.ToInt16(defaultValue);
            }
        }

        /// <summary>
        /// Routes Decimal by scrubbing types
        /// </summary>
        /// <param name="property"></param>
        /// <param name="cleanDataInstance"></param>
        /// <param name="scrubType"></param>
        /// <returns></returns>
        private object RouteDecimalType()
        {
            var scrubber = _currentProperty.GetCustomAttribute(typeof(ScrubDecimal));
            if (scrubber != null)
            {
                ScrubDecimal atr = scrubber as ScrubDecimal;
                DecimalType attrScrubType = atr.type;

                switch (attrScrubType)
                {
                    default:
                        var defaultValue = Faker.RandomNumber.Next(50000) + new Random().NextDouble();
                        return Convert.ToDecimal(defaultValue);
                }
            }
            else
            {
                var defaultValue = Faker.RandomNumber.Next(50000) + new Random().NextDouble();
                return Convert.ToDecimal(defaultValue);
            }
        }

        /// <summary>
        /// Routes Doubles by scrubbing types
        /// </summary>
        /// <param name="property"></param>
        /// <param name="cleanDataInstance"></param>
        /// <param name="scrubType"></param>
        /// <returns></returns>
        private object RouteDoubleType()
        {
            var scrubber = _currentProperty.GetCustomAttribute(typeof(ScrubDouble));
            if (scrubber != null)
            {
                ScrubDouble atr = scrubber as ScrubDouble;
                DoubleType attrScrubType = atr.type;

                switch (attrScrubType)
                {
                    default:
                        var defaultValue = Faker.RandomNumber.Next(50000) + new Random().NextDouble();
                        return defaultValue;
                }
            }
            else
            {
                var defaultValue = Faker.RandomNumber.Next(50000) + new Random().NextDouble();
                return defaultValue;
            }
        }

        private object RouteLongType()
        {

            var scrubber = _currentProperty.GetCustomAttribute(typeof(ScrubLong));
            if (scrubber != null)
            {
                ScrubLong atr = scrubber as ScrubLong;
                LongType attrScrubType = atr.type;

                switch (attrScrubType)
                {
                    default:
                        var defaultValue = Faker.RandomNumber.Next(50000) + new Random().NextDouble();
                        return Convert.ToInt64(defaultValue);
                }
            }
            else
            {
                var defaultValue = Faker.RandomNumber.Next(50000) + new Random().NextDouble();
                return Convert.ToInt64(defaultValue);
            }
        }

        /// <summary>
        /// Routes String by scrubbing types
        /// </summary>
        /// <param name="property"></param>
        /// <param name="cleanDataInstance"></param>
        /// <param name="scrubType"></param>
        /// <returns></returns>
        private object RouteStringType()
        {
            var scrubber = _currentProperty.GetCustomAttribute(typeof(ScrubString));
            if (scrubber != null)
            {
                ScrubString atr = scrubber as ScrubString;
                StringType attrScrubType = atr.type;

                switch (attrScrubType)
                {
                    case StringType.SSN:
                        return ScrubBasicTypes.ScrubSSN();

                    case StringType.Phone:
                        return Faker.Phone.Number();

                    case StringType.Email:
                        return Faker.Internet.Email();

                    case StringType.Address:
                        return Faker.Address.StreetAddress();

                    case StringType.AddressTwo:
                        return Faker.Address.SecondaryAddress();

                    case StringType.City:
                        return Faker.Address.City();

                    case StringType.State:
                        return Faker.Address.UsState();

                    case StringType.Zip:
                        return Faker.Address.ZipCode();

                    case StringType.FullName:
                        return Faker.Name.FullName();

                    case StringType.FirstName:
                        return Faker.Name.First();

                    case StringType.LastName:
                        return Faker.Name.Last();

                    case StringType.MiddleName:
                        //Two First Names
                        return Faker.Name.First();

                    case StringType.UserName:
                        return Faker.Internet.UserName();

                    default:
                        return RouteGenericText(atr.length);

                }
            }
            else
            {
                return Faker.Lorem.Sentence(144);
            }
        }

        private object RouteArrayType(object value)
        {
            var val = (Array)value;
            //DOESN'T WORK AND NOT SURE HOW TO INTEGRATE THIS WITHOUT TONS OF CHANGES
            for (var i = 0; i < val.Length; i++)
            {
                //var va = property.GetValue(cleanDataInstance, new object[] { i });
            }

            return null;
        }

        private object RouteGenericText(int length)
        {
            const int wordToSentence = 8;
            switch (length)
            {
                case int i when i >= 0 && i <= 10:
                    return Faker.Lorem.Sentence(10);
                case int i when i >= 250:
                    return Faker.Lorem.Sentences(i / wordToSentence);
                default:
                    return Faker.Lorem.Sentence(length);
            }
        }
    }
}
