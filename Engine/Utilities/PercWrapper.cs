using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NCI.WCM.CMSManager.CMS;
using NCI.WCM.CMSManager.PercussionWebSvc;

namespace Migrate_O_Matic
{
    public static class Utilities
    {
        public static long CreateItemWrapper(CMSController controller, string contentType, Dictionary<string, string> fields, string folder, out string warningMessage)
        {
            warningMessage = "";
            List<string> invalidFields = new List<string>();

            long id = controller.CreateItem(contentType, fields, null, folder, fieldName => { invalidFields.Add(fieldName); });

            if (invalidFields.Count > 0)
            {
                warningMessage = "Fields not found: ";
                foreach (string s in invalidFields)
                {
                    if (warningMessage.Substring(warningMessage.Length - 1, 1) != ",")
                        warningMessage += ("," + s);
                    else
                        warningMessage += s;
                }
            }
            return id;
        }

        public static long CreateItemWrapper(CMSController controller, string contentType, Dictionary<string, string> fields,IEnumerable<ChildFieldSet> childFields, string folder, out string warningMessage)
        {
            warningMessage = "";
            List<string> invalidFields = new List<string>();

            long id = controller.CreateItem(contentType, fields, childFields, folder, fieldName => { invalidFields.Add(fieldName); });

            if (invalidFields.Count > 0)
            {
                warningMessage = "Fields not found: ";
                foreach (string s in invalidFields)
                {
                    if (warningMessage.Substring(warningMessage.Length - 1, 1) != ",")
                        warningMessage += ("," + s);
                    else
                        warningMessage += s;
                }
            }
            return id;
        }

        public static long UpdateItemWrapper(CMSController controller, long id, FieldSet fields, out string warningMessage)
        {
            
            
            warningMessage = "";
            List<string> invalidFields = new List<string>();
            //ContentItemForUpdating[] item = { new ContentItemForUpdating(id, (FieldSet)fields) };
            List<ContentItemForUpdating> item = new List<ContentItemForUpdating>();
            item.Add(new ContentItemForUpdating(id, fields));
            
            List<long> returned = controller.UpdateContentItemList(item, fieldName => { invalidFields.Add(fieldName); });

            if (invalidFields.Count > 0)
            {
                warningMessage = "Fields not found: ";
                foreach (string s in invalidFields)
                {
                    if (warningMessage.Substring(warningMessage.Length - 1, 1) != ",")
                        warningMessage += ("," + s);
                    else
                        warningMessage += s;
                }
            }
            return id;
        }


        public static Dictionary<string, string> BuildFieldsDictionary(DataRow dr, string[] exclude)
        {
            bool addItem;
            Dictionary<string, string> localD = new Dictionary<string, string>();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (dr[dc.ColumnName] != System.DBNull.Value) 
                {
                    addItem = true;
                    foreach (string s in exclude)
                    {
                        if (dc.ColumnName.ToLower() == s.ToLower())
                            addItem = false;
                    }

                    if (addItem)
                    {
                            localD.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                    }
                }
            }
            return localD;
        }

        public static FieldSet BuildFieldSet(DataRow dr, string[] exclude)
        {
            bool addItem;
            FieldSet localFS = new FieldSet();
            foreach (DataColumn dc in dr.Table.Columns)
            {
                if (dr[dc.ColumnName] != System.DBNull.Value)
                {
                    addItem = true;
                    foreach (string s in exclude)
                    {
                        if (dc.ColumnName.ToLower() == s.ToLower())
                            addItem = false;
                    }

                    if (addItem)
                    {
                        localFS.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                    }
                }
            }
            return localFS;
        }



        public static long GetPercussionIDfromNCIViewId(CMSController controller, string contentType, Guid NCIViewId)
        {
            Dictionary<string, string> localD = new Dictionary<string, string>();
            localD.Add("viewid", NCIViewId.ToString());

            PercussionGuid[] ids = controller.SearchForContentItems(contentType, localD);

            if (ids.Count<PercussionGuid>() == 1)
                return ids[0].ID;
            else if (ids.Count<PercussionGuid>() == 0)
                return -1;
            else
            {
                return -2;
            }
        }


        public static long GetPercussionIDfromObjectId(CMSController controller, string contentType, Guid objectid)
        {
            Dictionary<string, string> localD = new Dictionary<string, string>();
            localD.Add("objectid", objectid.ToString());

            PercussionGuid[] ids = controller.SearchForContentItems(contentType, localD);

            if (ids.Count<PercussionGuid>() == 1)
                return ids[0].ID;
            else if (ids.Count<PercussionGuid>() == 0)
                return -1;
            else
            {
                return -2;
            }
        }


