using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using NCI.CMS.Percussion.Manager.CMS;

using FileManipulation;

namespace Munger
{
    class FileResolver : ILinkResolver
    {
        private string _hostname;

        public FileResolver(string hostname)
        {
            _hostname = hostname;
        }

        public LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl)
        {
            LinkCmsDetails details = null;

            string decodedUrl = HttpUtility.UrlDecode(prettyUrl).Trim();

            if (NciFileInfo.KnownExtensions.Any(extension => decodedUrl.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase)))
            {
                NciFileInfo fileInfo = NciFileInfo.DownloadImage(_hostname, decodedUrl);
                NciFile nciFile = new NciFile(fileInfo);

                long rawID =
                    controller.CreateItem(NciFile.ContentType, nciFile.FieldSet, null, fileInfo.Path, null);

                details = new LinkCmsDetails(new PercussionGuid(rawID), NciFile.ContentType);
            }

            return details;
        }
    }
}
