using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using NCI.CMS.Percussion.Manager;
using NCI.CMS.Percussion.Manager.CMS;
using NCI.CMS.Percussion.Manager.PercussionWebSvc;


namespace MigrationEngine.Utilities
{
    static class PercWrapper
    {
        // Special meaning Guids.  Real PercussionGuids don't have negative values and the
        // Revision is only -1 for new items.
        public static readonly PercussionGuid ContentItemNotFound = new PercussionGuid(-1) { Type = -1, Revision = -1 };
        public static readonly PercussionGuid TooManyContentItemsFound = new PercussionGuid(-2) { Type = -1, Revision = -1 };
        public static readonly PercussionGuid CmsErrorOccured = new PercussionGuid(-5) { Type = -1, Revision = -1 };


        public static long CreateItemWrapper(CMSController controller, string contentType, Dictionary<string, string> fields, string folder, out string warningMessage)
        {
            warningMessage = "";
            List<string> invalidFields = new List<string>();

            long id = controller.CreateItem(contentType, fields, null, folder, fieldName => { invalidFields.Add(fieldName); });

            if (invalidFields.Count > 0)
            {
                warningMessage = "Fields not found: ";
                for (int i = 0; i < invalidFields.Count; i++)
                {
                    if (i > 0)
                        warningMessage += (", ");
                    warningMessage += invalidFields[i];
                }
            }
            return id;
        }

        public static long CreateItemWrapper(CMSController controller, string contentType, Dictionary<string, string> fields, IEnumerable<ChildFieldSet> childFields, string folder, out string warningMessage)
        {
            warningMessage = "";
            List<string> invalidFields = new List<string>();

            long id = controller.CreateItem(contentType, fields, childFields, folder, fieldName => { invalidFields.Add(fieldName); });

            if (invalidFields.Count > 0)
            {
                warningMessage = "Fields not found: ";
                for (int i = 0; i < invalidFields.Count; i++)
                {
                    if (i > 0)
                        warningMessage += (", ");
                    warningMessage += invalidFields[i];
                }
            }
            return id;
        }

        public static PercussionGuid UpdateItemWrapper(CMSController controller, PercussionGuid id, FieldSet fields, out string warningMessage)
        {
            warningMessage = "";
            List<string> invalidFields = new List<string>();
            //ContentItemForUpdating[] item = { new ContentItemForUpdating(id, (FieldSet)fields) };
            List<ContentItemForUpdating> item = new List<ContentItemForUpdating>();
            item.Add(new ContentItemForUpdating(id.ID, fields));

            List<long> returned = controller.UpdateContentItemList(item, fieldName => { invalidFields.Add(fieldName); });

            if (invalidFields.Count > 0)
            {
                warningMessage = "Fields not found: ";
                for (int i = 0; i < invalidFields.Count; i++)
                {
                    if (i > 0)
                        warningMessage += (", ");
                    warningMessage += invalidFields[i];
                }
            }
            return id;
        }

        public static void CreateSingleRelationshipWrapper(CMSController controller, PercussionGuid ownerID, PercussionGuid dependentID, string slotName, string templateName)
        {
            PSAaRelationship[] relations = controller.CreateActiveAssemblyRelationships(ownerID, new PercussionGuid[] { dependentID }, slotName, templateName);
            if (relations.Length != 1)
            {
                throw new RelationshipException(string.Format("Relationship creation failed; expected 1, created {0}", relations.Length));
            }
        }

        public static long GetPercussionIDfromLocationAndContentType(CMSController controller, string contentType, string folder, out string message)
        {
            try
            {
                message = "";
                PercussionGuid[] ids = controller.SearchForContentItems(contentType, folder, true, new Dictionary<string, string> { });

                if (ids.Count<PercussionGuid>() == 1)
                    return ids[0].ID;
                else if (ids.Count<PercussionGuid>() == 0)
                    return -1;
                else
                {
                    return -2;
                }
            }
            catch (CMSSoapException ex)
            {
                message = ex.InnerException.Message;
                return -5;
            }
        }

        public static PercussionGuid GetNavon(CMSController controller, string folder, out string message)
        {
            folder = folder.Trim();

            string contentType = (folder == "/") ? "rffNavTree" : "rffNavon";
            string contentTypeName = (folder == "/") ? "NavTree" : "Navon";

            try
            {
                message = "";
                PercussionGuid[] ids = controller.SearchForContentItems(contentType, folder, false, new Dictionary<string, string> { });

                if (ids.Count<PercussionGuid>() == 1)
                    return ids[0];
                else if (ids.Count<PercussionGuid>() == 0)
                {
                    message = string.Format("Navon not found in {0}.", folder);
                    return ContentItemNotFound;
                }
                else
                {
                    message = string.Format("More than one Navon found in {0}.", folder);
                    return TooManyContentItemsFound;
                }
            }
            catch (CMSSoapException ex)
            {
                message = ex.InnerException.Message;
                return CmsErrorOccured;
            }
        }

        public static PercussionGuid GetNavTree(CMSController controller, out string message)
        {
            return GetNavon(controller, "/", out message);
        }

        public static void CreateTranslationRelationship(CMSController controller, PercussionGuid original, PercussionGuid translation)
        {
            controller.CreateRelationship(original, translation, CMSController.TranslationRelationshipType);
        }

    }
}