        public static long GetPercussionIDfromLocationAndContentType(CMSController controller, string contentType, string folder, out string message)
        {
            try
            {
                message = "";
                PercussionGuid[] ids = controller.SearchForContentItems(contentType, folder, new Dictionary<string, string> { });

                if (ids.Count<PercussionGuid>() == 1)
                    return ids[0].ID;
                else if (ids.Count<PercussionGuid>() == 0)
                    return -1;
                else
                {
                    return -2;
                }
            }
            catch (NCI.WCM.CMSManager.CMSSoapException ex)
            {
                message = ex.InnerException.Message;
                return -5;
            }
        }

        public static long GetContentId(CMSController controller, string contentType, string id, out string warningMessage)
        {
            if (contentType.ToString().ToLower() == "pdqCancerInfoSummaryLink".ToLower())
                return GetPercussionIDfor_pdqCancerInfoSummaryLink(controller, id, out warningMessage);
            else if (contentType.ToString().ToLower() == "pdqDrugInfoSummary".ToLower())
                return GetPercussionIDfromCdrId(controller, contentType, id, out warningMessage);
            else
                return GetPercussionIDfromNCIViewId(controller, contentType, new Guid(id), out warningMessage);
        }



        public static long GetPercussionIDfromCdrId(CMSController controller, string contentType, string cdrId, out string message)
        {
            try
            {
                Dictionary<string, string> localD = new Dictionary<string, string>();
                localD.Add("cdrid", cdrId);

                PercussionGuid[] ids = controller.SearchForContentItems(contentType, localD);

                message = "";
                if (ids.Count<PercussionGuid>() == 1)
                    return ids[0].ID;
                else if (ids.Count<PercussionGuid>() == 0)
                    return -1;
                else
                    return -2;

            }
            catch (NCI.WCM.CMSManager.CMSSoapException ex)
            {
                message = ex.InnerException.Message;
                return -5;
            }
        }
        
        public static long GetPercussionIDfor_pdqCancerInfoSummaryLink(CMSController controller, string cdrId, out string message)
        {
            long returnValue = -1;

            try
            {

                long id = GetPercussionIDfromCdrId(controller, "pdqCancerInfoSummary", cdrId, out message);

                message = "";
                if (id > 0)
                {
                    PercussionGuid pg = new PercussionGuid(id);
                    PercussionGuid[] ids = { pg };

                    PSAaRelationship[] results = controller.FindIncomingActiveAssemblyRelationships(ids, "pdqCancerInformationSummaryHealthProf", "pdqSnCancerInformationSummaryItemLink");
                    if (results.Length > 0)
                    {
                        if (results.Length > 1)
                            message = "More than one pdqCancerInformationSummaryHealthProf/pdqSnCancerInformationSummaryItemLink relatiohship found";
                        returnValue = results[0].ownerId;
                    }
                    else
                    {
                        results = controller.FindIncomingActiveAssemblyRelationships(ids, "pdqCancerInformationSummaryPatient", "pdqSnCancerInformationSummaryItemLink");
                        if (results.Length > 0)
                        {
                            if (results.Length > 1)
                                message = "More than one pdqCancerInformationSummaryPatient/pdqSnCancerInformationSummaryItemLink relatiohship found";
                            returnValue = results[0].ownerId;
                        }
                        else
                            returnValue = -3;
                    }
                }
                else 
                    returnValue = id;

                if (returnValue > 0)
                {
                    PercussionGuid pg1 = new PercussionGuid(returnValue);
                    return pg1.ID;
                }
                else
                    return returnValue;
            }
            catch (NCI.WCM.CMSManager.CMSSoapException ex)
            {
                message = ex.InnerException.Message;
                return -5;
            }
        }

        public static long GetNavon(CMSController controller, string folder, out string message)
        {
            try
            {
                message = "";
                PercussionGuid[] ids = controller.SearchForContentItems("rffNavon", folder, new Dictionary<string, string> { });

                if (ids.Count<PercussionGuid>() == 1)
                    return ids[0].ID;
                else if (ids.Count<PercussionGuid>() == 0)
                    return -1;
                else
                {
                    return -2;
                }
            }
            catch (NCI.WCM.CMSManager.CMSSoapException ex)
            {
                message = ex.InnerException.Message;
                return -5;
            }
        }

        public static long GetNavTree(CMSController controller, out string message)
        {
            try
            {
                message = "";
                PercussionGuid[] ids = controller.SearchForContentItems("rffNavTree", "/", new Dictionary<string, string> { });

                if (ids.Count<PercussionGuid>() == 1)
                    return ids[0].ID;
                else if (ids.Count<PercussionGuid>() == 0)
                    return -1;
                else
                {
                    return -2;
                }
            }
            catch (NCI.WCM.CMSManager.CMSSoapException ex)
            {
                message = ex.InnerException.Message;
                return -5;
            }
        }


