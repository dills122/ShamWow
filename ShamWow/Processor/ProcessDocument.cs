using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ShamWow.Constants;
using ShamWow.Attributes;

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

            //Iterates through all properties of the source type
            foreach (var property in _type.GetProperties())
            {
                RouteMajorPropertyType(property);
            }

            _IsScrubbed = true;

            return this;
        }

        public bool CheckManifest()
        {
            if (_IsScrubbed)
            {
                foreach(var item in _manifest.documentManifestInfos)
                {
                    if(String.Equals(item.cleanDataHash,item.dirtyDataHash))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private void RouteMajorPropertyType(PropertyInfo property)
        {
            if (ToScrub(property))
            {
                //Checks if the property is a class
                if (!property.PropertyType.Namespace.Contains("System"))
                {
                    var manifestItems = ScrubClass(property).Result;
                    //Adds manifest items from inner class
                    if (manifestItems.Count > 0)
                    {
                        _manifest.documentManifestInfos.AddRange(manifestItems);
                    }
                }
                else if (property.PropertyType.Namespace.Contains("Collections"))
                {
                    ScrubListItems(property).Wait();
                }
                else
                {
                    //var dirtyValue = GetPropertyValue(property);

                    ScrubProperty(property);

                    //var cleanValue = GetPropertyValue(property);

                   // _manifest.documentManifestInfos.Add(ManifestBuilder.CreateManifestInfo(property, dirtyValue, cleanValue));
                }
            }
        }

        /// <summary>
        /// Sets the class values
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private Task<List<DocumentManifestInfo>> ScrubClass(PropertyInfo property)
        {
            var obj = property.GetValue(_dataInstance);
            if (obj != null)
            {
                ProcessDocument process = new ProcessDocument(obj, _scrubType)
                    .Scrub();

                property.SetValue(_dataInstance, process.CleanData());
                return Task.FromResult(process.GetManifest().documentManifestInfos);
            }
            return Task.FromResult(new List<DocumentManifestInfo>());
        }

        /// <summary>
        /// Class scrubbing for List items
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private Task<Tuple<object,List<DocumentManifestInfo>>> ScrubClass(object obj)
        {
            object cleanedClass = null;

            if (obj != null)
            {
                ProcessDocument process = new ProcessDocument(obj, _scrubType)
                    .Scrub();
                cleanedClass = process.CleanData();
                return Task.FromResult(new Tuple<object, List<DocumentManifestInfo>>(cleanedClass, process._manifest.documentManifestInfos));
            }

            return Task.FromResult(new Tuple<object, List<DocumentManifestInfo>>(cleanedClass, new List<DocumentManifestInfo>()));
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
                    var tup = ScrubClass(ilist[i]).Result;
                    if (tup.Item1 != null)
                    {
                        //Sets the new object value
                        ilist[i] = tup.Item1;
                    }

                    if(tup.Item2.Count > 0)
                    {
                        _manifest.documentManifestInfos.AddRange(tup.Item2);
                    }
                }

                property.SetValue(_dataInstance, ilist);
            }
            return Task.CompletedTask;
        }

        private void ScrubProperty(PropertyInfo property)
        {
            var manifestInfo = RouteType(property, ref _dataInstance, ref _scrubType);
            if(manifestInfo != null)
            {
                _manifest.documentManifestInfos.Add(manifestInfo);
            }
        }

        /// <summary>
        /// Checks if the property should be scrubbed
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool ToScrub(PropertyInfo property)
        {
            if (_scrubType == ScrubTypes.Full)
            {
                return true;
            }
            else if (_scrubType == ScrubTypes.Marked)
            {
                if (ProcessingHelper.GetCustomAttributes(property, typeof(Scrub)) && GetPropertyValue(property) != null)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
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
