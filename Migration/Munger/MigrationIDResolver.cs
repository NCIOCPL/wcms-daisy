using System.Collections.Generic;
using System.Configuration;

using NCI.CMS.Percussion.Manager.CMS;
using NCI.CMS.Percussion.Manager.PercussionWebSvc;

using Munger.Configuration;

namespace Munger
{
    /// <summary>
    /// Attempts to resolve a pretty URL by looking up its migration ID
    /// in the migration database.
    /// </summary>
    class MigrationIDResolver :ILinkResolver
    {
        public LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl)
        {
            LinkCmsDetails details = null;

            if (prettyUrl == "/")
            {
                MungerConfiguration config = (MungerConfiguration)ConfigurationManager.GetSection("MungerConfig");
                prettyUrl = config.RootElementPath.Value;
                if (string.IsNullOrEmpty(prettyUrl))
                    prettyUrl = "/";
            }
            else if (prettyUrl.EndsWith("/"))
            {
                prettyUrl = prettyUrl.Substring(0, prettyUrl.Length - 1);
            }

            // Look up the URL's viewID and content type.
            string migrationID = DataAccess.GetMigrationID(prettyUrl);

            if (!string.IsNullOrEmpty(migrationID))
            {
                Dictionary<string, string> fieldCriteria = new Dictionary<string, string>();
                fieldCriteria.Add("mig_id", migrationID);
                PercussionGuid[] results = controller.SearchForContentItems(null, fieldCriteria);
                if (results.Length == 1)
                {
                    PSItem[] itemList = controller.LoadContentItems(results);
                    details = new LinkCmsDetails(results[0], itemList[0].contentType);
                }
                else if (results.Length > 1)
                {
                    string message = string.Format("LinkMunger: CMS has {0} items containing mig_id '{1}' for pretty url '{2}'.",
                         results.Length, migrationID, prettyUrl);
                    throw new LinkResolutionException(message);
                }
                else
                {
                    string message = string.Format("LinkMunger: CMS does not contain mig_id '{0}' for pretty url '{1}'.",
                         migrationID, prettyUrl);
                    throw new LinkResolutionException(message);
                }
            }

            return details; 
        }
    }
}
