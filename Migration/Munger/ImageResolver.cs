using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.CMS.Percussion.Manager.CMS;

using FileManipulation;

namespace Munger
{
    class ImageResolver : ILinkResolver
    {
        private string _hostname;

        public ImageResolver(string hostname)
        {
            _hostname = hostname;
        }

        public LinkCmsDetails ResolveLink(ICMSController controller, string prettyUrl)
        {
            LinkCmsDetails details = null;

            if (NciImage.KnownExtensions.Any(extension => prettyUrl.EndsWith(extension, StringComparison.OrdinalIgnoreCase)))
            {
                ImageInfo imageInfo = ImageInfo.DownloadImage(_hostname, prettyUrl);
                NciImage nciImage = new NciImage(imageInfo, "");

                long rawID =
                    controller.CreateItem(NciImage.ContentType, nciImage.FieldSet, null, imageInfo.Path, null);

                details = new LinkCmsDetails(new PercussionGuid(rawID), NciImage.ContentType);
            }

            return details;
        }
    }
}
