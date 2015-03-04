using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCI.CMS.Percussion;
using NCI.CMS.Percussion.Manager.CMS;
using NCI.CMS.Percussion.Manager.PercussionWebSvc;
using Gbu43.Configuration;
using Gbu43.Storage;
using OfficeOpenXml;
using System.IO;

namespace Gbu43
{
    /// <summary>
    /// The purpose of this program is to extract content from Percussion to speed up creation of migration
    /// spreadsheets.  This is a work in progress.
    /// 
    /// </summary>
    class Program
    {
        private static readonly int _MIG_ID_COL = 1;
        private static readonly int _COMMUNITY_COL = 2;
        private static readonly int _CONTENT_TYPE_COL = 3;
        private static readonly int _FOLDER_COL = 4;

        private static ContentExtractorConfiguration _config = new ContentExtractorConfiguration();
        private static Dictionary<string, ContentTypeSheet> _contentSheets = new Dictionary<string, ContentTypeSheet>();

        static void Main(string[] args)
        {            

            string folder = "/Configuration";
            using (CMSController controller = new CMSController("CancerGov_Configuration"))
            {

                ExtractFolder(controller, folder, true);
            }


            Console.WriteLine("Completed Extracting");
            OutputSheets();
            Console.WriteLine("Done");
            Console.Read();
        }

        private static void ExtractFolder(CMSController controller, string folderPath, bool recurse)
        {
            Console.WriteLine(folderPath);
            ExtractFolder(controller, folderPath, recurse, 0);
        }

        private static string Tabs(int num)
        {
            StringBuilder tabs = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                tabs.Append("\t");
            }

            return tabs.ToString();
        }

        private static void ExtractFolder(CMSController controller, string folderPath, bool recurse, int level)
        {
            PSItemSummary[] contentItems = controller.FindFolderChildren(folderPath);

            //We don't want to loop through everything as it may cause performance issues
            //let's find all folders and items of types that we want.  Then we can loop through those bits.
                        
            PSItemSummary[] subFolders = contentItems.Where(item => item.objectType == ObjectType.folder).ToArray();
            PSItemSummary[] itemsToFetch = contentItems.Where(item => item.objectType == ObjectType.item && _config.IsTypeToExtract(item.ContentType.name)).ToArray();

            //Get the content items of everything we are supposed to extract
            //TODO: Handle this in batches
            PSItem[] items = controller.LoadContentItems(itemsToFetch.Select(item => item.id).ToArray());

            foreach (PSItem item in items)
            {
                ContentTypeSheetItem sheetItem = new ContentTypeSheetItem();
                

                //Set Common Elements
                sheetItem.MigID = "Extracted_" + PercussionGuid.GetID(item.id);
                sheetItem.Folder = folderPath;
                sheetItem.ContentType = item.contentType;

                //Extract Fields
                //TODO: Handle Files
                foreach (PSField field in item.Fields)
                {                    
                    if (_config.GetFieldsForType(item.contentType).Contains(field.name))
                    {
                        if (field.PSFieldValue != null && field.PSFieldValue.Length == 1)
                        {
                            sheetItem.Add(field.name, field.PSFieldValue[0].RawData);
                        }
                        else
                        {
                            Console.WriteLine("Content Item " + sheetItem.MigID + ", Field, " + field.name + " Value is NULL or Empty");
                        }
                    }
                }

                ContentTypeSheet sheet = GetContentSheetForType(item.contentType);
                sheet.Add(sheetItem);
            }

            //Recurse and Extract Sub Folders
            foreach (PSItemSummary folder in subFolders)
            {
                //TODO: Ensure file path is not an exclude path

                string subFolderPath = folderPath + "/" + folder.name;
                Console.WriteLine(subFolderPath);

                if (recurse)
                    ExtractFolder(controller, subFolderPath, recurse, level++);
            }
        }

        private static void OutputSheets()
        {
            FileInfo outputFile = new FileInfo(@".\output.xlsx");
            if (outputFile.Exists)
            {
                outputFile.Delete();
                outputFile = new FileInfo(@".\output.xlsx");
            }

            //1. Create Workbook
            using (var package = new ExcelPackage(outputFile))
            {
                var sheets = _contentSheets.Values.OrderBy(sheet => sheet.SheetName);
                foreach (ContentTypeSheet sheet in sheets)
                {
                    Console.WriteLine("Processing Sheet: " + sheet.SheetName);

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheet.SheetName);
                    
                    //Fixed Header Items
                    worksheet.Cells[1, _MIG_ID_COL].Value = "mig_id";
                    worksheet.Cells[1, _COMMUNITY_COL].Value = "community";
                    worksheet.Cells[1, _CONTENT_TYPE_COL].Value = "contenttype";
                    worksheet.Cells[1, _FOLDER_COL].Value = "folder";

                    int rowOffset = 2; //1 for spreadsheets being 1-index based and 2 for Header row 

                    //Create dictionary to map fields to columns.
                    Dictionary<string, int> fieldColMap = new Dictionary<string, int>();

                    for (int sheetItemIndex = 0; sheetItemIndex < sheet.Count; sheetItemIndex++)
                    {
                        ContentTypeSheetItem sheetItem = sheet[sheetItemIndex];

                        //Fixed Column Items
                        worksheet.Cells[sheetItemIndex + rowOffset, _MIG_ID_COL].Value = sheetItem.MigID;
                        worksheet.Cells[sheetItemIndex + rowOffset, _COMMUNITY_COL].Value = "TEST";
                        worksheet.Cells[sheetItemIndex + rowOffset, _CONTENT_TYPE_COL].Value = sheetItem.ContentType;
                        worksheet.Cells[sheetItemIndex + rowOffset, _FOLDER_COL].Value = sheetItem.Folder;

                        foreach (string fieldName in sheetItem.Keys)
                        {
                            if (!fieldColMap.Keys.Contains(fieldName))
                            {
                                //Basically we need to know what column number to use for this field name.
                                //We know that folder is the last fixed column before the fields start,
                                //so our columns will start at (Folder Column Number + 1).  
                                //
                                //Next, we will take the next available slot, which should be at
                                //(number of keys for fields found so far + 1).
                                fieldColMap.Add(fieldName, fieldColMap.Keys.Count + (_FOLDER_COL + 1));
                                
                                //Add Column Header
                                worksheet.Cells[1, fieldColMap[fieldName]].Value = fieldName;
                            }

                            //Set the cell for this sheet item's field.
                            worksheet.Cells[sheetItemIndex + rowOffset, fieldColMap[fieldName]].Value = sheetItem[fieldName];

                        }
                    }
                    
                    
                }
                package.Save();
            }
        }

        private static ContentTypeSheet GetContentSheetForType(string contentType)
        {
            string sheetName = _config.GetSheetNameForType(contentType);

            if (_contentSheets.ContainsKey(sheetName))
            {
                return _contentSheets[sheetName];
            }
            else
            {
                _contentSheets.Add(sheetName, new ContentTypeSheet(sheetName));
                return _contentSheets[sheetName];
            }
        }

        /*
         * Relationship
         * ownerid | dependentid | slot | template
         * 
         * Translations
         * english_id | spanish_id
         * 
         */

    }
}
