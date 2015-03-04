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
            // Items in a collection returned by a Linq query aren't mutable.
            // To preserve any changes to the folders, we have to copy them over
            // to another collection.
            List<DaisyFolder> folderList = new List<DaisyFolder>();
            foreach (DaisyFolder entry in folders)
            {
                if(!entry.folder.StartsWith("/"))
                    entry.folder = "/" + entry.folder.Trim();
                folderList.Add(entry);
            }
            SerializeCollection<DaisyFolder>(folderList, @".\folder.xml");

            // ------------------
            // Content Extraction
            // ------------------

            //1. Setup Lists for site content
            List<DaisyContentItem> siteCommunity = new List<DaisyContentItem>();
            List<DaisyContentItem> siteAdminCommunity = new List<DaisyContentItem>();
            List<DaisyContentItem> ctbAdminCommunity = new List<DaisyContentItem>();

            //2. Loop through each sheet to find content sheets
            foreach (String sheetName in excel.GetWorksheetNames())
            {
                //All content item sheets will start with "2 CT "
                if (sheetName.ToLower().StartsWith("2 CT ") || sheetName.Equals("2 Content Items"))
                {
                    //We should probably check to make sure that content_type is not null, but 
                    //nothing else in this file generates errors.  This would end up being a
                    //validation error for Daisy input.

                    siteCommunity.AddRange(
                        ExtractItemsForCommunity(excel, sheetName, "site")                    
                        );

                    siteAdminCommunity.AddRange(
                        ExtractItemsForCommunity(excel, sheetName, "siteAdmin")
                        );

                    ctbAdminCommunity.AddRange(
                        ExtractItemsForCommunity(excel, sheetName, "ctbAdmin")
                        );
                }
            }
            
            //Now that we have looped through all the content sheets, output the Daisy Inputs

            SerializeCollection<DaisyContentItem>(siteCommunity, @".\siteContentItems.xml");           
            SerializeCollection<DaisyContentItem>(siteAdminCommunity, @".\siteAdminContentItems.xml");
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

        /// <summary>
        /// Extracts all the items from a sheet that are of a certain content type within a community
        /// </summary>
        /// <param name="excel">The excel file</param>
        /// <param name="sheetName">The name of the sheet</param>
        /// <param name="contentType">The content type</param>
        /// <param name="siteColumnNames"></param>
        /// <param name="community">The Community to get content for (site, siteAdmin, or ctbadmin)</param>
        /// <returns></returns>
        private static List<DaisyContentItem> ExtractItemsForCommunity(ExcelQueryFactory excel, String sheetName, string community)
        {
            var siteColumnNames = excel.GetColumnNames(sheetName);

            List<DaisyContentItem> items = ExtractItems(
                                            siteColumnNames,
                                            (
                                                //LINQ for all rows of a community
                                                from contentitem in excel.Worksheet(sheetName)
                                                where contentitem["community"] == community
                                                select contentitem
                                            ), 
                                            community
                                        );

            return items;
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
                        folder = CleanFolder(row["folder"])
                    };

                    PopulateItemFields(columnNames, row, item);

                    rtnItems.Add(item);
                }
            }

            return rtnItems;
        }

        private static string CleanFolder(string folder)
        {
            if (!folder.StartsWith("/"))
                return "/" + folder.Trim();
            else
                return folder;
        }

        private static void PopulateItemFields(IEnumerable<string> columnNames, Row row, DaisyContentItem item)
        {
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
