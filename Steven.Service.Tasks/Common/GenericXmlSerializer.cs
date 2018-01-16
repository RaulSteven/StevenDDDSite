using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Steven.Service.Tasks.Common
{
    public class GenericXmlSerializer<TProxy>
    {
        // ReSharper disable StaticFieldInGenericType
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(TProxy));
        // ReSharper restore StaticFieldInGenericType

        public static string ToXml(TProxy proxy)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            Serializer.Serialize(sw, proxy);

            return sb.ToString();
        }

        public static TProxy FromXml(string xml)
        {
            return (TProxy)Serializer.Deserialize(new XmlTextReader(new StringReader(xml)));
        }
    }
}
