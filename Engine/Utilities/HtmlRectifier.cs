using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Web;
using System.Web.UI;

using NCI.CMS.Percussion.Manager.CMS;
using Munger;

namespace MigrationEngine.Utilities
{
    static class FieldHtmlRectifier
    {

        static string[] fieldToBeRectified = { "bodyfield", "contact_text", "contact", "additional_information" };

        public static Dictionary<string, string> ConvertToXHtml(Guid migrationID, Dictionary<string, string> fields, IMigrationLog logger, CMSController controller)
        {
            Dictionary<string, string> outgoing = new Dictionary<string, string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionOutputAsXml = true;
            UrlMunger munger = new UrlMunger(controller);

            foreach (KeyValuePair<string, string> field in fields)
            {
                if (fieldToBeRectified.Contains(field.Key))
                {
                    //For Testing 
                    //string HtmlOut = HtmlEntity.DeEntitize("<p>Text</a></table>");
                    
                    string HtmlOut = HtmlEntity.DeEntitize(field.Value);

                    //HTML to XML
                    doc.LoadHtml(HtmlEntity.DeEntitize(HtmlOut));
                    if (doc.ParseErrors.Count<HtmlParseError>() > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("HTML to XML errors(corrected): \n");
                        foreach (HtmlParseError err in doc.ParseErrors)
                        {
                            sb.AppendFormat("Line({0}): {1}\n", err.Line.ToString(), err.Reason);
                        }
                        logger.LogTaskItemWarning(migrationID, sb.ToString(), null);
                    }
                    HtmlOut = doc.DocumentNode.InnerHtml;

                    //Munge
                    if (HtmlOut.Trim().Length > 0)
                    {
                        string mungeMessage = string.Empty;
                        HtmlOut = munger.RewriteUrls(HtmlOut, null, out mungeMessage);
                        if (mungeMessage != "")
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("Link Munger warnings: \n");
                            sb.Append(mungeMessage);
                            logger.LogTaskItemWarning(migrationID, sb.ToString(), null);
                        }
                    }
                    
                    //Build output list 
                    outgoing.Add(field.Key, HtmlOut);

                }
                else
                {
                    //Build output list 
                    outgoing.Add(field.Key, field.Value);
                }

            }

            return outgoing;  
        }

    }
}
