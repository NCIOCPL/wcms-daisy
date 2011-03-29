using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

using MigrationEngine.BusinessObjects;
using MigrationEngine.Mappers;

namespace MigrationEngine.DataAccess
{
    public class XmlDataGetter<ReturnType>
        : DataGetter<ReturnType> where ReturnType : MigrationData
    {
        [XmlAttribute("FileName")]
        public String FileName;

        public DataMapper<ReturnType> Mapper;

        public override List<ReturnType> LoadData()
        {
            if (string.IsNullOrEmpty(FileName))
                throw new ArgumentNullException("The XML FileName was not specified.");

            List<ReturnType> loadedValues = new List<ReturnType>();

            XmlDocument dataFile = new XmlDocument();
            dataFile.Load(FileName);

            XmlNodeList topNodes = dataFile.SelectNodes("//item");
            foreach (XmlNode node in topNodes)
            {
                ReturnType item = Mapper.MapItem(node);
                loadedValues.Add(item);
            }

            return loadedValues;
        }
    }
}
