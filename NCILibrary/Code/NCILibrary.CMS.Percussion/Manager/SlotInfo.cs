using System.Collections.Generic;

namespace NCI.CMS.Percussion.Manager.CMS
{
    public class SlotInfo
    {
        public PercussionGuid CmsGuid { get; private set; }
        public string Name { get; private set; }

        public List<ContentTypeToTemplateInfo> AllowedContentTemplatePairs = new List<ContentTypeToTemplateInfo>();

        public SlotInfo(string name, PercussionGuid id)
        {
            Name = name;
            CmsGuid = id;
        }
    }
}