using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.WCM.CMSManager.CMS;

namespace Munger
{
    class ImageResolver : ILinkResolver
    {
        public LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl)
        {
            LinkCmsDetails details = null;

            if (NciImage.KnownExtensions.Any(extension => prettyUrl.EndsWith(extension, StringComparison.OrdinalIgnoreCase)))
            {
                ImageInfo imageInfo = ImageInfo.DownloadImage("http://www.cancer.gov", prettyUrl);
                NciImage nciImage = new NciImage(imageInfo, "");

                long rawID =
                    controller.CreateItem(nciImage.ContentType, nciImage.FieldSet, null, imageInfo.Path, null);

                details = new LinkCmsDetails(new PercussionGuid(rawID), nciImage.ContentType);
            }

            return details;
        }
    }
}
