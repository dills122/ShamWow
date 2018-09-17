using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ShamWow.Processor
{
    public static class ProcessingHelper
    {

        /// <summary>
        /// Duplicate
        /// </summary>
        /// <param name="property"></param>
        /// <param name="AttributeName"></param>
        /// <returns></returns>
        public static bool GetCustomAttributes(PropertyInfo property, Type AttributeName)
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
        public static List<Attribute> GetCustomAttributes(PropertyInfo property)
        {
            return property.GetCustomAttributes().ToList();
        }
    }

    public static class XmlHelper
    {
        public static string SerializeObject(object obj)
        {
            StringBuilder results = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings() { OmitXmlDeclaration = true };

            using (XmlWriter writer = XmlWriter.Create(results, settings))
            {
                new XmlSerializer(obj.GetType()).Serialize(writer, obj);
            }

            return results.ToString();
        }

        public static T DeserializeObject<T>(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(xml))
            {
                return (T)xs.Deserialize(sr);
            }
        }
    }
}
