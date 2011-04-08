using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using NCI.WCM.CMSManager.CMS;

namespace Munger
{
    class FileResolver : ILinkResolver
    {
        public LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl)
        {
            LinkCmsDetails details = null;

            string decodedUrl = HttpUtility.UrlDecode(prettyUrl).Trim();

            if (NciFileInfo.KnownExtensions.Any(extension => decodedUrl.EndsWith(extension)))
            {
                NciFileInfo fileInfo = NciFileInfo.DownloadImage("http://www.cancer.gov", decodedUrl);
                NciFile nciFile = new NciFile(fileInfo);

                long rawID =
                    controller.CreateItem(nciFile.ContentType, nciFile.FieldSet, null, fileInfo.Path, null);

                details = new LinkCmsDetails(new PercussionGuid(rawID), nciFile.ContentType);
            }

            return details;
        }
    }
}
