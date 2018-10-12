using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;

using NCI.CMS.Percussion.Manager.CMS;
using Munger;
using MigrationEngine.Descriptors;
using AngleSharp.Dom.Events;

namespace MigrationEngine.Utilities
{
    static public class FieldHtmlRectifier
    {
        // List of fields containing HTML content.
        static string[] fieldToBeRectified = { "bodyfield", "contact_text", "contact", "additional_information" };

        public static Dictionary<string, string> RewriteHtml(ContentDescriptionBase contentItem, IMigrationLog logger, IUrlMunger munger)
        {
            //String UniqueIdentifier = contentItem.UniqueIdentifier;
            Dictionary<string, string> fields = contentItem.Fields;

            Dictionary<string, string> outgoing = new Dictionary<string, string>();

            ParseErrorCollection errors = new ParseErrorCollection();

            // Create a parser for the current item's locale.
            HtmlParser parser = CreateParser(contentItem.Locale, errors);

            // We're working with HTML snippets, but the parser expects Documents.  To avoid parsing errors for the
            // missing skeleton, we provide one.
            const string htmlEnvelope = "<!DOCTYPE html><html><body>{0}</body></html>";

            foreach (KeyValuePair<string, string> field in fields)
            {
                // Is this one of the fields which is supposed to contain HTML?
                if (fieldToBeRectified.Contains(field.Key))
                {
                    // Clear any errors from previous fields.
                    errors.Clear();

                    // Load the field into a DOM.  At this point, all HTML entities have been converted to their
                    // Unicode equivalents, and the markup is well-formed.
                    IDocument document = parser.Parse(String.Format(htmlEnvelope, field.Value));

                    // Record any parse errors.
                    if(errors.Count > 0)
                    {
                        string message = errors.ToString();
                        logger.LogTaskItemWarning(contentItem.UniqueIdentifier, message, null);
                    }

                    //Munge
                    if (document.Body.InnerHtml.Trim().Length > 0)
                    {
                        string mungeMessage = string.Empty;
                        munger.RewriteUrls(document, null, out mungeMessage);
                        if (String.IsNullOrEmpty(mungeMessage) == false)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("Link Munger warnings: \n");
                            sb.Append(mungeMessage);
                            logger.LogTaskItemWarning(contentItem.UniqueIdentifier, sb.ToString(), null);
                        }
                    }

                    string HtmlOut = document.Body.InnerHtml;

                    //Build output list 
                    outgoing.Add(field.Key, HtmlOut);

                }
                else
                {
                    //Copy unaltered field to output list.
                    outgoing.Add(field.Key, field.Value);
                }

            }

            return outgoing;  
        }

        /// <summary>
        /// Helper method to encapsulate set up of an HtmlParser instance.
        /// </summary>
        /// <param name="locale">The locale to use when parsing a content item (e.g. en-us).</param>
        /// <param name="errorCollection">Reference to a ParseErrorCollection which will receive any errors
        /// which occur while processing the HTML snippet.</param>
        /// <returns>An HtmlParser object.</returns>
        private static HtmlParser CreateParser(string locale, ParseErrorCollection errorCollection)
        {
            IConfiguration config = AngleSharp.Configuration.Default.SetCulture(locale);
            HtmlParser parser = new HtmlParser(config);

            // Capture parsing errors.
            parser.Context.ParseError += (obj, ev) => {
                HtmlErrorEvent ex = ev as HtmlErrorEvent;
                errorCollection.Add(String.Format("Error: {0} Offset: {1}.", ex.Message, ex.Position.Position));
            };

            return parser;
        }
    }
}
