using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace MigrationEngine.Descriptors
{
    /// <summary>
    /// Business object representing a Percusison folder and its Navon.
    /// </summary>
    public class FolderDescription : ContentDescriptionBase
    {
        [XmlIgnore()]
        public String Path { get; set; }

        protected override string PropertyString
        {
            get
            {
                string fmt = @"<migrationId value=""{0}""/><path value=""{1}""/>";
                return string.Format(fmt, UniqueIdentifier, Path);
            }
        }
   }
}