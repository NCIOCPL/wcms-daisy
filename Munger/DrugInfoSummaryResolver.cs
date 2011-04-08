using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.PercussionWebSvc;
using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{
    class DrugInfoSummaryResolver : ILinkResolver
    {

        public LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl)
        {
            const string drugInfoFolder = "/cancertopics/druginfo/";


            LinkCmsDetails details = null;

            if (prettyUrl.StartsWith(drugInfoFolder))
            {
                string prettyUrlName = prettyUrl.Substring(drugInfoFolder.Length);

                Dictionary<string, string> fieldCriteria = new Dictionary<string, string>();
                fieldCriteria.Add("pretty_url_name", prettyUrlName);

                PercussionGuid[] searchResults = controller.SearchForContentItems(null, drugInfoFolder, fieldCriteria);
                if (searchResults.Length == 1)
                {   // We've found our target, but we need to be in the config community in case it's a pdqDrugInfoSummary.
                    // cgvDrugInfoSummary can be loaded from config or cgov, but pdqDrugInfoSummary only loads from config.
                    using (CMSController altController = new CMSController("CancerGov_Configuration"))
                    {
                        PSItem[] item = altController.LoadContentItems(searchResults);
                        details = new LinkCmsDetails(searchResults[0], item[0].contentType);
                    }
                }
                else if (searchResults.Length > 1)
                {
                    string message
                        = string.Format("DrugInfoSummary: Searching for {0}, found {1} results, expected 1.", prettyUrl, searchResults.Length);
                    throw new CmsSearchException(message);
                }
            }

            return details;
        }
    }
}
