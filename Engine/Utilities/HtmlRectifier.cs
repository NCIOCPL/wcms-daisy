using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Web;
using System.Web.UI;

namespace MigrationEngine.Utilities
{
    static class FieldHtmlRectifier
    {

        static string[] fieldToBeRectified = { "bodyfield", "long_description" };

        public static Dictionary<string, string> Doit(Guid migrationID, Dictionary<string, string> fields, IMigrationLog logger)
        {
            Dictionary<string, string> outgoing = new Dictionary<string, string>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.OptionOutputAsXml = true;

            foreach (KeyValuePair<string, string> field in fields)
            {
                if (fieldToBeRectified.Contains(field.Key))
                {
                    doc.LoadHtml(HtmlEntity.DeEntitize(field.Value));
                    outgoing.Add(field.Key, doc.DocumentNode.InnerHtml);
                    logger.LogTaskItemWarning("Rectify: " + field.Key, null);
                }
                else
                {
                    outgoing.Add(field.Key, field.Value);
                }
            }

            return outgoing;  
        }

        public static string HtmlToXml(string HtmlString)
        {
            return HtmlString;
        }

        public static string Munge(string HtmlString)
        {
            return HtmlString;
        }



    }
}
