using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NCI.Web.CDE
{
    public class LocalFieldCollection : IEnumerable<LocalField>, IXmlSerializable
    {
        private Dictionary<string, LocalField> _localFields = new Dictionary<string, LocalField>();


        public void Add(LocalField localField)
        {
            if (localField == null)
            {
                throw new ArgumentNullException("localField");
            }

            if (_localFields.ContainsKey(localField.Name) == true)
            {
                throw new PageAssemblyException(String.Format("The local field with name \"{0}\" cannot be added to the collection, there is already an field with that name.", localField.Name));
            }

            _localFields.Add(localField.Name, localField);
        }

        public int Count
        {
            get
            {
                return _localFields.Count;
            }
        }

        public LocalField this[string name]
        {
            get
            {
                if (_localFields.ContainsKey(name) == false)
                {
                    throw new PageAssemblyException(String.Format("The local fields collection does not contain a field with name \"{0}.\"", name));
                }

                return _localFields[name];
            }
        }

        #region IEnumerable<LocalField> Members

        public IEnumerator<LocalField> GetEnumerator()
        {
            return _localFields.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _localFields.Values.GetEnumerator();
        }

        #endregion

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            //string localFieldInfoXml = reader.ReadOuterXml();
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(localFieldInfoXml);
            //foreach (XmlElement localFieldElement in doc.DocumentElement.ChildNodes)
            //{
            //    // Add a new local field element.
            //    LocalField localField = new LocalField();
            //    localField.Name = localFieldElement.Name;
            //    localField.Value = localFieldElement.InnerText;
            //    Add(localField);
            //}


            while (reader.Read())
            {
                if ((reader.LocalName == "LocalFields") && (reader.IsStartElement() == false)) { reader.Read(); break; }

                switch (reader.LocalName)
                {
                    case "Field":
                        {
                            LocalField localField = new LocalField();
                            localField.Name = reader.GetAttribute("Name");
                            localField.Value = reader.ReadString();
                            Add(localField);                            
                        }
                        break;
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            // TODO: implement.
        }

        #endregion
    }
}