        public static ChildFieldSet GetBestBetSynonyms(string category_name)
        {
            //List<ChildFieldSet> childrenList = new List<ChildFieldSet>(); 
            DataSet synonyms = DataAccess.GetBestBetSynonymsDataSet(category_name);

            ChildFieldSet child = new ChildFieldSet("synonyms");

            foreach (DataRow dr in synonyms.Tables["Synonyms"].Rows)
            {
                FieldSet synFields = new FieldSet();
                foreach (DataColumn dc in synonyms.Tables["Synonyms"].Columns)
                {
                    if(dc.ColumnName != "catname")
                        synFields.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                child.Fields.Add(synFields);
            }

            return child;
        }

        public static ChildFieldSet GetBestBetNegatedSynonyms(string category_name)
        {
            //List<ChildFieldSet> childrenList = new List<ChildFieldSet>();
            DataSet synonyms = DataAccess.GetBestBetNegatedSynonymsDataSet(category_name);

            ChildFieldSet child = new ChildFieldSet("negated_synonyms");

            foreach (DataRow dr in synonyms.Tables["NegatedSynonyms"].Rows)
            {
                FieldSet synFields = new FieldSet();
                foreach (DataColumn dc in synonyms.Tables["NegatedSynonyms"].Columns)
                {
                    if (dc.ColumnName != "catname")
                        synFields.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                child.Fields.Add(synFields);
            }
            return child;
        }

        public static long GetPercussionIDfromNCIViewId(CMSController controller, string contentType, Guid NCIViewId, out string warning)
        {
            Dictionary<string, string> localD = new Dictionary<string, string>();
            localD.Add("viewid", NCIViewId.ToString());
            warning = "";
            PercussionGuid[] ids = controller.SearchForContentItems(contentType, localD);

            if (ids.Count<PercussionGuid>() == 1)
                return ids[0].ID;
            else if (ids.Count<PercussionGuid>() == 0)
                return -1;
            else
            {
                foreach (PercussionGuid pg in ids)
                {
                    if (warning.Length == 0)
                    {
                        warning = "More than one viewId found - using first: " + pg.ID.ToString();
                    }
                    else
                        warning += ", " + pg.ID.ToString();
                }

                return ids[0].ID;
            }
        }

        public static long GetPercussionIDfromObjectId(CMSController controller, string contentType, Guid NCIViewId, out string warning)
        {
            Dictionary<string, string> localD = new Dictionary<string, string>();
            localD.Add("objectid", NCIViewId.ToString());
            warning = "";
            PercussionGuid[] ids = controller.SearchForContentItems(contentType, localD);

            if (ids.Count<PercussionGuid>() == 1)
                return ids[0].ID;
            else if (ids.Count<PercussionGuid>() == 0)
                return -1;
            else
            {
                foreach (PercussionGuid pg in ids)
                {
                    if (warning.Length == 0)
                        warning = "More than one objectId found - using first: " + pg.ID.ToString();
                    else
                        warning += ", " + pg.ID.ToString();
                }

                return ids[0].ID;
            }
        }

        public static long GetPercussionIDfromUploadedFilename(CMSController controller, string UploadedFilename, out string warning)
        {
            Dictionary<string, string> localD = new Dictionary<string, string>();
            localD.Add("item_file_attachment_filename", UploadedFilename);
            warning = "";
            PercussionGuid[] ids = controller.SearchForContentItems("nciFile", localD);

            if (ids.Count<PercussionGuid>() == 1)
                return ids[0].ID;
            else if (ids.Count<PercussionGuid>() == 0)
                return -1;
            else
            {
                foreach (PercussionGuid pg in ids)
                {
                    if (warning.Length == 0)
                        warning = "More than one objectId found - using first: " + pg.ID.ToString();
                    else
                        warning += ", " + pg.ID.ToString();
                }

                return ids[0].ID;
            }
        }

        public static long GetPercussionID(CMSController controller, string contentType, string title, string pretty_url_name)
        {
            Dictionary<string, string> localD = new Dictionary<string, string>();
            localD.Add("long_title", title);
            localD.Add("pretty_url_name", pretty_url_name);

            PercussionGuid[] ids = controller.SearchForContentItems(contentType, localD);

            if (ids.Count<PercussionGuid>() == 1)
                return ids[0].ID;
            else if (ids.Count<PercussionGuid>() == 0)
                return -1;
            else
                return -2;
        }


        public static string getFileExtension(string filePath)
        {
            int index = filePath.LastIndexOf('.');
            if (index != -1)
                return filePath.Substring(index);
            else
                return filePath;
        }

        public static string getFilename(string filePath)
        {
            int index = filePath.LastIndexOf('/');
            if (index != -1)
                return filePath.Substring(index + 1);
            else
                return filePath;
        }

        public static string getJustFilename(string filePath)
        {
            int index = filePath.LastIndexOf('/');
            int endIndex = filePath.LastIndexOf('.');
            if (index != -1)
            {
                if (endIndex != -1)
                    return filePath.Substring(index + 1, filePath.Length - (index + 1) - (filePath.Length - endIndex));
                else
                    return filePath.Substring(index + 1);
            }
            else
            {
                if (endIndex != -1)
                    return filePath.Substring(0, filePath.Length - (index + 1) - (filePath.Length - endIndex));
                else
                    return filePath;
            }
        }


    }
}
