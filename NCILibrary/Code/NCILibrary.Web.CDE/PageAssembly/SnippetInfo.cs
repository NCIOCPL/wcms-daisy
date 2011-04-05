using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
namespace NCI.Web.CDE
{
    /// <summary>
    /// Holds the snippet information used to populate a slot on the page.
    /// </summary>
    public class SnippetInfo : IXmlSerializable
    {
        private string _snippetData = null;
        private bool correctedCDATA = false;
        private List<DisplayVersions> listOnlyDisplayFor = new List<DisplayVersions>();
        /// <summary>
        /// Gets and sets the path to the user control that will render this
        /// snippet.
        /// </summary>
        public string SnippetTemplatePath { get; set; }
        /// <summary>
        /// html data to be displayed on the page
        /// </summary>
        public string Data 
        {
            get 
            {
                // The snippet CDATA may contain CDATA as part of the data but percussion replaces the CDATA 
                // close tag with Replace ']]>' with ']]ENDCDATA' this ']]ENDCDATA' should be replaced with 
                // valid CDATA close tag ']]>' before it can be deserialized
                if (!correctedCDATA && !string.IsNullOrEmpty(_snippetData))
                {
                    _snippetData = _snippetData.Replace("]]ENDCDATA", "]]>");
                    correctedCDATA = true;
                }
                return _snippetData; 
            }
            set { _snippetData = value; } 
        }
        /// <summary>
        /// Slot to be used on the page rendered
        /// </summary>
        public string SlotName { get; set; }

        public string ContentID { get; set; }

        public DisplayVersions[] OnlyDisplayFor
        {
            get
            {

                return listOnlyDisplayFor.ToArray();
            }
        }

        ///// <summary>
        ///// The id of the CDR definition. 
        ///// </summary>
        //public string CDRId { get; set; }

        ///// <summary>
        ///// The name of the CDR Definition.
        ///// </summary>
        //public string CDRDefinitionName { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            SnippetInfo target = obj as SnippetInfo;

            if (target == null)
                return false;

            if (ContentID != target.ContentID)
                return false;

            if (SnippetTemplatePath != target.SnippetTemplatePath)
                return false;

            if (SlotName != target.SlotName)
                return false;

            if (Data != target.Data)
                return false;

            return true;
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            while (reader.Read())
            {
                if ((reader.LocalName == "SnippetInfo") && (reader.IsStartElement() == false)) { reader.Read(); break; }

                switch (reader.LocalName)
                {
                    case "Data":
                        {
                            Data = reader.ReadString();
                        }
                        break;
                    case "SnippetTemplatePath":
                        {
                            SnippetTemplatePath = reader.ReadString();
                        }
                        break;
                    case "SlotName":
                        {
                            SlotName = reader.ReadString();
                        }
                        break;
                    case "ContentID":
                        {
                            ContentID = reader.ReadString();
                        }
                        break;
                    //case "CDRId":
                    //    string cdrId = reader.ReadString();
                    //    if (!string.IsNullOrEmpty(cdrId))
                    //        CDRId = cdrId;
                    //    break;

                    //case "CDRDefinitionName":
                    //    CDRDefinitionName = reader.ReadString();
                    //    break;

                    case "DisplayVersion":
                        {
                            listOnlyDisplayFor.Add((DisplayVersions)Enum.Parse(typeof(DisplayVersions), reader.ReadString()));
                        }
                        break;


                }
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("SnippetTemplatePath", SnippetTemplatePath);
            writer.WriteElementString("SlotName", SlotName);
            writer.WriteStartElement("Data");
            writer.WriteCData(Data);
            writer.WriteElementString("ContentID", ContentID);

            writer.WriteEndElement();
        }

        #endregion
    }
}
