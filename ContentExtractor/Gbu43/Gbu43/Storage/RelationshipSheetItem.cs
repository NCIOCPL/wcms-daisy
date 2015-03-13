using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gbu43.Storage
{
    public class RelationshipSheetItem
    {
        private ContentTypeSheetItem _owner;

        public ContentTypeSheetItem Owner { get { return _owner; } }
        public ContentTypeSheetItem Dependent { get; set; }
        public string OwnerMigID { get { return _owner.MigID; } }
        public string DependentMigID { get {
            if (Dependent == null)
                return DependentID.ToString(); //If this Dependent was not extracted, then use the percussion id

            return Dependent.MigID; 
        } }        
        public long DependentID { get; internal set; }
        public string Slot { get; internal set; }
        public string Template { get; internal set; }
        

        //Gets or sets whether or not this relationship's dependent id has been mapped to a 
        //mig id.
        public bool IsProcessed { get; set; }
        public bool OnlyStoreIfExtractedDependent  { get; internal set; }
        

        public RelationshipSheetItem(
            ContentTypeSheetItem owner, 
            long dependentID, 
            string slot, 
            string template, 
            bool onlyStoreIfExtractedDependent
            )
        {
            _owner = owner;
            DependentID = dependentID;
            Slot = slot;
            Template = template;
            IsProcessed = false;
            OnlyStoreIfExtractedDependent = onlyStoreIfExtractedDependent;
        }
    }
}
