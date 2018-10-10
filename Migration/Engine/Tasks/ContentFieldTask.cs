using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using NCI.CMS.Percussion.Manager.CMS;

using MigrationEngine;
using MigrationEngine.Descriptors;
using MigrationEngine.Utilities;
using Munger;

namespace MigrationEngine.Tasks
{
    /// <summary>
    /// Implements common functionality for all Tasks which create or
    /// update content item fields.
    /// </summary>
    public abstract class ContentFieldTask : MigrationTask
    {
        public abstract override void Doit(IMigrationLog logger);


        /// <summary>
        /// Perform required preprocessing on the fields to be save in the CMS.
        /// The base implementation creates pseudo-xhtml, rewrites links to their managed equivlanets,
        /// and forces pretty_url_name values to lowercase.
        /// </summary>
        /// <param name="contentItem">Content item containing the fields to be processed.</param>
        /// <param name="controller">CMS controller object</param>
        /// <param name="logger">An instance of a IMigrationLog.</param>
        /// <returns>The processed field set.</returns>
        protected virtual Dictionary<string, string> PreProcessFields(ContentDescriptionBase contentItem,
            CMSController controller, IMigrationLog logger)
        {
            UrlMunger munger = new UrlMunger(controller);

            Dictionary<string, string> processedFields =
                            FieldHtmlRectifier.ConvertToXHtml(contentItem, logger, munger);

            if (processedFields.ContainsKey(Constants.Fields.PRETTY_URL))
            {
                processedFields[Constants.Fields.PRETTY_URL] =
                    processedFields[Constants.Fields.PRETTY_URL].ToLower();
            }

            return processedFields;
        }
    }
}
