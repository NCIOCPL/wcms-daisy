using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{

    /// <summary>
    /// Interface shared by all classes which attempt to look up a Pretty URL
    /// by differing methods and translate it into a LinkCmsDetails object
    /// (content ID and type).
    /// </summary>
    interface ILinkResolver
    {
        LinkCmsDetails ResolveLink(ICMSController controller, string prettyUrl);
    }
}
