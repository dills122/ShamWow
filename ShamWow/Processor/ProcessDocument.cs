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
    public class ProcessDocument : Router
    {
        private Type _type;
        private object _dataInstance;
        private DocumentManifest _manifest;
        private bool _IsScrubbed = false;
        private ScrubTypes _scrubType;

        public ProcessDocument(object dirtyDataInstance, ScrubTypes scrubType)
        {
            if (dirtyDataInstance == null)
            {
                throw new NullReferenceException("Data instance cannot be Null");
            }

            //Sets all the intial information
            _type = dirtyDataInstance.GetType();
            _dataInstance = dirtyDataInstance;
            _manifest = new DocumentManifest();
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

        private void ScrubCollections(List<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                ScrubListItems(prop);
            }
        }

        private void ScrubClasses(List<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                ScrubClass(prop);
            }
        }

        private void ScrubBaseTypes(List<PropertyInfo> properties)
        {
            foreach (var prop in properties)
            {
                RouteType(prop, ref _dataInstance, ref _scrubType);
            }
        }

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

        private List<PropertyInfo> GetClasses(bool IsFiltered)
        {
            var collection = _type.GetProperties().Where(p => IsClass(p))
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

        private List<PropertyInfo> FilterProperties(List<PropertyInfo> properties)
        {
            return properties.Where(p => ProcessingHelper.GetCustomAttributes(p, typeof(Scrub))).ToList();
        }

        private bool IsCollection(PropertyInfo property)
        {
            return property.PropertyType.Namespace.Contains("Collections");
        }

        private bool IsClass(PropertyInfo property)
        {
            return !property.PropertyType.Namespace.Contains("System");
        }

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

        private void ProcessManifestItems(List<DocumentManifestInfo> manifestInfos)
        {
            if (manifestInfos.Count > 0)
            {
                _manifest.documentManifestInfos.AddRange(manifestInfos);
            }
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
        /// Gets the value from the property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private object GetPropertyValue(PropertyInfo property)
        {
            return property.GetValue(_dataInstance, null);
        }

    }
}
