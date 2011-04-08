using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.WCM.CMSManager.CMS;

namespace Munger
{
    class DatabaseResolver :ILinkResolver
    {
        public LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl)
        {
            LinkCmsDetails details = null;

            if (prettyUrl == "/")
            {
                prettyUrl = "/homepage";
            }
            else if (prettyUrl.EndsWith("/"))
            {
                prettyUrl = prettyUrl.Substring(0, prettyUrl.Length - 1);
            }

            // Look up the URL's viewID and content type.
            LinkLegacyDetails cGovDetails = DataAccess.GetUrlDetails(prettyUrl);
            if (cGovDetails != null)
            {
                Dictionary<string, string> fieldCriteria = new Dictionary<string, string>();
                fieldCriteria.Add("viewid", cGovDetails.ViewID);
                PercussionGuid[] results = controller.SearchForContentItems(cGovDetails.ContentType, fieldCriteria);
                if (results != null && results.Length > 0)
                {
                    details = new LinkCmsDetails(results[0], cGovDetails.ContentType);
                }
                else
                {
                    string message = string.Format("LinkMunger: CMS does not contain view '{0}' for pretty url '{1}'.",
                         cGovDetails.ViewID, prettyUrl);
                    throw new LinkResolutionException(message);
                }
            }

            return details; 
        }
    }
}
