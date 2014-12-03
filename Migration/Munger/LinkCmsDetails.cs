using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{
    /// <summary>
    /// Business object to represent the content id and content type
    /// of a single business object.
    /// </summary>
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
