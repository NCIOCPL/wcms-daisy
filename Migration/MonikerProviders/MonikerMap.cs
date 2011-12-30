using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace MonikerProviders
{
    [XmlRoot("MonikerMap")]
    public class MonikerMap
        : Dictionary<string, Moniker>, IXmlSerializable
    {
        public static void PersistToFile(MonikerMap map, string filename)
        {
            XmlSerializer serialzer = new XmlSerializer(typeof(MonikerMap));
            using (TextWriter writer = new StreamWriter(filename))
            {
                serialzer.Serialize(writer, map);
            }
        }

        public static MonikerMap LoadFromFile(string filename)
        {
            MonikerMap map = new MonikerMap();

            XmlSerializer serialzer = new XmlSerializer(typeof(MonikerMap));
            using (TextReader reader = new StreamReader(filename))
            {
                map = (MonikerMap)serialzer.Deserialize(reader);
            }

            return map;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            //XmlSerializer keySerializer = new XmlSerializer(typeof(string));
            //XmlSerializer valueSerializer = new XmlSerializer(typeof(Moniker));

            XmlSerializer itemSerializer = new XmlSerializer(typeof(Moniker));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //reader.ReadStartElement("Moniker");
                Moniker moniker = (Moniker)itemSerializer.Deserialize(reader);
                //reader.ReadEndElement();

                this.Add(moniker.Name, moniker);

                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            //XmlSerializer keySerializer = new XmlSerializer(typeof(string));
            //XmlSerializer valueSerializer = new XmlSerializer(typeof(Moniker));

            //foreach (string key in this.Keys)
            //{
            //    writer.WriteStartElement("item");

            //    writer.WriteStartElement("key");
            //    keySerializer.Serialize(writer, key);
            //    writer.WriteEndElement();

            //    writer.WriteStartElement("value");
            //    Moniker value = this[key];
            //    valueSerializer.Serialize(writer, value);
            //    writer.WriteEndElement();

            //    writer.WriteEndElement();
            //}

            XmlSerializer itemSerializer = new XmlSerializer(typeof(Moniker));
            foreach (Moniker moniker in this.Values)
            {
                itemSerializer.Serialize(writer, moniker);
            }
        }
    }
}
