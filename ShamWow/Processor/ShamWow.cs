using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using ShamWow.Interfaces.Attributes;
using ShamWow.Constants;

namespace ShamWow.Processor
{
    /// <summary>
    /// ShamWow Cleaner that processes and scrubs files
    /// </summary>
    public class ShamWowEngine : IShamWow
    {
        private Router _router;
        private readonly Type _type;
        private object _dataInstance;
        private DocumentManifest _manifest;
        private bool _IsScrubbed = false;
        private ScrubMode _mode;

        private ShamWowEngine(object dirtyDataInstance, ScrubMode mode)
        {
            if (dirtyDataInstance == null)
            {
                throw new NullReferenceException("Data instance cannot be Null");
            }

            //Sets all the intial information
            _type = dirtyDataInstance.GetType();
            _dataInstance = dirtyDataInstance;
            _manifest = new DocumentManifest();
            _mode = mode;
            _router = new Router(_type, dirtyDataInstance);
        }

        private ShamWowEngine(object dirtyDataInstance, ScrubMode mode, Dictionary<string, object> stateValues)
        {
            if (dirtyDataInstance == null)
            {
                throw new NullReferenceException("Data instance cannot be Null");
            }

            //Sets all the intial information
            _type = dirtyDataInstance.GetType();
            _dataInstance = dirtyDataInstance;
            _manifest = new DocumentManifest();
            _mode = mode;
            _router = new Router(_type, _dataInstance, stateValues);
        }

        /// <summary>
        /// Returns a new Instance of the Processor
        /// </summary>
        /// <returns></returns>
        public static Factory GetFactory()
        {
            return new Factory((obj, scrub) => new ShamWowEngine(obj, scrub));
        }

        /// <summary>
        /// Returns the data after scrubbing finished
        /// </summary>
        /// <returns></returns>
        public object CleanData()
        {
            if (!_IsScrubbed)
            {
                throw new NotSupportedException("Data has not been scrubbed yet");
            }
            return _dataInstance;
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
        public ShamWowEngine Scrub()
        {

            ScrubCollections(GetCollections());

            ScrubClasses(GetClasses());

            ScrubBaseTypes(GetBaseTypes());

            _IsScrubbed = true;

            return this;
        }

        public Dictionary<string, object> GetStateValues()
        {
            if (!_IsScrubbed)
            {
                throw new NotSupportedException("Data has not been scrubbed yet");
            }
            return _router.GetValues();
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
                _router.ScrubProperty(prop, ref _dataInstance);
                var manifestInfo = _router._manifestItem;

                if (manifestInfo != null)
                {
                    _manifest.documentManifestInfos.Add(manifestInfo);
                }
            }
        }

        /// <summary>
        /// Gets a Collections of all Collections with the Object
        /// </summary>
        /// <returns></returns>
        private List<PropertyInfo> GetCollections()
        {
            var collection = _type.GetProperties().Where(p => IsCollection(p))
                .Where(p => GetPropertyValue(p) != null)
                .Where(p => p.GetCustomAttribute<PreserveValueAttribute>() == null)  // If a property has [PreserveValue], then don't return it in the collection to scrub
                .ToList();

            return FilterProperties(collection);
        }

        /// <summary>
        /// Gets a Collection of all Classes within the Object
        /// </summary>
        /// <returns></returns>
        private List<PropertyInfo> GetClasses()
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
        private List<PropertyInfo> GetBaseTypes()
        {
            var collection = _type.GetProperties().Where(p => !IsClass(p) && !IsCollection(p))
                .Where(p => !IsPropertyNullOrEmpty(p))
                .Where(p => p.GetCustomAttribute<PreserveValueAttribute>() == null)  // If a property has [PreserveValue], then don't return it in the collection to scrub
                .ToList();

            return FilterProperties(collection);
        }

        /// <summary>
        /// Sets the class values
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Task ScrubClass(PropertyInfo property)
        {
            ShamWowEngine process = new ShamWowEngine(property.GetValue(_dataInstance), _mode, _router.GetValues())
                .Scrub();

            _router.MergeStateValues(process.GetStateValues());

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
            ShamWowEngine process = new ShamWowEngine(obj, _mode, _router.GetValues())
               .Scrub();

            _router.MergeStateValues(process.GetStateValues());

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
            return property.CanRead
                ? property.GetValue(_dataInstance, null)
                : null;
        }

        private List<PropertyInfo> FilterProperties(List<PropertyInfo> properties)
        {
            if (_mode == ScrubMode.Marked)
            {
                var attributeList = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace == "ShamWow.Interfaces.Attributes")
                .ToList();

                return properties.Where(prop => prop.GetCustomAttributes()
                .Where(a => attributeList.Contains(a.GetType())) != null)
                .ToList();
            }
            return properties;
        }

        /// <summary>
        /// Checks if a value is a default value
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsPropertyNullOrEmpty(PropertyInfo property)
        {
            var val = property.GetValue(_dataInstance, null);

            if (val == null)
            {
                return true;
            }

            var type = val.GetType();

            object obj = type.IsValueType ? Activator.CreateInstance(type) : null;

            return Equals(val, obj);
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
