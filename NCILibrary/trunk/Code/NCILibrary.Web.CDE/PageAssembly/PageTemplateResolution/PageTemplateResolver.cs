using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;

using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public static class PageTemplateResolver
    {
        private const string PageTemplateConfigurationKey = "PageTemplateConfiguration";
        private const string PageTemplateConfigurationFilename = "PageTemplateConfiguration.xml";
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets an instance of the HttpServerUtility for the current request.
        /// </summary>
        private static HttpServerUtility Server {
            get { return HttpContext.Current.Server; }
        }

        private static string PageTemplateConfigurationPath
        {
            get
            {
                return Server.MapPath(ContentDeliveryEngineConfig.PathInformation.PageTemplateConfigurationPath.Path);
            }
        }
        
        private static PageTemplateConfiguration PageTemplateConfiguration
        {
            get 
            {
                if (HttpContext.Current.Cache[PageTemplateConfigurationKey] == null)
                {
                    //Make sure no one else tries to access this until we are done.
                    lock (_lock)
                    {
                        //Double check that another thread did not create the item before us.
                        if (HttpContext.Current.Cache[PageTemplateConfigurationKey] == null)
                        {
                            PageTemplateConfiguration config =
                                LoadPageTemplateConfiguration(PageTemplateConfigurationPath);

                            AddPageTemplateConfigurationToCache(config, PageTemplateConfigurationPath);
                        }
                    }
                }
                    
                return (PageTemplateConfiguration)HttpContext.Current.Cache[PageTemplateConfigurationKey];
            }
        }

        private static void AddPageTemplateConfigurationToCache(PageTemplateConfiguration config, string configurationPath)
        {
            HttpContext.Current.Cache.Add(
                PageTemplateConfigurationKey,
                config,
                new CacheDependency(configurationPath),
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(PTCCachedRemovalCallback));

        }

        private static void PTCCachedRemovalCallback(string key, object config, CacheItemRemovedReason reason)
        {
            //If the PageTemplateConfiguration was removed because of the dependency, then
            //reload the PageTemplateConfiguration
            if (
                key == PageTemplateConfigurationKey && 
                reason == CacheItemRemovedReason.DependencyChanged
                )
            {
                PageTemplateConfiguration reloadedConfig =
                    LoadPageTemplateConfiguration(PageTemplateConfigurationPath);

                AddPageTemplateConfigurationToCache(reloadedConfig, PageTemplateConfigurationPath);
            }
        }

        /// <summary>
        /// Retrieves the LayoutTemplateInfo representing the named template.
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public static PageTemplateInfo GetPageTemplateInfo(string templateName, DisplayVersions version)
        {
            PageTemplateInfo rtnInfo = null;

            if (string.IsNullOrEmpty(templateName))
            {
                throw new ArgumentNullException("templateName", "The templateName parameter cannot be null or empty.");
            }


            if (PageTemplateConfiguration != null)
            {
                //Get the page template collection for the templatename we are looking for.
                PageTemplateCollection coll = 
                    PageTemplateConfiguration.PageTemplateCollections.FirstOrDefault(ptc => ptc.TemplateName == templateName);

                //If there is a collection, ask the collection for the template with the version we are looking for.
                if (coll != null)
                {
                    rtnInfo = coll.GetPageTemplateInfo(version);
                }
            }

            return rtnInfo;
        }

        private static PageTemplateConfiguration LoadPageTemplateConfiguration(string configurationPath)
        {
            PageTemplateConfiguration rtnConfig = null;

            XmlSerializer serializer = new XmlSerializer(typeof(PageTemplateConfiguration));

            try
            {
                using (FileStream xmlFile = File.Open(configurationPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {

                    rtnConfig = (PageTemplateConfiguration)serializer.Deserialize(XmlReader.Create(xmlFile));
                }
            }
            catch (Exception ex)
            {
                //TODO: Do something here
            }

            return rtnConfig;
        }
    }
}
