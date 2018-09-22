using ShamWow.Attributes;
using ShamWow.Scrubbers;
using System;
using System.Reflection;
using ShamWow.Constants;
using ShamWow.Exceptions;

namespace ShamWow.Processor
{
    //TODO Rewrite Router to work with all types and follow new standards
    public class Router
    {
        private object _cleanData;
        private object _dirtyData;
        private DocumentManifestInfo _manifestoInfo;

        private void CleanUp()
        {
            _cleanData = null;
            _dirtyData = null;
        }


        public DocumentManifestInfo RouteType(PropertyInfo property, ref object dataInstance, ref ScrubTypes scrubType)
        {
            if (Equals(property.PropertyType, typeof(System.Int32)))
            {
                RouteIntegerType(property, ref dataInstance, scrubType);
            }
            else if (Equals(property.PropertyType, typeof(System.Double)))
            {
                RouteDoubleType(property, ref dataInstance, scrubType);
            }
            else if (Equals(property.PropertyType, typeof(System.Decimal)))
            {
                RouteDecimalType(property, ref dataInstance, scrubType);
            }
            else if (Equals(property.PropertyType, typeof(System.String)))
            {
                RouteStringType(property, ref dataInstance, scrubType);
            }
            else
            {
                throw new NotSupportedTypeException("Type not supported by scrubber yet");
            }

            CreateManifestoItem(property);
            //Cleans up the gobal data values before returning
            CleanUp();

            return _manifestoInfo;
        }

        //Implement the rest of the Attributes
        public object RouteStringType(PropertyInfo property, ref object cleanDataInstance, ScrubTypes scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            var data = property.GetCustomAttribute(typeof(StringAtr));
            if (data != null)
            {
                StringAtr atr = data as StringAtr;
                string attrScrubType = atr.scrubType;

                switch (attrScrubType)
                {
                    case "SSN":
                        property.SetValue(cleanDataInstance, ScrubBasicTypes.ScrubSSN());
                        break;
                    case "Phone":
                        property.SetValue(cleanDataInstance, Faker.Phone.Number());
                        break;
                    case "Email":
                        property.SetValue(cleanDataInstance, Faker.Internet.Email());
                        break;
                    case "Address":
                        property.SetValue(cleanDataInstance, Faker.Address.StreetAddress());
                        break;
                    case "AddressTwo":
                        property.SetValue(cleanDataInstance, Faker.Address.SecondaryAddress());
                        break;
                    case "City":
                        property.SetValue(cleanDataInstance, Faker.Address.City());
                        break;
                    case "State":
                        property.SetValue(cleanDataInstance, Faker.Address.UsState());
                        break;
                    case "Zip":
                        property.SetValue(cleanDataInstance, Faker.Address.ZipCode());
                        break;
                    case "FullName":
                        property.SetValue(cleanDataInstance, Faker.Name.FullName());
                        break;
                    case "FirstName":
                        property.SetValue(cleanDataInstance, Faker.Name.First());
                        break;
                    case "LastName":
                        property.SetValue(cleanDataInstance, Faker.Name.Last());
                        break;
                    case "MiddleName":
                        //Two First Names
                        property.SetValue(cleanDataInstance, Faker.Name.First());
                        break;
                    case "UserName":
                        property.SetValue(cleanDataInstance, Faker.Internet.UserName());
                        break;
                    default:
                        //TODO Update to lorem
                        property.SetValue(cleanDataInstance, Faker.Lorem.Sentence());
                        break;
                }

                _cleanData = property.GetValue(cleanDataInstance, null);
            }
            else if (scrubType == ScrubTypes.Full)
            {
                property.SetValue(cleanDataInstance, Faker.Lorem.Sentence());

                _cleanData = property.GetValue(cleanDataInstance, null);
            }

            return cleanDataInstance;
        }

        private void CreateManifestoItem(PropertyInfo property)
        {
            if (!object.Equals(_cleanData, _dirtyData) && _cleanData != null && _dirtyData != null)
            {
                _manifestoInfo = ManifestBuilder.CreateManifestInfo(property, _dirtyData, _cleanData);
            }
        }


        public object RouteDoubleType(PropertyInfo property, ref object cleanDataInstance, ScrubTypes scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            return cleanDataInstance;
        }

        public object RouteDecimalType(PropertyInfo property, ref object cleanDataInstance, ScrubTypes scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            return cleanDataInstance;
        }

        public object RouteIntegerType(PropertyInfo property, ref object cleanDataInstance, ScrubTypes scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            var data = property.GetCustomAttribute(typeof(IntAtr));
            if (data != null)
            {
                IntAtr atr = data as IntAtr;
                string attrScrubType = atr.scrubType;

                switch (attrScrubType)
                {
                    case "Zip":
                        property.SetValue(cleanDataInstance, Convert.ToInt32(Faker.Address.ZipCode().Replace("-", string.Empty)));
                        break;
                    case "Phone":
                        //property.SetValue(cleanDataInstance, Convert.ToInt32(Faker.Phone.Number().Replace("-", string.Empty)));
                        break;
                    case "VIN":
                        //TODO improve
                        property.SetValue(cleanDataInstance, Faker.RandomNumber.Next(10000000, 99999999));
                        break;
                }
            }
            else
            {
                property.SetValue(cleanDataInstance, Faker.RandomNumber.Next(10000, 56400));
            }
            return cleanDataInstance;
        }
    }
}
