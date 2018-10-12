using System;
using System.Collections.Generic;
using NCI.CMS.Percussion.Manager.PercussionWebSvc;

namespace NCI.CMS.Percussion.Manager.CMS
{
    public interface ICMSController
    {
        Dictionary<string, PercussionGuid> Community { get; }
        ContentTypeManager ContentTypeManager { get; }
        FolderManager FolderManager { get; }
        string SiteRootPath { get; set; }
        SlotManager SlotManager { get; }
        TemplateNameManager TemplateNameManager { get; }

        void AddFolderChildren(string folderPath, PercussionGuid[] idCollection);
        void CheckInItems(PercussionGuid[] itemIDList);
        PSItemStatus[] CheckOutForEditing(PercussionGuid[] guidList);
        PSAaRelationship[] CreateActiveAssemblyRelationships(long parentItemID, long[] childItemIDList, string slotName, string snippetTemplateName);
        PSAaRelationship[] CreateActiveAssemblyRelationships(long parentItemID, long[] childItemIDList, string slotName, string snippetTemplateName, int index);
        PSAaRelationship[] CreateActiveAssemblyRelationships(PercussionGuid parentItemID, PercussionGuid[] childItemIDList, string slotName, string snippetTemplateName);
        List<long> CreateContentItemList(List<ContentItemForCreating> contentItems);
        List<long> CreateContentItemList(List<ContentItemForCreating> contentItems, Action<string> errorHandler);
        long CreateItem(string contentType, Dictionary<string, string> newFieldValues, IEnumerable<ChildFieldSet> childFieldList, string targetFolder, Action<string> invalidFieldnameHandler);
        PSRelationship CreateRelationship(long parentItemID, long childItemID, string relationshipType);
        PSRelationship CreateRelationship(PercussionGuid parentItemID, PercussionGuid childItemID, string relationshipType);
        void DeleteActiveAssemblyRelationships(PSAaRelationship[] relationships, bool alreadyInEditingState);
        void DeleteFolders(PSFolder[] folderInfo);
        void DeleteItem(long itemID);
        void DeleteItemList(PercussionGuid[] itemList);
        void Dispose();
        PSItemSummary[] FindFolderChildren(string path);
        PSAaRelationship[] FindIncomingActiveAssemblyRelationships(PercussionGuid[] IDList);
        PSAaRelationship[] FindIncomingActiveAssemblyRelationships(PercussionGuid[] dependentIDList, string slotName, string templateName);
        bool FolderExists(string path);
        string GetPathInSite(PSItem item);
        object GetWorkflowState(long[] itemIDs, WorkflowStateInfererDelegate inferState);
        PSFolder GuaranteeFolder(string folderPath);
        PSFolder GuaranteeFolder(string folderPath, FolderManager.NavonAction navonAction);
        PSItem[] LoadContentItems(long[] itemIDList);
        PSItem[] LoadContentItems(PercussionGuid[] itemIDList);
        PSItem[] LoadLinkingContentItems(long itemID);
        void MoveContentItemFolder(string sourcePath, string targetPath, long[] idcoll);
        void MoveContentItemFolder(string sourcePath, string targetPath, PercussionGuid[] idcoll);
        void PerformWorkflowTransition(long[] idList, string triggerName);
        void PerformWorkflowTransition(PercussionGuid[] idList, string triggerName);
        void ReleaseFromEditing(PSItemStatus[] statusList);
        PercussionGuid[] SaveContentItems(PSItem[] itemList);
        PercussionGuid[] SearchForContentItems(string contentType, Dictionary<string, string> fieldCriteria);
        PercussionGuid[] SearchForContentItems(string contentType, string path, bool searchSubFolders, Dictionary<string, string> fieldCriteria);
        PercussionGuid[] SearchForContentItems(string contentType, string siteBasePath, string path, bool searchSubFolders, Dictionary<string, string> fieldCriteria);
        PercussionGuid[] SearchForItemsInSlot(PercussionGuid owner, string slotname);
        List<long> UpdateContentItemList(List<ContentItemForUpdating> contentItems);
        List<long> UpdateContentItemList(List<ContentItemForUpdating> contentItems, Action<string> invalidFieldnameHandler);
        bool VerifyItemsExist(long[] itemIDList);
        bool VerifyItemsExist(PercussionGuid[] itemIDList);
        bool VerifySingleItemExists(long itemID);
        bool VerifySingleItemExists(PercussionGuid itemID);
    }
}