using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{
    class LinkCmsDetails
    {
        public PercussionGuid PercussionGuid { get; private set; }
        public string ContentType { get; private set; }

        public LinkCmsDetails(PercussionGuid guid, string contentType)
        {
            PercussionGuid = guid;
            ContentType = contentType;
        }
    }
}
