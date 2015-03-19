using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blu82.MigrationTasks
{
    public enum MigrationTaskTypes
    {
        Reset,
        FolderCreator,
        GeneralContentCreator,
        RelationshipCreator,
        ContentUpdater,
        LinkToFolder,
        Transitioner,
        TranslationRelationshipCreator
    }

    public class MigrationTask
    {
        public MigrationTaskTypes MigrationTaskType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }

        public string OutputXML()
        {
            if (String.IsNullOrEmpty(Name))
            {
                throw new NullReferenceException("MigrationTask Name must not be null");
            }

            StringBuilder xml = new StringBuilder();

            //Reset is wierd, so we will do this separately
            if (MigrationTaskType != MigrationTaskTypes.Reset)
            {
                xml.Append(
                    String.Format(
                        "\t\t<MigrationTask xsi:type=\"{0}\" Name=\"{1}\">\n",
                        MigrationTaskType.ToString(),
                        Name
                ));

                string datagetter = "";
                string datamapper = "";

                switch (MigrationTaskType)
                {
                    case MigrationTaskTypes.GeneralContentCreator:
                        datagetter = "XmlDataGetterOfFullItemDescription";
                        datamapper = "XmlFullItemDescriptionMapper";
                        break;                        
                    case MigrationTaskTypes.ContentUpdater:
                        datagetter = "XmlDataGetterOfUpdateContentItem";
                        datamapper = "XmlUpdateItemMapper";
                        break;                        
                    case MigrationTaskTypes.FolderCreator:
                        datagetter = "XmlDataGetterOfFolderDescription";
                        datamapper = "XmlFolderDescriptionMapper";
                        break;
                    case MigrationTaskTypes.LinkToFolder:
                        datagetter = "XmlDataGetterOfFolderLinkDescription";
                        datamapper = "XmlFolderLinkDescriptionMapper";
                        break;
                    case MigrationTaskTypes.RelationshipCreator:
                        datagetter = "XmlDataGetterOfRelationshipDescription";
                        datamapper = "XmlRelationshipDescriptionMapper";
                        break;
                    case MigrationTaskTypes.TranslationRelationshipCreator:
                        datagetter = "XmlDataGetterOfTranslationDescription";
                        datamapper = "XmlTranslationDescriptionMapper";
                        break;
                    case MigrationTaskTypes.Transitioner:
                        datagetter = "XmlDataGetterOfContentTypeTransitionDescription";
                        datamapper = "XmlContentTypeTransitionDescriptionMapper";
                        break;                        
                }

                if (!string.IsNullOrEmpty(datagetter) && !string.IsNullOrEmpty(datamapper))
                {
                    if (String.IsNullOrEmpty(Name))
                    {
                        throw new NullReferenceException("MigrationTask Filename must not be null");
                    }

                    xml.Append(String.Format(
                        "\t\t\t<DataGetter xsi:type=\"{0}\" FileName=\"{1}\">\n", 
                        datagetter,
                        Filename));
                    xml.Append(String.Format("\t\t\t\t<Mapper xsi:type=\"{0}\" />\n", datamapper));
                    xml.Append("\t\t\t</DataGetter>\n");

                }
                else
                {
                    Console.WriteLine("Unknown MigrationTask");
                    return "";
                }
            }
            else
            {
                xml.Append(
                    String.Format(
                        "\t\t<MigrationTask xsi:type=\"{0}\" Name=\"{1}\" MonikerReset=\"true\">\n",
                        MigrationTaskType.ToString(),
                        Name
                ));
            }

            xml.Append("\t\t</MigrationTask>\n");

            return xml.ToString();
        }
    }
}
