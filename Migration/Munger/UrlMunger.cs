using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using AngleSharp.Dom;

using NCI.CMS.Percussion.Manager.CMS;
using Munger.Configuration;

namespace Munger
{
    public class UrlMunger : IUrlMunger
    {
        private ImageMunger _imageMunger;
        private ILinkMunger _linkMunger;
        private static Logger _logger;

        public UrlMunger(CMSController controller)
        {
            MungerConfiguration configuration = (MungerConfiguration)ConfigurationManager.GetSection("MungerConfig");

            if (_logger == null)
                _logger = new Logger();
            _imageMunger = new ImageMunger(controller, _logger, configuration);
            _linkMunger = new LinkMunger(controller, _logger, configuration);
        }

        public void RewriteUrls(IDocument document, string pageUrl, out string messages)
        {
            string rewritten;

            // TODO: Remove XmlDocument dependency.
            XmlDocument fullDoc;
            try
            {
                fullDoc = CreateXmlDocument(document.Body.InnerHtml);
            }
            catch (Exception ex)
            {
                messages = "ERROR from UrlMunger - NOT MUNGED , " + ex.ToString();
                return;// document.Body.InnerHtml;
            }

            List<string> errors = new List<string>();

            _imageMunger.RewriteImageReferences(fullDoc, pageUrl, errors);

            _linkMunger.RewriteLinkReferences(document, pageUrl, errors);

            rewritten = GetDocumentBody(fullDoc);

            messages = string.Empty;
            if (errors.Count > 0)
            {
                string rollup = string.Empty;
                errors.ForEach(msg => rollup += (msg + " | "));
                messages = rollup;
            }

            //return rewritten;
        }

        public string RewriteSingleUrl(string url, out string messages)
        {
            List<string> errors = new List<string>();

            string rewritten = _linkMunger.RewriteSingleUrl(url, errors);

            messages = string.Empty;
            if (errors.Count > 0)
            {
                string rollup = string.Empty;
                errors.ForEach(msg => rollup += (msg + " | "));
                messages = rollup;
            }

            return rewritten;
        }

        private XmlDocument CreateXmlDocument(string docBody)
        {
            //string xml_string = "<!DOCTYPE html SYSTEM \""
            //    + Environment.CurrentDirectory + "\\DTD\\xhtml1-transitional.dtd"
            //    + "\"> <html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>simple document</title></head> <body>"
            //    + docBody
            //    + "</body></html>";
            string xml_string = "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>simple document</title></head> <body>"
                + docBody
                + "</body></html>";

            XmlReaderSettings settings = new XmlReaderSettings
            {
                ProhibitDtd = false,
                ValidationType = ValidationType.DTD
            };

            StringReader xml_sr = new StringReader(xml_string);

            XmlDocument doc = new XmlDocument();
            using (XmlReader reader = XmlReader.Create(xml_sr, settings))
            {
                doc.Load(reader);
            }

            return doc;
        }

        private string GetDocumentBody(XmlDocument doc)
        {
            // There will always be a body node.
            // Using InnerXml would cause the namespace declaration to appear across
            // all top-level elements.  Taking OuterXml limits it to a single top-level
            // node which can be cleaned up via string manipulation.
            XmlNodeList bodyNodes = doc.GetElementsByTagName("body");

            string bodyElement = bodyNodes[0].OuterXml;
            int start = bodyElement.IndexOf('>');   // End of the opening <body> tag.
            int end = bodyElement.IndexOf("</body>");   // Closing tag.  Obviously.
            return bodyElement.Substring(start + 1, end - start - 1);
        }
    }
}
