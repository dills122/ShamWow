using ShamWow.Interfaces.Attributes;
using ShamWow.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ShamWow.Exceptions;
using ShamWow.Scrubbers;

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
        public Router(Type type, object dataInstance, Dictionary<string, object> stateValues)
        {
            MergeStateValues(stateValues);
            FillStateValues(type, dataInstance);
        }

        public Dictionary<string, object> GetValues()
        { return _stateValues; }

        public void FillStateValues(Type type, object dataInstance)
        {
            var props = type.GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(StatefulScrub)));

            foreach (var prop in props)
            {
                var val = prop.GetValue(dataInstance, null);
                _currentProperty = prop;
                var scrubbedValue = RouteValue(val);
                var keyValue = GetStateKeyValue(prop);

                if (!_stateValues.ContainsKey(keyValue))
                {
                    _stateValues.Add(keyValue, scrubbedValue);
                }
                //Reset to avoid issues with multiples
                _currentProperty = null;
            }
        }

        private string GetStateKeyValue(PropertyInfo property)
        {
            var attr = property.GetCustomAttribute(typeof(StatefulScrub));

            var stateAttr = attr as StatefulScrub;

            if (stateAttr == null)
            {
                return string.Empty;
            }
            return stateAttr.valueName;
        }

        public void MergeStateValues(Dictionary<string, object> stateValues)
        {
            var merged = _stateValues.Concat(stateValues)
                .GroupBy(i => i.Key)
                .ToDictionary(group => group.Key, group => group.First().Value);

            if (merged.Count > 0)
            {
                _stateValues = merged;
            }
        }

        public void ScrubProperty(PropertyInfo property, ref object dataInstance)
        {
            object cleanValue;

            _currentProperty = property;

            var dirtyValue = property.GetValue(dataInstance, null);

            var predefinedValue = RoutePredefinedType();

            if (predefinedValue != null)
            {
                SetPropertyValue(dirtyValue, predefinedValue, dataInstance);
                _currentProperty = null;
                return;
            }

            //Check if its stateful
            var key = GetStateKeyValue(property);

            if (string.IsNullOrEmpty(key))
            {
                cleanValue = RouteValue(dirtyValue);

                SetPropertyValue(dirtyValue, cleanValue, dataInstance);
            }
            else
            {
                SetPropertyValue(dirtyValue, _stateValues[key], dataInstance);
            }

            _currentProperty = null;
        }

        private void SetPropertyValue(object dirtyValue, object cleanValue, object dataInstance)
        {
            if (_currentProperty.PropertyType.IsArray)
            {
                Array arry = (Array)cleanValue;
                var elementType = dirtyValue.GetType().GetElementType();
                Array destinationArray = Array.CreateInstance(elementType, arry.Length);
                Array.Copy(arry, destinationArray, arry.Length);

                _currentProperty.SetValue(dataInstance, destinationArray);
            }
            else
            {
                _currentProperty.SetValue(dataInstance, _currentProperty.PropertyType.Name == "String" ? Convert.ToString(cleanValue) : cleanValue);
            }
            CreateManifestItem(_currentProperty, dirtyValue, cleanValue);
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
                IntegerScrubber attrScrubType = atr.scrubber;

                switch (attrScrubType)
                {
                    case IntegerScrubber.Zip:
                        return Convert.ToInt32(Faker.Address.ZipCode().Replace("-", string.Empty));
                    default:
                        return Faker.RandomNumber.Next(10000000, 99999999);
                }
            }
            else
            {
                return Faker.RandomNumber.Next(10000000, 99999999);
            }
        }

        private object RoutePredefinedType()
        {
            var propAttribute = _currentProperty.GetCustomAttribute(typeof(PredefinedValue));
            if (propAttribute != null)
            {
                PredefinedValue atr = propAttribute as PredefinedValue;
                var value = atr.value;
                return value;
            }
            return null;
        }

        private object RouteByteType()
        {
            //Documents
            if (_currentProperty.PropertyType.IsArray)
            {
                return ScrubFiles.ScrubFile();
            }
            return null;
        }

        private object RouteShortType()
        {
            var scrubber = _currentProperty.GetCustomAttribute(typeof(ScrubShort));
            if (scrubber != null)
            {
                ScrubShort atr = scrubber as ScrubShort;
                ShortScrubber attrScrubType = atr.scrubber;

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
                DecimalScrubber attrScrubType = atr.scrubber;

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
                DoubleScrubber attrScrubType = atr.scrubber;

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
                LongScrubber attrScrubType = atr.scrubber;

                switch (attrScrubType)
                {
                    case LongScrubber.Phone:
                        var strValue = new string(Faker.Phone.Number().Where(char.IsDigit).ToArray());
                        Int64.TryParse(strValue, out long result);
                        return result;
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
                StringScrubber attrScrubType = atr.scrubber;
                bool IsRedacted = atr.IsRedacted;

                if (IsRedacted)
                {
                    return "*** REDACTED ***";
                }

                switch (attrScrubType)
                {
                    case StringScrubber.SSN:
                        return ScrubBasicTypes.ScrubSSN();

                    case StringScrubber.Phone:
                        return Faker.Phone.Number();

                    case StringScrubber.Email:
                        return Faker.Internet.Email();

                    case StringScrubber.Address:
                        return Faker.Address.StreetAddress();

                    case StringScrubber.AddressTwo:
                        return Faker.Address.SecondaryAddress();

                    case StringScrubber.City:
                        return Faker.Address.City();

                    case StringScrubber.State:
                        return Faker.Address.UsState();

                    case StringScrubber.CountyFIPS:
                        return ScrubCountyCode.ScrubCountyFIPS();

                    case StringScrubber.CountyName:
                        return ScrubCountyCode.ScrubCountyName();

                    case StringScrubber.StateCode:
                        return Faker.Address.UsStateAbbr();

                    case StringScrubber.Zip:
                        return Faker.Address.ZipCode();

                    case StringScrubber.FullName:
                        return Faker.Name.FullName();

                    case StringScrubber.FirstName:
                        return Faker.Name.First();

                    case StringScrubber.LastName:
                        return Faker.Name.Last();

                    case StringScrubber.MiddleName:
                        //Two First Names
                        return Faker.Name.First();

                    case StringScrubber.UserName:
                        return Faker.Internet.UserName();

                    case StringScrubber.Number:
                        var minValue = atr.minValue;
                        var maxValue = atr.maxValue;
                        return Faker.RandomNumber.Next(minValue, maxValue);

                    case StringScrubber.VIN:
                        return ScrubBasicTypes.ScrubVIN();

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
            var arrayInstance = (Array)value;

            var tempArray = new object[arrayInstance.Length];

            for (var i = 0; i < arrayInstance.Length; i++)
            {
                var valueType = arrayInstance.GetValue(i);

                switch (valueType)
                {
                    case int ints:
                        tempArray[i] = RouteIntegerType();
                        break;
                    case byte[] bs:
                    case byte b:
                        tempArray[i] = RouteByteType();
                        break;
                    case short s:
                        tempArray[i] = RouteShortType();
                        break;
                    case double d:
                        tempArray[i] = RouteDoubleType();
                        break;
                    case decimal dec:
                        tempArray[i] = RouteDecimalType();
                        break;
                    case string s:
                        tempArray[i] = RouteStringType();
                        break;
                    case long l:
                        tempArray[i] = RouteLongType();
                        break;
                    case bool b:
                    case DateTime d:
                        // Don't scrub these types, so just break here
                        break;

                    default:
                        throw new NotSupportedTypeException($"Type not supported by Array scrubber yet: {arrayInstance.GetType().GetElementType()}");
                }
            }
            return tempArray;
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
