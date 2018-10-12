using NCI.CMS.Percussion.Manager.CMS;

using Munger.Configuration;

namespace Munger
{
    /// <summary>
    /// Helper class to assist in making unit tests possible without changing everything to public.
    /// </summary>
    public static class MungerFactory
    {
        static public ILinkMunger CreateLinkMunger(ICMSController controller, ILogger messageLog, IMungerConfiguration configuration)
        {
            return new LinkMunger(controller, messageLog, configuration);
        }
    }
}
