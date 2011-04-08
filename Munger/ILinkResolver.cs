using NCI.WCM.CMSManager.CMS;

namespace Munger
{
    interface ILinkResolver
    {
        LinkCmsDetails ResolveLink(CMSController controller, string prettyUrl);
    }
}
