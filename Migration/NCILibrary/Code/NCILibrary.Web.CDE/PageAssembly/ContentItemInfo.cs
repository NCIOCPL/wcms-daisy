using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    public class ContentItemInfo
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ContentItemID { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ContentItemType { get; set; }

        public override bool Equals(object obj)
        {
            ContentItemInfo target = obj as ContentItemInfo;

            if (target == null)
                return false;

            if (ContentItemID != target.ContentItemID)
                return false;


            if (ContentItemType != target.ContentItemType)
                return false;

            return true;
        }
    }
}
