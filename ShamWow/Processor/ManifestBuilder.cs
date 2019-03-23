using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.Data.HashFunction.xxHash;
using System.Linq;
using System.Reflection;

namespace ShamWow.Processor
{
    /// <summary>
    /// Builds a manifest that contains a hashed version of the data before/after scrubbing
    /// </summary>
    public static class ManifestBuilder
    {
        private static readonly IxxHash xxHash = xxHashFactory.Instance.Create();

        public static string HashValue(string value)
        {
            if (value != null || !String.IsNullOrEmpty(value))
            {
                var hash = xxHash.ComputeHash(value);
                return hash.AsBase64String();
            }
            return string.Empty;
        }

        public static string HashValue(object value)
        {
            try
            {
                //Specific for Byte Array, Need to find better solution
                Type valueType = value.GetType();
                if(valueType.IsArray && valueType.Name.Contains("Byte"))
                {
                    value = Convert.ToBase64String((byte[])value);
                }

                var hash = xxHash.ComputeHash(value.ToString());
                return hash.AsBase64String();
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public static DocumentManifestInfo AddManifestInfo(PropertyInfo property, ref object dataObject, bool IsDirty = false, DocumentManifestInfo info = null)
        {
            var prop = $"{property.Name}-{property.PropertyType.ToString().Split('.').Last()}";

            if (info == null)
            {
                return IsDirty ? new DocumentManifestInfo() { property = prop, dirtyDataHash = HashValue(property.GetValue(dataObject)) } :
                                 new DocumentManifestInfo() { property = prop, cleanDataHash = HashValue(property.GetValue(dataObject)) };
            }
            else
            {
                if (IsDirty)
                {
                    info.dirtyDataHash = HashValue(property.GetValue(dataObject, null));
                    return info;
                }
                else
                {
                    info.cleanDataHash = HashValue(property.GetValue(dataObject, null));
                    return info;
                }
            }
        }

        public static DocumentManifestInfo CreateManifestInfo(PropertyInfo property, object dirtyValue, object cleanValue)
        {
            var prop = $"{property.Name}-{property.PropertyType.ToString().Split('.').Last()}";
            return new DocumentManifestInfo
            {
                cleanDataHash = HashValue(cleanValue),
                dirtyDataHash = HashValue(dirtyValue),
                property = prop
            };
        }
    }

    public class DocumentManifest
    {
        public Guid manifestId { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public List<DocumentManifestInfo> documentManifestInfos { get; set; }
        public DocumentManifest()
        {
            manifestId = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            documentManifestInfos = new List<DocumentManifestInfo>();
        }
    }

    public class DocumentManifestInfo
    {
        public string property { get; set; }
        public bool IsStateful { get; set; }
        public string dirtyDataHash { get; set; }
        public string cleanDataHash { get; set; }

        public DocumentManifestInfo() { }

        public DocumentManifestInfo(string property, string dirtyValue, string cleanValue)
        {
            this.property = property;
            this.dirtyDataHash = ManifestBuilder.HashValue(dirtyDataHash);
            this.cleanDataHash = ManifestBuilder.HashValue(cleanValue);
        }
    }
}
