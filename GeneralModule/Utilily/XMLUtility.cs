using System;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace GeneralModule.Utilily
{
        public static class XMLUtilily
        {
            public static T EleToEntity<T>(string strEle)
            {
                T obj = Activator.CreateInstance<T>();
                if (String.IsNullOrWhiteSpace(strEle))
                {
                    return obj;
                }
                try
                {
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(strEle)))
                    {
                        XmlSerializer serializer = new XmlSerializer(obj.GetType());
                        object serializedObj = serializer.Deserialize(stream);
                        return (T)serializedObj;
                    }
                }
                catch { }
                return obj;
            }
            public static XElement EntityToEle(Object objEntiry)
            {
                if (objEntiry == null) { return null; }

                XmlSerializer serializer = new XmlSerializer(objEntiry.GetType());
                MemoryStream stream = new MemoryStream();
                try
                {
                    serializer.Serialize(stream, objEntiry);
                    string strEle = Encoding.UTF8.GetString(stream.ToArray());
                    return XElement.Parse(strEle);
                }
                catch { return null; }
                finally { stream.Close(); }
            }
        }
    }
  