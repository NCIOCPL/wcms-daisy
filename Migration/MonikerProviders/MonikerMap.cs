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

            if (File.Exists(filename))
            {
                XmlSerializer serialzer = new XmlSerializer(typeof(MonikerMap));
                using (TextReader reader = new StreamReader(filename))
                {
                    map = (MonikerMap)serialzer.Deserialize(reader);
                }
            }

            return map;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlSerializer itemSerializer = new XmlSerializer(typeof(Moniker));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                Moniker moniker = (Moniker)itemSerializer.Deserialize(reader);

                this.Add(moniker.Name.ToLowerInvariant(), moniker);

                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer itemSerializer = new XmlSerializer(typeof(Moniker));
            foreach (Moniker moniker in this.Values)
            {
                itemSerializer.Serialize(writer, moniker);
            }
        }
    }
}
