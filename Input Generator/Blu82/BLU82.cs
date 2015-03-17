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

            //Get folders (but only where the folder is not null -- empty rows can result in empt DaisyFolder items
            var folders = from folder in excel.Worksheet<DaisyFolder>("1 Folders")
                          where (!string.IsNullOrEmpty(folder.folder))
                          select folder;
            // Items in a collection returned by a Linq query aren't mutable.
            // To preserve any changes to the folders, we have to copy them over
            // to another collection.
            List<DaisyFolder> folderList = new List<DaisyFolder>();
            foreach (DaisyFolder entry in folders)
            {
                if (!entry.folder.StartsWith("/"))
                    entry.folder = "/" + entry.folder.Trim();
                folderList.Add(entry);
            }

            if (folderList.Count > 0)
            {
                SerializeCollection<DaisyFolder>(folderList, @".\folder.xml");
            }
            else
            {
                Console.WriteLine("There are no folders to save");
            }

            // ------------------
            // Content Extraction
            // ------------------

            //1. Setup Lists for site content            
            List<DaisyContentItem> newSiteCommunity = new List<DaisyContentItem>();
            List<DaisyContentItem> newSiteAdminCommunity = new List<DaisyContentItem>();
            List<DaisyContentItem> newCTBAdminCommunity = new List<DaisyContentItem>();

            List<DaisyContentItem> updateSiteCommunity = new List<DaisyContentItem>();
            List<DaisyContentItem> updateSiteAdminCommunity = new List<DaisyContentItem>();
            List<DaisyContentItem> updateCTBAdminCommunity = new List<DaisyContentItem>();

            //2. Loop through each sheet to find content sheets
            foreach (String sheetName in excel.GetWorksheetNames())
            {
                //All content item sheets will start with "2 CT "
                if (
                    sheetName.StartsWith("2 CT ", StringComparison.InvariantCultureIgnoreCase) 
                    || sheetName.Equals("2 Content Items", StringComparison.InvariantCultureIgnoreCase)
                ){
                    Console.WriteLine("Extracting Sheet: " + sheetName);

                    //Daisy Handles Updates differently than with creates, so we need to follow
                    //the Daisy moniker rules.  Long or folder path (for navon) is an update,
                    //and anything that is not a long or does not start with a '/' is a new item.

                    newSiteCommunity.AddRange(
                        ExtractNewItemsForCommunity(excel, sheetName, "site")                    
                        );

                    newSiteAdminCommunity.AddRange(
                        ExtractNewItemsForCommunity(excel, sheetName, "siteAdmin")
                        );

                    newCTBAdminCommunity.AddRange(
                        ExtractNewItemsForCommunity(excel, sheetName, "ctbAdmin")
                        );

                    updateSiteCommunity.AddRange(
                        ExtractUpdateItemsForCommunity(excel, sheetName, "site")
                        );

                    updateSiteAdminCommunity.AddRange(
                        ExtractUpdateItemsForCommunity(excel, sheetName, "siteAdmin")
                        );

                    updateCTBAdminCommunity.AddRange(
                        ExtractUpdateItemsForCommunity(excel, sheetName, "ctbAdmin")
                        );

                }
            }
            
            //Now that we have looped through all the content sheets, output the Daisy Inputs

            if (newSiteCommunity.Count > 0)
            {
                SerializeCollection<DaisyContentItem>(newSiteCommunity, @".\newSiteContentItems.xml");
            }
            else
            {
                Console.WriteLine("No new site items to save");
            }

            if (newSiteAdminCommunity.Count > 0) {
                SerializeCollection<DaisyContentItem>(newSiteAdminCommunity, @".\newSiteAdminContentItems.xml");
            }
            else
            {
                Console.WriteLine("No new site admin items to save");
            }

            if (newCTBAdminCommunity.Count > 0) {
                SerializeCollection<DaisyContentItem>(newCTBAdminCommunity, @".\newCTBAdminContentItems.xml");
            }
            else
            {
                Console.WriteLine("No new CTB Admin items to save");
            }

            if (updateSiteCommunity.Count > 0) {
                SerializeCollection<DaisyContentItem>(updateSiteCommunity, @".\updateSiteContentItems.xml");
            }
            else
            {
                Console.WriteLine("No updated site items to save");
            }

            if (updateSiteAdminCommunity.Count > 0)
            {
                SerializeCollection<DaisyContentItem>(updateSiteAdminCommunity, @".\updateSiteAdminContentItems.xml");
            }
            else
            {
                Console.WriteLine("No updated site admin items to save");
            }

            if (updateCTBAdminCommunity.Count > 0) {
                SerializeCollection<DaisyContentItem>(updateCTBAdminCommunity, @".\updateCTBAdminContentItems.xml");
            }
            else
            {
                Console.WriteLine("No updated CTB admin items to save");
            }


            // Get Share to Folders
            var shareItems = from shareTo in excel.Worksheet<DaisyFolderLink>("3 Share To")
                              where (!string.IsNullOrEmpty(shareTo.mig_id))
                              select shareTo;
            if (shareItems.Count() > 0)
            {
                SerializeCollection<DaisyFolderLink>(shareItems, @".\shareTo.xml");
            }
            else
            {
                Console.WriteLine("No 'share to' items to save");
            }
             
            //Get relationships
            var relationships = from relationship in excel.Worksheet<DaisyRelationships>("4 Relationships")
                                where (!string.IsNullOrEmpty(relationship.ownerid))
                                select relationship;

            if (relationships.Count() > 0)
            {
                SerializeCollection<DaisyRelationships>(relationships, @".\relationships.xml");
            }
            else
            {
                Console.WriteLine("No relationships to save");
            }

            // Get translations
            var translations = from translation in excel.Worksheet<DaisyTranslation>("5 Translations")
                               where (!string.IsNullOrEmpty(translation.english_id))
                               select translation;

            if (translations.Count() > 0)
            {
                SerializeCollection<DaisyTranslation>(translations, @".\translations.xml");
            }
            else
            {
                Console.WriteLine("No translations to save");
            }

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
        private static List<DaisyContentItem> ExtractNewItemsForCommunity(ExcelQueryFactory excel, String sheetName, string community)
        {
            var siteColumnNames = excel.GetColumnNames(sheetName);

            var sheetItems = excel.Worksheet(sheetName).ToList()
                .Where( 
                    contentitem => contentitem["community"] == community
                    && (contentitem["mig_id"] != null && contentitem["mig_id"].ToString().Trim() != "")
                    && (!contentitem["mig_id"].ToString().StartsWith("/") && !IsLong(contentitem["mig_id"].ToString()))
                );

            List<DaisyContentItem> items = ExtractItems(
                                            siteColumnNames,
                                            sheetItems, 
                                            community
                                        );

            return items;
        }

        /// <summary>
        /// Extracts all the update items from a sheet that are of a certain content type within a community
        /// </summary>
        /// <param name="excel">The excel file</param>
        /// <param name="sheetName">The name of the sheet</param>
        /// <param name="contentType">The content type</param>
        /// <param name="siteColumnNames"></param>
        /// <param name="community">The Community to get content for (site, siteAdmin, or ctbadmin)</param>
        /// <returns></returns>
        private static List<DaisyContentItem> ExtractUpdateItemsForCommunity(ExcelQueryFactory excel, String sheetName, string community)
        {
            var siteColumnNames = excel.GetColumnNames(sheetName);


            var sheetItems = excel.Worksheet(sheetName).ToList()
                             .Where( 
                                contentitem => contentitem["community"] == community
                                && (contentitem["mig_id"] != null && contentitem["mig_id"].ToString().Trim() != "")
                                && (contentitem["mig_id"].ToString().StartsWith("/") || IsLong(contentitem["mig_id"].ToString()))
                              );


            List<DaisyContentItem> items = ExtractItems(
                                            siteColumnNames,
                                            sheetItems,
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
        private static List<DaisyContentItem> ExtractItems(IEnumerable<string> columnNames, IEnumerable<Row> contentitems, string community)
        {
            List<DaisyContentItem> rtnItems = new List<DaisyContentItem>();

            Console.WriteLine("Extracting Items: " + community);

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

                    Console.WriteLine("Extracting Item: " + item.mig_id);

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

        /// <summary>
        /// Tests if a string is long.  Helper for determining mig_id type.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsLong(string s)
        {
            bool isLong = false;

            try
            {
                long.Parse(s);
                isLong = true;
            }
            catch { }

            return isLong;
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
