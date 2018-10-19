using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.Data.HashFunction.xxHash;
using System.Reflection;

namespace ShamWow.Processor
{
    public static class ManifestBuilder
    {
        public static readonly IxxHash xxHash = xxHashFactory.Instance.Create();

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
                var hash = xxHash.ComputeHash(value.ToString());
                return hash.AsBase64String();
            }
            catch(Exception ex)
            {
                return String.Empty;
            }
        }

        public static DocumentManifestInfo AddManifestInfo(PropertyInfo property, ref object dataObject, bool IsDirty = false,DocumentManifestInfo info = null)
        {
            if(info == null)
            {
                return IsDirty ? new DocumentManifestInfo() { property = property, dirtyDataHash = HashValue(property.GetValue(dataObject)) } :
                                 new DocumentManifestInfo() { property = property, cleanDataHash = HashValue(property.GetValue(dataObject)) };
            }
            else
            {
                if(IsDirty)
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
            return new DocumentManifestInfo
            {
                cleanDataHash = HashValue(cleanValue),
                dirtyDataHash = HashValue(dirtyValue),
                property = property
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
        public PropertyInfo property { get; set; }
        public string dirtyDataHash { get; set; }
        public string cleanDataHash { get; set; }

        public DocumentManifestInfo()
        {

        }

        public DocumentManifestInfo(PropertyInfo property, string dirtyValue, string cleanValue)
        {
            this.property = property;
            this.dirtyDataHash = ManifestBuilder.HashValue(dirtyDataHash);
            this.cleanDataHash = ManifestBuilder.HashValue(cleanValue);
        }
    }
}
