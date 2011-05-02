using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munger
{
    class LinkLegacyDetails
    {

        public string ViewID { get; private set; }
        public string ContentType { get; private set; }

        public LinkLegacyDetails(string viewid, string contentType)
        {
            ViewID = viewid;
            ContentType = contentType;
        }
    }
}