using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{
    interface ILinkResolver
    {
        LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl);
    }
}
