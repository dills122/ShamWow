using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ShamWow.SerializationHelpers
{
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
