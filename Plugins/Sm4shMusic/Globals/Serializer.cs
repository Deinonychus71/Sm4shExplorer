using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Sm4shMusic.Globals
{
    public static class Serializer
    {
        public static void SerializeObjectToXml<T>(T obj, string pathToSave)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StreamWriter wr = new StreamWriter(pathToSave))
            {
                xmlSerializer.Serialize(wr, obj);
            }
        }

        public static T DeserializeXmlToObject<T>(string pathToOpen)
        {
            T result;
            using (FileStream plainTextFile = new FileStream(pathToOpen, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                XmlReader reader = XmlReader.Create(plainTextFile, new XmlReaderSettings() { CheckCharacters = false });
                result = (T)xmlSerializer.Deserialize(reader);
            }
            return result;
        }
    }
}
