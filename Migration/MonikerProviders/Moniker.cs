using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MonikerProviders
{
    [XmlRoot("Moniker")]
    public class Moniker
    {
        internal Moniker()
        {
        }

        public Moniker(string name, long contentID, string contentType)
        {
            Name = name;
            ContentID = contentID;
            ContentType = contentType;
        }

        public string Name { get; set; }

        public long ContentID { get; set; }

        public string ContentType { get; set; }

        public override int GetHashCode()
        {
            int hash = 27;
            hash += base.GetHashCode() * 13;
            hash += Name.GetHashCode() * 13;
            hash += ContentID.GetHashCode() * 13;
            hash += ContentType.GetHashCode() * 13;

            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Moniker rhs = obj as Moniker;
            if ((Object)rhs == null)
                return false;

            return this == rhs;
        }

        public static bool operator ==(Moniker lhs, Moniker rhs)
        {
            bool result;

            if ((Object)lhs != null && (Object)rhs != null)
                result = lhs.Name == rhs.Name
                    && lhs.ContentID == rhs.ContentID
                    && lhs.ContentType == rhs.ContentType;
            else
                result = (Object)lhs == (Object)rhs;

            return result;
        }

        public static bool operator !=(Moniker lhs, Moniker rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            string fmt = "{{Name: \"{0}\", ContentType: \"{1}\", ContentID: {2}}}";
            return string.Format(fmt, this.Name, this.ContentType, this.ContentID);
        }
    }
}
