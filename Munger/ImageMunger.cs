using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

using NCI.WCM.CMSManager.CMS;

namespace Munger
{
    class ImageMunger
    {
        CMSController CMSController;
        Logger MessageLog;

        // For talking to Percussion.
        // Map of Image URLs to Percussion content IDs.
        // Deliberately static so it won't go away between calls.
        // This object is not thread-safe.
        private static Dictionary<string, PercussionGuid> _imageUrlMap = new Dictionary<string, PercussionGuid>();

        public ImageMunger(CMSController controller, Logger messageLog)
        {
            CMSController = controller;
            MessageLog = messageLog;
        }

        /// <summary>
        /// Rewrites the img tags within an XHTML document fragment to reference nciImage content
        /// items as inline line images.
        /// </summary>
        /// <param name="docBody">The document fragment to be rewritten</param>
        /// <param name="pageUrl">The URL for the page the fragment is stored in.</param>
        /// <param name="messages">Output parameter containing any errors from processing.</param>
        /// <returns>A copy of docBody with rewritten img tags.</returns>
        public void RewriteImageReferences(XmlDocument doc, string pageUrl, List<string> messages)
        {
            List<string> errors = new List<string>();

            try
            {
                XmlNodeList links = doc.GetElementsByTagName("img");

                foreach (XmlNode link in links)
                {
                    try
                    {
                        string img = link.Attributes["src"].Value;
                        if (IsACancerGovImage(img))
                        {
                            RewriteImageTag(link, pageUrl);
                        }
                    }
                    catch (ImageMungingException ex)
                    {
                        string mungeUrl = link.Attributes["src"].Value ?? "null";
                        string type = ex.GetType().Name;
                        MessageLog.OutputLine("ImageMunger", type, pageUrl, mungeUrl, ex.Message);
                        errors.Add(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        string mungeUrl = link.Attributes["src"].Value ?? "null";
                        string type = ex.GetType().Name;
                        MessageLog.OutputLine("ImageMunger", type, pageUrl, mungeUrl, ex.ToString());
                        string message = string.Format("Unknown error: {0}.", ex.ToString());
                        errors.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                MessageLog.OutputLine("ImageMunger", type, pageUrl, "none", ex.ToString());
                string message = string.Format("Unknown error: {0}.", ex.ToString());
                errors.Add(message);
            }

            if (errors.Count > 0)
            {
                messages.Add("ImageMunger.RewriteLinkReferences encountered errors:");
                messages.AddRange(errors);
            }
        }

        /// <summary>
        /// Rewrites an img tag as an inline image.
        /// Moves the referenced image to Percussion if it is not already there.
        /// </summary>
        /// <param name="imageNode">XmlNode containing an img tag.</param>
        private void RewriteImageTag(XmlNode imageNode, string pageUrl)
        {
            XmlAttributeCollection atributes = imageNode.Attributes;

            // Grab the image URL.
            string img = atributes["src"].Value;
            string altText = string.Empty;
            if (imageNode.Attributes["alt"] != null)
                altText = imageNode.Attributes["alt"].Value;

            // Turn the URL into Percussion server-relative and cancer.gov absolute links.
            string relativePath = MakeRelativePath(img, pageUrl);

            // Is the URL already in the map?
            PercussionGuid imageContentID;
            if (_imageUrlMap.ContainsKey(relativePath))
            {
                imageContentID = _imageUrlMap[relativePath];
            }
            else
            {   //  No: Move the Image
                imageContentID = MigrateImage(relativePath, altText);
                _imageUrlMap.Add(relativePath, imageContentID);
            }
            
            // Rewrite tag as an inline image.
            XmlAttribute attrib;

            PercussionGuid snippetTemplate = CMSController.TemplateNameManager["nciBnImage"];


            // Replace the src attribute.
            atributes.Remove(atributes["src"]);

            attrib = imageNode.OwnerDocument.CreateAttribute("src");
            attrib.Value
                = string.Format("/Rhythmyx/assembler/render?sys_authtype=0&sys_variantid={0}&sys_revision=1&sys_contentid={1}&sys_context=0",
                snippetTemplate.ID.ToString(),
                imageContentID.ID.ToString()
                );
            atributes.Append(attrib);

            // Mark as a special item.
            attrib = imageNode.OwnerDocument.CreateAttribute("inlinetype");
            attrib.Value = "rximage";
            atributes.Append(attrib);

            // Inline image slot.
            SlotInfo slot = CMSController.SlotManager["sys_inline_image"];
            attrib = imageNode.OwnerDocument.CreateAttribute("rxinlineslot");
            attrib.Value = slot.CmsGuid.ID.ToString();
            atributes.Append(attrib);

            // The image to use.
            attrib = imageNode.OwnerDocument.CreateAttribute("sys_dependentid");
            attrib.Value = imageContentID.ID.ToString();
            atributes.Append(attrib);

            // Don't allow the image to be edited.
            attrib = imageNode.OwnerDocument.CreateAttribute("contenteditable");
            attrib.Value = "false";
            atributes.Append(attrib);

            // Set up the snippet template.
            attrib = imageNode.OwnerDocument.CreateAttribute("sys_dependentvariantid");
            attrib.Value = snippetTemplate.ID.ToString();
            atributes.Append(attrib);
        }

        private PercussionGuid MigrateImage(string imagePath, string altText)
        {
            ImageInfo imgInfo = ImageInfo.DownloadImage("http://www.cancer.gov", imagePath);
            NciImage nciImage = new NciImage(imgInfo, altText);

            long rawID =
                CMSController.CreateItem(nciImage.ContentType, nciImage.FieldSet, null, imgInfo.Path, null);

            return new PercussionGuid(rawID);
        }

        private string MakeRelativePath(string path, string pageUrl)
        {
            // Path will always be lower-case.  This is deliberate and important.
            string imgpath = path.ToLowerInvariant();
            string percussionPath = string.Empty;

            if (imgpath.StartsWith("/"))
            {
                percussionPath = imgpath;
            }
            else if (imgpath.StartsWith("http://cancer.gov/")
                || imgpath.StartsWith("http://www.cancer.gov/"))
            {
                int endServerIndex = imgpath.IndexOf(".gov/") + 4;
                percussionPath = imgpath.Substring(endServerIndex);
            }
            else if (imgpath.StartsWith("../"))
            {
                // TODO: Handle URL with ../
                string message = string.Format("ImageMunger doesn't know how to translate image URLs starting with ../. URL: {0}", imgpath);
                throw new ImagePathException(message);
            }
            else
            {   // Huh?
                string message = string.Format("ImageMunger doesn't know how to translate the URL: {0}", imgpath);
                throw new ImagePathException(message);
            }

            return percussionPath;
        }

        private bool IsACancerGovImage(string imageUrl)
        {
            return (imageUrl.StartsWith("/")
                || imageUrl.StartsWith("../")
                || imageUrl.StartsWith("~/")
                || imageUrl.StartsWith("http://cancer.gov", true, CultureInfo.InvariantCulture)
                || imageUrl.StartsWith("http://www.cancer.gov", true, CultureInfo.InvariantCulture));
        }
    }
}