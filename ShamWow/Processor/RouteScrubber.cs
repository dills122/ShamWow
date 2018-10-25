using ShamWow.Attributes;
using ShamWow.Scrubbers;
using System;
using System.Reflection;
using ShamWow.Constants;
using ShamWow.Exceptions;

namespace ShamWow.Processor
{
    //TODO Rewrite Router to work with all types and follow new standards
    public abstract class Router
    {
        private object _cleanData;
        private object _dirtyData;
        private DocumentManifestInfo _manifestoInfo;

        private void CleanUp()
        {
            _cleanData = null;
            _dirtyData = null;
        }

        public DocumentManifestInfo RouteType(PropertyInfo property, ref object dataInstance, ref ScrubType scrubType)
        {
            var type = property.GetValue(dataInstance, null);

            switch (type)
            {
                case decimal d:
                    RouteDecimalType(property, ref dataInstance, scrubType);
                    break;
                case Int32 i:
                    RouteIntegerType(property, ref dataInstance, scrubType);
                    break;
                case Double d:
                    RouteDoubleType(property, ref dataInstance, scrubType);
                    break;
                case String s:
                    RouteStringType(property, ref dataInstance, scrubType);
                    break;
                default:
                    throw new NotSupportedTypeException("Type not supported by scrubber yet");
            }

            CreateManifestoItem(property);
            //Cleans up the gobal data values before returning
            CleanUp();

            return _manifestoInfo;
        }

        //Implement the rest of the Attributes
        private object RouteStringType(PropertyInfo property, ref object cleanDataInstance, ScrubType scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            var data = property.GetCustomAttribute(typeof(StringAtr));
            if (data != null)
            {
                StringAtr atr = data as StringAtr;
                StringType attrScrubType = atr.scrubType;

                switch (attrScrubType)
                {
                    case StringType.SSN:
                        property.SetValue(cleanDataInstance, ScrubBasicTypes.ScrubSSN());
                        break;
                    case StringType.Phone:
                        property.SetValue(cleanDataInstance, Faker.Phone.Number());
                        break;
                    case StringType.Email:
                        property.SetValue(cleanDataInstance, Faker.Internet.Email());
                        break;
                    case StringType.Address:
                        property.SetValue(cleanDataInstance, Faker.Address.StreetAddress());
                        break;
                    case StringType.AddressTwo:
                        property.SetValue(cleanDataInstance, Faker.Address.SecondaryAddress());
                        break;
                    case StringType.City:
                        property.SetValue(cleanDataInstance, Faker.Address.City());
                        break;
                    case StringType.State:
                        property.SetValue(cleanDataInstance, Faker.Address.UsState());
                        break;
                    case StringType.Zip:
                        property.SetValue(cleanDataInstance, Faker.Address.ZipCode());
                        break;
                    case StringType.FullName:
                        property.SetValue(cleanDataInstance, Faker.Name.FullName());
                        break;
                    case StringType.FirstName:
                        property.SetValue(cleanDataInstance, Faker.Name.First());
                        break;
                    case StringType.LastName:
                        property.SetValue(cleanDataInstance, Faker.Name.Last());
                        break;
                    case StringType.MiddleName:
                        //Two First Names
                        property.SetValue(cleanDataInstance, Faker.Name.First());
                        break;
                    case StringType.UserName:
                        property.SetValue(cleanDataInstance, Faker.Internet.UserName());
                        break;
                    case StringType.Lorem:
                        property.SetValue(cleanDataInstance, Faker.Lorem.Sentence(atr.length));
                        break;
                    default:
                        //TODO Update to lorem
                        property.SetValue(cleanDataInstance, Faker.Lorem.Sentence());
                        break;
                }

                _cleanData = property.GetValue(cleanDataInstance, null);
            }
            else if (scrubType == ScrubType.Full)
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


        private object RouteDoubleType(PropertyInfo property, ref object cleanDataInstance, ScrubType scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            return cleanDataInstance;
        }

        private object RouteDecimalType(PropertyInfo property, ref object cleanDataInstance, ScrubType scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            return cleanDataInstance;
        }

        private object RouteIntegerType(PropertyInfo property, ref object cleanDataInstance, ScrubType scrubType)
        {
            _dirtyData = property.GetValue(cleanDataInstance, null);

            var data = property.GetCustomAttribute(typeof(IntAtr));
            if (data != null)
            {
                IntAtr atr = data as IntAtr;
                IntType attrScrubType = atr.scrubType;

                switch (attrScrubType)
                {
                    case IntType.Zip:
                        property.SetValue(cleanDataInstance, Convert.ToInt32(Faker.Address.ZipCode().Replace("-", string.Empty)));
                        break;
                    case IntType.VIN:
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
