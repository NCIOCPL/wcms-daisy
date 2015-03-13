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
        //
        public enum ContentSheetCols
        {
            MigID = 1,
            Community = 2,
            ContentType = 3,
            Folder = 4
        }

        public enum RelationSheetCols
        {
            OwnerID = 1,
            DependentID = 2,
            Slot = 3,
            Template = 4
        }

        private static ContentExtractorConfiguration _config = new ContentExtractorConfiguration();
        private static Dictionary<string, ContentTypeSheet> _contentSheets = new Dictionary<string, ContentTypeSheet>();
        private static RelationshipSheet _relationSheet = new RelationshipSheet();
        private static TemplateNameManager _templateNameManager = null;
        private static DirectoryInfo _outputFolder = null;

        static void Main(string[] args)
        {
            string outputFolderPath = @"./output";
            //Set output folder based on command args
            if (args.Length == 1)
            {
                outputFolderPath = args[0];
            }

            #region Setup Output folder

            try { _outputFolder = new DirectoryInfo(outputFolderPath); }
            catch (Exception ex)
            {
                Console.WriteLine("Folder, " + outputFolderPath + ", is not valid, " + ex.ToString());
                return;
            }

            if (_outputFolder.Exists)
            {
                try {
                    _outputFolder.Delete(true);
                    Console.WriteLine("Deleted Folder, " + _outputFolder.ToString());
                } catch (Exception ex) {
                    Console.WriteLine("Could not delete folder, " + _outputFolder + ", " + ex.ToString());
                }
            }
            
            try
            {
                _outputFolder.Create();
                Console.WriteLine("Created Folder, " + _outputFolder.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not create Folder, " + _outputFolder.ToString() + ", Error: " + ex.ToString());
                return;
            }

            #endregion

            string folder = "/Configuration";
            using (CMSController controller = new CMSController("CancerGov_Configuration"))
            {

                _templateNameManager = controller.TemplateNameManager;
                ExtractFolder(controller, folder, true);
            }

            Console.WriteLine("Completed Extracting");
            PrepareSheetsForSaving();
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
            PSItem[] items = controller.LoadContentItems(itemsToFetch.Select(item => item.id).ToArray(), true); //Include Binaries

            foreach (PSItem item in items)
            {
                //If we should extract the item (based on type configuration and the id
                //then extract.
                if (_config.ShouldExtractItem(item.contentType, PercussionGuid.GetID(item.id)))
                {
                    

                    ContentTypeSheetItem sheetItem = new ContentTypeSheetItem(
                        PercussionGuid.GetID(item.id), 
                        _config.OnlyStoreCTIfDependent(item.contentType),
                        _config.ExtractAsUpdate(item.contentType, item.id));

                    //Set Common Elements
                    sheetItem.Folder = folderPath;
                    sheetItem.ContentType = item.contentType;
                    

                    //Pull fields into dictionary so that we can grab fields in an ad-hoc manner
                    Dictionary<string, PSField> fields = item.Fields.ToDictionary(f => f.name);

                    //The following should not happen, but if it does, let's not blow up.
                    if (!fields.ContainsKey("sys_communityid"))
                    {
                        Console.WriteLine("Error: Item, " + item.id + ", does not have a community ID");
                        continue;
                    }

                    sheetItem.Community = GetCommunityForSheet(controller, fields["sys_communityid"]);

                    #region Extract Fields

                    //Loop through each field to extract
                    foreach (string fieldName in _config.GetFieldsForType(item.contentType))
                    {
                        if (fieldName.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
                        {
                            string fileFieldName = fieldName.Substring(5); //Strip off "file:"

                            if (!fields.ContainsKey(fileFieldName))
                            {
                                Console.WriteLine("Field, " + fileFieldName + ", does not exist");
                                continue;
                            }

                            //Get Filename
                            if (!fields.ContainsKey(fileFieldName + "_filename"))
                            {
                                Console.WriteLine("Could not get filename for Field, " + fileFieldName);
                                continue;
                            }

                            string fileName = fields[fileFieldName + "_filename"].GetFirstValue();

                            if (String.IsNullOrEmpty(fileName))
                            {
                                Console.WriteLine("Empty filename for Field, " + fileFieldName);
                                continue;
                            }

                            //Save File
                            DirectoryInfo dir = new DirectoryInfo(_outputFolder.FullName + folderPath);
                            if (!dir.Exists) { dir.Create(); }

                            if (fields[fileFieldName].SaveFieldAsFile(_outputFolder.FullName + folderPath + "\\" + fileName))
                            {
                                sheetItem.Add(fieldName, "file://." + folderPath + "/" + fileName);
                            }
                        }
                        else
                        {
                            if (fields.ContainsKey(fieldName))
                            {

                                string fieldVal = fields[fieldName].GetFirstValue();
                                if (fieldVal != null)
                                {
                                    sheetItem.Add(fieldName, fieldVal);
                                }
                                else
                                {
                                    Console.WriteLine("Content Item " + sheetItem.MigID + ", Field, " + fieldName + " Value is NULL or Empty");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Field, " + fieldName + ", is not a valid field for " + item.contentType);
                            }
                        }
                    }

                    ContentTypeSheet sheet = GetContentSheetForType(item.contentType);
                    sheet.Add(sheetItem);

                    #endregion //End Extract Fields

                    #region Extract Relations

                    //It would be nice if Percussion differentated "no slot items" from "did not retrieve any 
                    //slot items because of request options."
                    if (item.Slots != null)
                    {
                        foreach (PSItemSlots slot in item.Slots)
                        {

                            SlotItemExtractionType extractType = _config.ExtractSlotForType(item.contentType, slot.Name);

                            //Skip Slot if not extracting
                            if (extractType == SlotItemExtractionType.NoExtract)
                                continue;

                            //Loop through relations
                            foreach (PSRelatedItem slotitem in slot.PSRelatedItem)
                            {
                                if (slotitem.PSAaRelationship != null)
                                {
                                    string templateName = _templateNameManager[new PercussionGuid(slotitem.PSAaRelationship.Template.id)];

                                    if (!string.IsNullOrEmpty(templateName))
                                    {

                                        RelationshipSheetItem relItem = new RelationshipSheetItem(
                                            sheetItem,
                                            PercussionGuid.GetID(slotitem.PSAaRelationship.dependentId),
                                            slot.Name,
                                            templateName,
                                            (extractType == SlotItemExtractionType.ExtractIfExists)
                                            );

                                        _relationSheet.Add(relItem);
                                        
                                        sheetItem.AddSlotItem(relItem);

                                    }
                                    else
                                    {
                                        Console.WriteLine("Cannot get template name for " + slotitem.PSAaRelationship.Template.id);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("AA Info is null");
                                }
                            }
                        }
                    }

                    #endregion

                }
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

        private static string GetCommunity(CMSController controller, PSField field)
        {
            long commID = field.GetFirstValueAsLong();

            if (commID == -1)
            {
                Console.WriteLine("Error Occurred Getting community ID from Field");
                return null;
            }

            return controller.Community.Where(pair => pair.Value.Guid == commID).Select(pair => pair.Key).FirstOrDefault();
        }

        private static string GetCommunityForSheet(CMSController controller, PSField field)
        {
            string community = GetCommunity(controller, field);
            string sheetCommunity = "site";

            if (community != null)
            {
                Console.WriteLine("Community: " + community);
                if (community.Equals("CTBGeneralConfiguration", StringComparison.InvariantCultureIgnoreCase))
                {
                    sheetCommunity = "ctbadmin";
                }
                else if (community.EndsWith("_Configuration", StringComparison.InvariantCultureIgnoreCase))
                {
                    sheetCommunity = "siteadmin";
                }
            }

            return sheetCommunity;
        }

        private static void PrepareSheetsForSaving()
        {

            #region Step 1. Hookup References for RelationshipSheetItem Dependents now that all ContentTypeSheetItems have been extracted

            // To help process the items and relationships we should store, we need to hook up the references between the relationships and the
            // dependents.  (The references from relationships to owners is setup when the relationship item is instantiated)
            //
            foreach (RelationshipSheetItem relItem in _relationSheet)
            {
                foreach (ContentTypeSheet csheet in _contentSheets.Values)
                {
                    relItem.Dependent = csheet.GetSheetItemByID(relItem.DependentID);
                    if (relItem.Dependent != null)
                        break; //Found it, move to next rel item
                }
            }

            #endregion

            #region Step 2. Remove any Relationship Sheet Items that do not have an extracted dependent and are not marked as OnlyStoreIfExtractedDependent
            //Since extracted items can have AA relationships to existing items, we would NOT have a reference to the dependent ContentSheetItem because it would not be
            //extracted.  This however is slightly dangerous and requires care in use.  Therefore, a flag, OnlyStoreIfExtractedDependent, is set on the Relationship Sheet
            //item when the is is created based on business rules. (Which currently always sets it as false...)
            //
            //This step removes any of those RelationshipSheetItems where there is no extracted dependent and it is not marked to store if there is not an extracted dependent.
            //(and therefore would not show up on the relationships tab)

            //Loop through a copy of the relationship sheet (i.e. List<RelationshipSheetItem>) so that we can actually remove elements from the sheet
            foreach (RelationshipSheetItem relItem in _relationSheet.ToArray())
            {
                if (relItem.Dependent == null && relItem.OnlyStoreIfExtractedDependent)
                {
                    //Remove the relationship from the owner since the relationship would not be saved.
                    relItem.Owner.RemoveSlotItem(relItem);

                    //Remove the relationship from the sheet
                    _relationSheet.Remove(relItem);                    
                }
            }
            #endregion

            #region Step 3. Process all content items that we do not OnlyStoreIfDependent           
            
            //This will allow us to mark all items to be saved as processed so that we remove those items that are NOT being saved.
            foreach (ContentTypeSheet sheet in _contentSheets.Values)
            {
                foreach (ContentTypeSheetItem item in sheet.Where(i=>!i.OnlyStoreIfDependent))
                {
                    ProcessItemForStoring(item);
                }
            }
            #endregion

            #region Step 4. Remove all content items that have not been processed.

            //These items would be those that are marked as OnlyStoreIfDependent and are NOT a dependent in any relationship
            foreach (ContentTypeSheet sheet in _contentSheets.Values)
            {
                //Note, we use ToArray since we will be modifying the contents of the sheet.
                foreach (ContentTypeSheetItem item in sheet.Where(i => !i.IsProcessed).ToArray())
                {
                    //Clean up child relationships since we are not being stored.                    
                    foreach (RelationshipSheetItem relItem in item.SlotItems)
                    {
                        _relationSheet.Remove(relItem);
                    }

                    //Remove this item from the sheet.
                    sheet.Remove(item);
                }
            }
            
            #endregion
        }

        private static void ProcessItemForStoring(ContentTypeSheetItem item)
        {
            //If the item has been processed before, there is no need to process again.
            //this could happen if it is a dependent in multiple slot relationships
            if (item.IsProcessed == true)
                return;

            //Mark this item as is processed
            item.IsProcessed = true;

            //Loop through all slot items where the dependents are not null.  These items should be saved, and therefore be marked as processed.
            //Technically a micro optimization could be to skip those slot items where the Dependent is NOT marked as OnlyStoreIfDependent since 
            //we process those items elsewhere.  It would only save time in very large scale extractions... and then other issues would probably occur
            foreach (RelationshipSheetItem relItem in item.SlotItems.Where(ri => ri.Dependent != null))
            {
                ProcessItemForStoring(relItem.Dependent);
            }
        }

        private static void OutputSheets()
        {            

            FileInfo outputFile = new FileInfo(_outputFolder.FullName + "\\" + "output.xlsx");
            
            if (outputFile.Exists)
            {
                outputFile.Delete();
                outputFile = new FileInfo(_outputFolder.FullName + "\\" + "output.xlsx");
            }

            //1. Create Workbook
            using (var package = new ExcelPackage(outputFile))
            {
                #region Folders
                ExcelWorksheet folderSheet = package.Workbook.Worksheets.Add("1 Folders");
                folderSheet.Cells[1, 1].Value = "folder";
                folderSheet.Cells[1, 2].Value = "show_in_nav";
                #endregion

                #region Output Content Sheets

                var sheets = _contentSheets.Values.OrderBy(sheet => sheet.SheetName);
                foreach (ContentTypeSheet sheet in sheets)
                {
                    Console.WriteLine("Processing Sheet: " + sheet.SheetName);

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheet.SheetName);
                    
                    //Fixed Header Items
                    worksheet.Cells[1, (int)ContentSheetCols.MigID].Value = "mig_id";
                    worksheet.Cells[1, (int)ContentSheetCols.Community].Value = "community";
                    worksheet.Cells[1, (int)ContentSheetCols.ContentType].Value = "contenttype";
                    worksheet.Cells[1, (int)ContentSheetCols.Folder].Value = "folder";

                    int rowOffset = 2; //1 for spreadsheets being 1-index based and 2 for Header row 

                    //Create dictionary to map fields to columns.
                    Dictionary<string, int> fieldColMap = new Dictionary<string, int>();

                    for (int sheetItemIndex = 0; sheetItemIndex < sheet.Count; sheetItemIndex++)
                    {
                        ContentTypeSheetItem sheetItem = sheet[sheetItemIndex];

                        //Fixed Column Items
                        worksheet.Cells[sheetItemIndex + rowOffset, (int)ContentSheetCols.MigID].Value = sheetItem.MigID;
                        worksheet.Cells[sheetItemIndex + rowOffset, (int)ContentSheetCols.Community].Value = sheetItem.Community;
                        worksheet.Cells[sheetItemIndex + rowOffset, (int)ContentSheetCols.ContentType].Value = sheetItem.ContentType;
                        worksheet.Cells[sheetItemIndex + rowOffset, (int)ContentSheetCols.Folder].Value = sheetItem.Folder;

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
                                fieldColMap.Add(fieldName, fieldColMap.Keys.Count + (((int)ContentSheetCols.Folder) + 1));
                                
                                //Add Column Header
                                worksheet.Cells[1, fieldColMap[fieldName]].Value = fieldName;
                            }

                            //Set the cell for this sheet item's field.
                            worksheet.Cells[sheetItemIndex + rowOffset, fieldColMap[fieldName]].Value = sheetItem[fieldName];

                        }
                    }


                }
                #endregion

                #region Share To
                ExcelWorksheet shareToSheet = package.Workbook.Worksheets.Add("3 Share To");
                shareToSheet.Cells[1, 1].Value = "mig_id";
                shareToSheet.Cells[1, 2].Value = "folder";

                #endregion

                #region Output Relationship Sheet

                //Output the items
                //TODO: Make Sheet Names Configurable
                ExcelWorksheet relWorksheet = package.Workbook.Worksheets.Add("4 Relationships");
                relWorksheet.Cells[1, (int)RelationSheetCols.OwnerID].Value = "ownerid";
                relWorksheet.Cells[1, (int)RelationSheetCols.DependentID].Value = "dependentid";
                relWorksheet.Cells[1, (int)RelationSheetCols.Slot].Value = "slot";
                relWorksheet.Cells[1, (int)RelationSheetCols.Template].Value = "template";

                for (int i = 0; i < _relationSheet.Count; i++)
                {
                    relWorksheet.Cells[i + 2, (int)RelationSheetCols.OwnerID].Value = _relationSheet[i].OwnerMigID;
                    relWorksheet.Cells[i + 2, (int)RelationSheetCols.DependentID].Value = _relationSheet[i].DependentMigID;
                    relWorksheet.Cells[i + 2, (int)RelationSheetCols.Slot].Value = _relationSheet[i].Slot;
                    relWorksheet.Cells[i + 2, (int)RelationSheetCols.Template].Value = _relationSheet[i].Template;
                }

                #endregion

                #region Translations
                package.Workbook.Worksheets.Add("5 Translations");
                shareToSheet.Cells[1, 1].Value = "english_id";
                shareToSheet.Cells[1, 2].Value = "spanish_id";

                #endregion

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
