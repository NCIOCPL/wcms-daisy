using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.ComponentModel;


namespace NCI.Web.CDE
{
    /// <summary>
    /// This class represents the properties of a PromoUrl.
    /// </summary>
    public class PromoUrl
    {
        /// <summary>
        /// Name of the promo url 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The MappedTo value is pretry url where the requested promo url will be directed.
        /// </summary>
        public string MappedTo { get; set; }
    }

    /// <summary>
    /// This class encapsulates all the information related to Promo Url.
    /// </summary>
    [System.Xml.Serialization.XmlRootAttribute("PromoUrlMapping", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class PromoUrlMapping : IXmlSerializable
    {
        /// <summary>
        /// A collection of all promo url item.
        /// </summary>
        public IDictionary<string, PromoUrl> PromoUrls { get; set; }
        public PromoUrlMapping()
        {
            PromoUrls = new Dictionary<string, PromoUrl>();
        }

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// This method reads the promo url mapping and populates the PromoUrls dictionary object
        /// The key is name of the promo url and the value is an object of promourl.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlSerializer valueSerializer = new XmlSerializer(typeof(PromoUrl));

            if (reader.IsEmptyElement)
                return;

            // Read to PromoUrls element
            reader.Read();
            if (reader.IsEmptyElement)
                return;

            while (reader.Read())
            {
                if (reader.LocalName == "PromoUrl")
                {
                    //Create instance of promo url
                    PromoUrl promoUrl = new PromoUrl();
                    string key = reader.GetAttribute("Name");
                    if (!string.IsNullOrEmpty(key))
                    {
                        key = key.ToLower();
                        promoUrl.Name = key;
                        reader.Read();
                        promoUrl.MappedTo = reader.ReadString();
                        if (!PromoUrls.ContainsKey(key))
                            PromoUrls.Add(key, promoUrl);
                        reader.ReadEndElement();
                    }
                    else
                    {
                        throw new Exception("name attribute cannot be null for the Promo URL");
                    }
                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            throw new Exception("This class does not support Serialization");
        }

        #endregion
    }
}


