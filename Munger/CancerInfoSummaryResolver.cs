using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using NCI.WCM.CMSManager.PercussionWebSvc;
using NCI.WCM.CMSManager.CMS;

namespace Munger
{
    class CancerInfoSummaryResolver : ILinkResolver
    {

        public LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl)
        {
            const string cisEnglishFolder = "/cancertopics/pdq/";
            const string cisSpanishFolder = "/espanol/pdq/";
            const string cisContentType = "pdqCancerInfoSummary";
            const string cislinkContentType = "pdqCancerInfoSummaryLink";

            // Match cancertopics or espanol, followed by "pdq" and then the
            // summary type, cancer type, and finally, the audience.
            Regex cisUrlPattern = new Regex(@"/(.+)/pdq/([a-zA-Z0-9\-]+?)/([a-zA-Z0-9\-]+?)/([a-zA-Z0-9\-]+?)/?$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

            // Match cancertopics or espanol, followed by "pdq" and then the
            // summary type, cancer type, but no audience.
            Regex cisLinkUrlPattern = new Regex(@"/(.+)/pdq/([a-zA-Z0-9\-]+?)/([a-zA-Z0-9\-]+?)/?$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

            string[] cisFolderSet = { cisEnglishFolder, cisSpanishFolder };


            LinkCmsDetails details = null;

            if (Array.Exists(cisFolderSet, folder => prettyUrl.StartsWith(folder)))
            {
                // Check whether the pretty URL matches the audience-specific or
                // a cancer name portion a summary path.
                Match audiencePathMatch = cisUrlPattern.Match(prettyUrl);
                Match cancernamePathMatch = cisLinkUrlPattern.Match(prettyUrl);

                if (audiencePathMatch.Success || cancernamePathMatch.Success)
                {
                    PercussionGuid[] foundIDs;

                    string contentType;

                    // Search for a content item depending on which one matched.
                    if (audiencePathMatch.Success)
                        contentType = cisContentType;
                    else
                        contentType = cislinkContentType;

                    foundIDs = controller.SearchForContentItems(contentType, prettyUrl, null);

                    if (foundIDs != null && foundIDs.Length == 1)
                    {
                        details = new LinkCmsDetails(foundIDs[0], contentType);
                    }
                    else if (foundIDs != null && foundIDs.Length > 1)
                    {
                        string message
                            = string.Format("CancerInfoSummary: Searching for {0}, found {1} results, expected 1.", prettyUrl, foundIDs.Length);
                        throw new CmsSearchException(message);
                    }
                }
            }

            return details;
        }
    }
}
