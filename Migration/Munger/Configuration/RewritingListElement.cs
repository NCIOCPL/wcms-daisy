using System.Configuration;

namespace Munger.Configuration
{

    /// <summary>
    /// Provides a list of old/new path value pairs.
    /// </summary>
    public class RewritingListElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RewritingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RewritingElement)element).OldPath;
        }

        public RewritingElement this[int index]
        {
            get
            {
                return (RewritingElement)BaseGet(index);
            }
        }
    }
}
