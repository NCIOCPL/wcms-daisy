using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;


namespace NCI.Web.CDE
{
    /// <summary>
    /// BlockedSlots contain information about the blocked slot which should not be displayed on the page rendered. 
    /// </summary>
    public class BlockedSlot
    {
        /// <summary>
        /// The name of the slot to be blocked.
        /// </summary>
        [XmlAttribute(AttributeName = "Name", Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }
    }
}
