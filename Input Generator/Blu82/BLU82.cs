using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using LinqToExcel;
using System.IO;

namespace Blu82
{
    class BLU82
    {
        private static void GenerateDaisyInput(string inputFile)
        {
            var excel = new ExcelQueryFactory(inputFile);
            //excel.AddTransformation<DaisyFolder>(x => x.mig_id, cellValue => new Guid(cellValue));
            //// Technically, this line should probably be required, but LinqToExcel throws an exctption
            //// about a duplicate key.  The tranformation for DaisyFolder seems to carry over though,
            //// so this works because of magic.
            ////excel.AddTransformation<DaisyContentItem>(x => x.mig_id, cellValue => new Guid(cellValue));
            //excel.AddTransformation<DaisyRelationships>(x => x.ownerid, cellValue => new Guid(cellValue));
            //excel.AddTransformation<DaisyRelationships>(x => x.dependentid, cellValue => new Guid(cellValue));

            //Get folders
            var folders = from folder in excel.Worksheet<DaisyFolder>("1 Folders")
                          select folder;

            SerializeCollection<DaisyFolder>(folders, @".\folder.xml");

            var siteColumnNames = excel.GetColumnNames("2 Content Items");

            //Get Site content items            
            var siteContentitems = from contentitem in excel.Worksheet("2 Content Items")
                                   where contentitem["community"] == "site"
                                   select contentitem;
            List<DaisyContentItem> siteCommunity = ExtractItems(siteColumnNames, siteContentitems, "site");
            SerializeCollection<DaisyContentItem>(siteCommunity, @".\siteContentItems.xml");

            //Get Site Admin content items
            var siteAdminContentitems = from contentitem in excel.Worksheet("2 Content Items")
                                        where contentitem["community"] == "siteAdmin"
                                        select contentitem;
            List<DaisyContentItem> siteAdminCommunity = ExtractItems(siteColumnNames, siteAdminContentitems, "siteAdmin");
            SerializeCollection<DaisyContentItem>(siteAdminCommunity, @".\siteAdminContentItems.xml");

            //Get CTB Admin content items
            var ctbAdminContentitems = from contentitem in excel.Worksheet("2 Content Items")
                                       where contentitem["community"] == "ctbAdmin"
                                       select contentitem;
            List<DaisyContentItem> ctbAdminCommunity = ExtractItems(siteColumnNames, ctbAdminContentitems, "ctbAdmin");
            SerializeCollection<DaisyContentItem>(ctbAdminCommunity, @".\ctbAdminContentItems.xml");

            // Get Share to Folders
            var folderLinks = from shareTo in excel.Worksheet<DaisyFolderLink>("3 Share To")
                              select shareTo;
            SerializeCollection<DaisyFolderLink>(folderLinks, @".\shareTo.xml");

            //Get relationships
            var relationships = from relationship in excel.Worksheet<DaisyRelationships>("4 Relationships")
                                select relationship;
            SerializeCollection<DaisyRelationships>(relationships, @".\relationships.xml");

            // Get translations
            var translations = from translation in excel.Worksheet<DaisyTranslation>("5 Translations")
                               select translation;
            SerializeCollection<DaisyTranslation>(translations, @".\translations.xml");

            Console.WriteLine("Press Enter");
            Console.Read();
        }

        private static void SerializeCollection<T>(IEnumerable<T> objToSer, String fileName)
        {
            DaisyCollection<T> collection = new DaisyCollection<T>() { Items = objToSer.ToArray() };

            XmlSerializer serializer = new XmlSerializer(typeof(DaisyCollection<T>), new XmlRootAttribute("list"));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            TextWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, collection, ns);
            writer.Close();
        }

        /// <summary>
        /// Extracts DaisyContentItem instances from the a collection of rows from a sheet.
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="contentitems"></param>
        /// <param name="community"></param>
        /// <returns></returns>
        private static List<DaisyContentItem> ExtractItems(IEnumerable<string> columnNames, IQueryable<Row> contentitems, string community)
        {
            List<DaisyContentItem> rtnItems = new List<DaisyContentItem>();

            foreach (Row row in contentitems)
            {
                if (!String.IsNullOrEmpty(row["mig_id"]))
                {
                    DaisyContentItem item = new DaisyContentItem()
                    {
                        community = community,
                        mig_id = row["mig_id"],
                        contenttype = row["contenttype"],
                        folder = row["folder"]
                    };

                    //Add additional unknown fields
                    foreach (String columnName in columnNames.Where(name => { return name != "community" && name != "mig_id" && name != "contenttype" && name != "folder"; }))
                    {
                        string val = row[columnName];
                        if (!string.IsNullOrEmpty(val))
                        {
                            if (columnName.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
                            {
                                string fileFieldName = columnName.Remove(0, 5);
                                WebFileInfo fileInfo = WebFileInfo.DownloadFile(val);

                                item.Fields.Add(fileFieldName, fileInfo.Data);
                                item.Fields.Add(fileFieldName + "_ext", fileInfo.Extension);
                                item.Fields.Add(fileFieldName + "_filename", fileInfo.FileName);
                                item.Fields.Add(fileFieldName + "_size", fileInfo.FileSize.ToString());
                                item.Fields.Add(fileFieldName + "_type", fileInfo.MimeType);

                                //item_file_attachment
                                //item_file_attachment_ext
                                //item_file_attachment_filename
                                //item_file_attachment_size
                                //item_file_attachment_type
                            }
                            else if (columnName.StartsWith("image:", StringComparison.OrdinalIgnoreCase))
                            {
                                string imageFieldName = columnName.Remove(0, 6);
                                ImageInfo fileInfo = ImageInfo.DownloadImage(val);

                                item.Fields.Add(imageFieldName, fileInfo.Data);
                                item.Fields.Add(imageFieldName + "_ext", fileInfo.Extension);
                                item.Fields.Add(imageFieldName + "_filename", fileInfo.FileName);
                                item.Fields.Add(imageFieldName + "_size", fileInfo.FileSize.ToString());
                                item.Fields.Add(imageFieldName + "_type", fileInfo.MimeType);
                                item.Fields.Add(imageFieldName + "_width", fileInfo.Width.ToString());
                                item.Fields.Add(imageFieldName + "_height", fileInfo.Height.ToString());

                                //img1
                                //img1_ext
                                //img1_filename
                                //img1_height
                                //img1_size
                                //img1_type
                                //img1_width

                            }
                            else
                            {
                                item.Fields.Add(columnName, val);
                            }
                        }
                    }

                    rtnItems.Add(item);
                }
            }

            return rtnItems;
        }

        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                GenerateDaisyInput(args[0]);
            }
            else
            {
                Console.WriteLine("Syntax: BLU82 <scriptname>");
            }
        }
    }
}
