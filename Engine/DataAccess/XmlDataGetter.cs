using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using MigrationEngine.BusinessObjects;

namespace MigrationEngine.DataAccess
{
    public class XmlDataGetter<ReturnType>
        : DataGetter<ReturnType> where ReturnType : MigrationData
    {
        [XmlAttribute("FileName")]
        public String FileName = "datafile.xml";

        public override ReturnType LoadData()
        {
            throw new NotImplementedException();
        }
    }
}
