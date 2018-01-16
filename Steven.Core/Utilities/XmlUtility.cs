using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Steven.Core.Utilities
{
    public class XmlUtility
    {

        public static string XmlSerialize<T>(T obj)
        {
            string xmlcontentstring = null;
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(ms, Encoding.UTF8);
            XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
            xmlSer.Serialize(xmlWriter, obj);
            xmlWriter.Close();
            xmlcontentstring = Encoding.UTF8.GetString(ms.ToArray());
            return xmlcontentstring;
        }

        public static T XmlDeserialize<T>(string xmlcontentstring)
        {
            T ret = default(T);
            byte[] buffer = Encoding.UTF8.GetBytes(xmlcontentstring);
            MemoryStream ms = new MemoryStream(buffer);
            XmlTextReader xmlReader = new XmlTextReader(ms);
            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            ret = (T)xmlSer.Deserialize(xmlReader);
            xmlReader.Close();
            return ret;
        } 
    }
}