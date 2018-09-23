using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ShamWow.Constants;
using ShamWow.Attributes;
using System.Linq;

namespace ShamWow.Processor
{
    public class ProcessDocument : Router, IProcessDocument
    {
        private Type _type;
        private object _dataInstance;
        private DocumentManifest _manifest;
        private bool _IsScrubbed = false;
        private ScrubTypes _scrubType;

        private ProcessDocument(object dirtyDataInstance, ScrubTypes scrubType)
        {
            if (dirtyDataInstance == null)
            {
                throw new NullReferenceException("Data instance cannot be Null");
            }

            //Sets all the intial information
            _type = dirtyDataInstance.GetType();
            _dataInstance = dirtyDataInstance;
            _scrubType = scrubType;
            _manifest = new DocumentManifest();
        }

        /// <summary>
        /// Returns a new Instance of the Processor
        /// </summary>
        /// <returns></returns>
        public static Factory GetFactory()
        {
            return new Factory((obj, scrub) => new ProcessDocument(obj, scrub));
        }

        /// <summary>
        /// Returns the data after scrubbing finished
        /// </summary>
        /// <returns></returns>
        public object CleanData()
        {
            if (_IsScrubbed)
            {
                return _dataInstance;
            }
            else
            {
                throw new NotSupportedException("Data has not been scrubbed yet");
            }
        }

        /// <summary>
        /// Returns the completed manifest
        /// </summary>
        /// <returns></returns>
        public DocumentManifest GetManifest()
        {
            return _manifest;
        }

        /// <summary>
        /// Initiates the Scrubbing Process
        /// </summary>
        /// <returns></returns>
        public ProcessDocument Scrub()
        {
            var IsFiltered =
                _scrubType == ScrubTypes.Marked ? true : false;

            ScrubCollections(GetCollections(IsFiltered));

            ScrubClasses(GetClasses(IsFiltered));

            ScrubBaseTypes(GetBaseTypes(IsFiltered));

            _IsScrubbed = true;

            return this;
        }

        /// <summary>
        /// Checks if the Manifest is Valid
        /// </summary>
        /// <returns></returns>
        public bool CheckManifest()
        {
            if (_IsScrubbed)
            {
                foreach (var item in _manifest.documentManifestInfos)
                {
                    if (String.Equals(item.cleanDataHash, item.dirtyDataHash))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Scrubs all Collection Type Properties
        /// </summary>
        /// <param name="properties"></param>
        private void ScrubCollections(List<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                ScrubListItems(prop);
            }
        }

        /// <summary>
        /// Scrubs all Class Type Properties
        /// </summary>
        /// <param name="properties"></param>
        private void ScrubClasses(List<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                ScrubClass(prop);
            }
        }

        /// <summary>
        /// Sends all Base Type Properties to be Scrubbed
        /// </summary>
        /// <param name="properties"></param>
        private void ScrubBaseTypes(List<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                var manifestInfo = RouteType(prop, ref _dataInstance, ref _scrubType);
                if (manifestInfo != null)
                {
                    _manifest.documentManifestInfos.Add(manifestInfo);
                }
            }
        }

        /// <summary>
        /// Gets a Collections of all Collections with the Object
        /// </summary>
        /// <param name="IsFiltered"></param>
        /// <returns></returns>
        private List<PropertyInfo> GetCollections(bool IsFiltered)
        {
            var collection = _type.GetProperties().Where(p => IsCollection(p))
                .Where(p => GetPropertyValue(p) != null)
                .ToList();

            if (IsFiltered)
            {
                return FilterProperties(collection);
            }
            else
            {
                return collection;
            }
        }

        /// <summary>
        /// Gets a Collection of all Classes within the Object
        /// </summary>
        /// <param name="IsFiltered"></param>
        /// <returns></returns>
        private List<PropertyInfo> GetClasses(bool IsFiltered)
        {
            var collection = _type.GetProperties().Where(p => IsClass(p))
                .Where(p => GetPropertyValue(p) != null)
                .ToList();

            //Cant filter classes since they are not required to be marked for inner properties to be scrubbed
            return collection;
        }

        /// <summary>
        /// Get All of the Base Type Properties
        /// </summary>
        /// <param name="IsFiltered"></param>
        /// <returns></returns>
        private List<PropertyInfo> GetBaseTypes(bool IsFiltered)
        {
            var collection = _type.GetProperties().Where(p => !IsClass(p) && !IsCollection(p))
                .Where(p => GetPropertyValue(p) != null)
                .ToList();

            if (IsFiltered)
            {
                return FilterProperties(collection);
            }
            else
            {
                return collection;
            }
        }

        /// <summary>
        /// Sets the class values
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Task ScrubClass(PropertyInfo property)
        {
            ProcessDocument process = new ProcessDocument(property.GetValue(_dataInstance), _scrubType)
                .Scrub();

            property.SetValue(_dataInstance, process.CleanData());

            ProcessManifestItems(process.GetManifest().documentManifestInfos);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Class scrubbing for List items
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Task<object> ScrubClass(object obj)
        {
            ProcessDocument process = new ProcessDocument(obj, _scrubType)
               .Scrub();

            ProcessManifestItems(process.GetManifest().documentManifestInfos);

            return Task.FromResult(process.CleanData());
        }

        /// <summary>
        /// Scrubs Collections types
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Task ScrubListItems(PropertyInfo property)
        {
            IList ilist = property.GetValue(_dataInstance, null) as IList;

            if (ilist.Count > 0)
            {
                for (int i = 0; i < ilist.Count; i++)
                {
                    var obj = ScrubClass(ilist[i]).Result;

                    //Sets the new object value
                    ilist[i] = obj;
                }

                property.SetValue(_dataInstance, ilist);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Filters out properties that aren't marked to be scrubbed
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private List<PropertyInfo> FilterProperties(List<PropertyInfo> properties)
        {
            return properties.Where(p => GetCustomAttributes(p, typeof(Scrub))).ToList();
        }

        /// <summary>
        /// Checks if the Property is a Collection
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsCollection(PropertyInfo property)
        {
            return property.PropertyType.Namespace.Contains("Collections");
        }

        /// <summary>
        /// Checks if the Property is a Class
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsClass(PropertyInfo property)
        {
            return !property.PropertyType.Namespace.Contains("System");
        }

        /// <summary>
        /// Processes the new incoming Manifest Items
        /// </summary>
        /// <param name="manifestInfos"></param>
        private void ProcessManifestItems(List<DocumentManifestInfo> manifestInfos)
        {
            if (manifestInfos.Count > 0)
            {
                _manifest.documentManifestInfos.AddRange(manifestInfos);
            }
        }

        /// <summary>
        /// Gets the value from the property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private object GetPropertyValue(PropertyInfo property)
        {
            return property.GetValue(_dataInstance, null);
        }

        /// <summary>
        /// Returns if the Attribute exists on the Property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        private bool GetCustomAttributes(PropertyInfo property, Type AttributeName)
        {

            var customAttribute = property.GetCustomAttributes().Where(a => a.TypeId == AttributeName).ToList();
            //Eh
            return customAttribute.Count == 1;
        }

        /// <summary>
        /// Returns all of the Custom Attributes on that property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private List<Attribute> GetCustomAttributes(PropertyInfo property)
        {
            return property.GetCustomAttributes().ToList();
        }

    }
}
