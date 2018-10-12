
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

using NCI.CMS.Percussion.Manager.CMS;

using Munger.Configuration;
using AngleSharp.Dom;

namespace Munger
{
    class LinkMunger : MungerBase, ILinkMunger
    {
        // Map of link URLs to Percussion content IDs.
        // Deliberately static so it won't go away between calls.
        // This object is not thread-safe.
        private static Dictionary<string, LinkCmsDetails> _linkUrlMap = new Dictionary<string, LinkCmsDetails>();

        // Hash table of Clinical Trial pretty URLs.
        // Deliberately static so it won't go away between calls.
        // Loaded via constructor.
        // This object is not thread-safe.
        //private static HashSet<string> _clinicalTrialUrlSet = null;

        // Map of content content types to allowed templates for the inline link slot.
        private List<KeyValuePair<string, PercussionGuid>> _slotContentTypeToTemplateIDMap
            = new List<KeyValuePair<string, PercussionGuid>>();

        // Map of URL substitutions
        LinkSubstituter _linkSubstituter;

        // Map of programmatic link substitutions
        private static Dictionary<string, string> _programmaticLinks;

        private PercussionGuid _inlineLinkSlotID;

        public LinkMunger(ICMSController controller, ILogger messageLog, IMungerConfiguration config)
            : base(controller, messageLog, config)
        {
            _linkSubstituter = new LinkSubstituter(config);

            //SlotInfo inlineLinkSlot = CMSController.SlotManager["sys_inline_link"];
            //_inlineLinkSlotID = inlineLinkSlot.CmsGuid;
            //foreach (ContentTypeToTemplateInfo info in inlineLinkSlot.AllowedContentTemplatePairs)
            //{
            //    _slotContentTypeToTemplateIDMap.Add(new KeyValuePair<string, PercussionGuid>(info.ContentTypeName, info.TemplateID));
            //}

            //if (_programmaticLinks == null)
            //{
            //    _programmaticLinks = new Dictionary<string, string>();
            //    MungerConfiguration config = (MungerConfiguration)ConfigurationManager.GetSection("MungerConfig");
            //    foreach (RewritingElement item in config.ProgrammaticLinkList)
            //    {
            //        _programmaticLinks.Add(item.OldPath.ToLowerInvariant(), item.NewPath);
            //    }
            //}
        }

        /// <summary>
        /// Rewrites the a tags within an XHTML document fragment to reference content
        /// via inline links to the items' content IDs.
        /// </summary>
        /// <param name="docBody">The document fragment to be rewritten</param>
        /// <param name="pageUrl">The URL for the page the fragment is stored in.</param>
        /// <param name="messages">Output parameter containing any errors from processing.</param>
        /// <returns>A copy of docBody with rewritten a tags.</returns>
        public void RewriteLinkReferences(IDocument document, string pageUrl, List<string> messages)
        {
            List<string> errors = new List<string>();

            //try
            //{
            //    XmlNodeList links = document.GetElementsByTagName("a");

            //    foreach (XmlNode link in links)
            //    {
            //        try
            //        {
            //            XmlAttribute hrefAttrib = link.Attributes["href"];
            //            XmlAttribute nameAttrib = link.Attributes["name"];

            //            if (hrefAttrib == null && nameAttrib != null)
            //            {   // Don't process <a name="foo">.
            //                continue;
            //            }
            //            else if (hrefAttrib == null && nameAttrib == null)
            //            {
            //                string message =
            //                    string.Format("LinkMunger doesn't know how to translate links without an href. Link: {0}",
            //                    link.OuterXml);
            //                throw new NoLinkSpecifiedException(message);
            //            }

            //            string linkTarget = hrefAttrib.Value;

            //            if (LinkIsSiteInternal(linkTarget))
            //            {
            //                RewriteAnchorTag(link, pageUrl);
            //            }
            //        }
            //        // Need to catch this separately since it explicitly has no href to retrieve.
            //        catch (NoLinkSpecifiedException ex)
            //        {
            //            string type = ex.GetType().Name;
            //            MessageLog.OutputLine("LinkMunger", type, pageUrl, "null", ex.Message);
            //            errors.Add(ex.Message);
            //        }
            //        catch (LinkMungingException ex)
            //        {
            //            string mungeUrl = link.Attributes["href"].Value ?? "null";
            //            string type = ex.GetType().Name;
            //            MessageLog.OutputLine("LinkMunger", type, pageUrl, mungeUrl, ex.Message);
            //            errors.Add(ex.Message);
            //        }
            //        catch (Exception ex)
            //        {
            //            string mungeUrl = link.Attributes["href"].Value ?? "null";
            //            string type = ex.GetType().Name;
            //            MessageLog.OutputLine("LinkMunger", type, pageUrl, mungeUrl, ex.ToString());
            //            string message = string.Format("Unknown error: {0}.", ex.ToString());
            //            errors.Add(message);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string type = ex.GetType().Name;
            //    MessageLog.OutputLine("LinkMunger", type, pageUrl, "none", ex.ToString());
            //    string message = string.Format("Unknown error: {0}.", ex.ToString());
            //    errors.Add(message);
            //}

            if (errors.Count > 0)
            {
                messages.Add("LinkMunger.RewriteLinkReferences encountered errors:");
                messages.AddRange(errors);
            }
        }

        public string RewriteSingleUrl(string link, List<string> errors)
        {
            string rewrittenUrl;

            // Turn the URL into a server-relative link.
            string relativePath = CleanupPath(link, string.Empty);

            relativePath = ReplaceWithSubstituteLink(relativePath);

            // Clean up or substitution may have revealed an external link in a logging url.
            if (LinkIsSiteInternal(relativePath))
            {
                // Once we know it's definitely an internal link, now it's OK to lowercase it.
                relativePath = relativePath.ToLower();

                ValidatePath(relativePath, link);

                // Is the URL a type we rewrite?
                if (LinkIsRewritable(relativePath))
                {   // Yes

                    if (LinkIsProgrammatic(relativePath))
                    {
                        rewrittenUrl = RewriteProgrammaticLink(relativePath);
                    }
                    else
                        rewrittenUrl = relativePath;
                }
                else
                {// Replace link with the server-relative version.
                    rewrittenUrl = relativePath;
                }
            }
            else
            {
                // We just cleaned up an external link
                rewrittenUrl = relativePath;
            }

            return rewrittenUrl;
        }

        /// <summary>
        /// Rewrites an img tag as an inline image.
        /// Moves the referenced image to Percussion if it is not already there.
        /// </summary>
        /// <param name="imageNode">XmlNode containing an img tag.</param>
        private void RewriteAnchorTag(XmlNode linkNode, string pageUrl)
        {
            XmlAttributeCollection atributes = linkNode.Attributes;

            // Grab the image URL.
            string link = atributes["href"].Value;

            // Turn the URL into a lower-case, server-relative link.
            string relativePath = CleanupPath(link, pageUrl);

            relativePath = ReplaceWithSubstituteLink(relativePath);

            // Clean up or substitution may have revealed an external link in a logging url.
            if (LinkIsSiteInternal(relativePath))
            {
                ValidatePath(relativePath, linkNode.OuterXml);

                // Is the URL a type we rewrite?
                if (LinkIsRewritable(relativePath))
                {   // Yes


                    if (LinkIsProgrammatic(relativePath))
                    {
                        atributes["href"].Value = RewriteProgrammaticLink(relativePath);
                    }
                    else
                    {
                        // At this point we know the link is to a resource which
                        // should be in the CMS.  First we look for it in the in-memory
                        // map of urls to CMS items.  If it isn't there, we go to
                        // look it up.

                        // Is the URL already in the map?
                        LinkCmsDetails linkDetails;
                        if (_linkUrlMap.ContainsKey(relativePath))
                        {
                            linkDetails = _linkUrlMap[relativePath];
                        }
                        else
                        {   //  No: Look up the details.
                            linkDetails = GetLinkDetails(relativePath); // Look up the content ID. MigrateImage(relativePath, altText);
                            _linkUrlMap.Add(relativePath, linkDetails);
                        }

                        ReplaceLinkNodeAttributes(linkNode, linkDetails);
                    }
                }
                else
                {// Replace link with the server-relative version.
                    atributes["href"].Value = relativePath;
                }
            }
            else
            {
                // We just cleaned up an external link
                atributes["href"].Value = relativePath;
            }
        }

        private void ReplaceLinkNodeAttributes(XmlNode linkNode, LinkCmsDetails linkDetails)
        {
            XmlAttributeCollection atributes = linkNode.Attributes;

            PercussionGuid snippetTemplate = GetTemplateID(linkDetails.ContentType);
            PercussionGuid linkTarget = linkDetails.PercussionGuid;


            // Rewrite tag as an inline link.
            XmlAttribute attrib;

            // Replace the href attribute.
            atributes.Remove(atributes["href"]);

            attrib = linkNode.OwnerDocument.CreateAttribute("href");
            attrib.Value
                = string.Format("/Rhythmyx/assembler/render?sys_authtype=0&sys_variantid={0}&sys_revision=1&sys_contentid={1}&sys_context=0",
                snippetTemplate.ID.ToString(),
                linkTarget.ID.ToString());
            atributes.Append(attrib);

            // Mark as a special item.
            attrib = linkNode.OwnerDocument.CreateAttribute("inlinetype");
            attrib.Value = "rxhyperlink";
            atributes.Append(attrib);

            // The link target
            attrib = linkNode.OwnerDocument.CreateAttribute("sys_dependentid");
            attrib.Value = linkTarget.ID.ToString();
            atributes.Append(attrib);

            // Inline link slot.
            attrib = linkNode.OwnerDocument.CreateAttribute("rxinlineslot");
            attrib.Value = _inlineLinkSlotID.ID.ToString();
            atributes.Append(attrib);

            // Set up the snippet template.
            attrib = linkNode.OwnerDocument.CreateAttribute("sys_dependentvariantid");
            attrib.Value = snippetTemplate.ID.ToString();
            atributes.Append(attrib);
        }

        private PercussionGuid GetTemplateID(string contentType)
        {
            int i;
            for (i = 0; i < _slotContentTypeToTemplateIDMap.Count; i++)
            {
                if (_slotContentTypeToTemplateIDMap[i].Key == contentType)
                    break;
            }
            if (i >= _slotContentTypeToTemplateIDMap.Count)
            {
                string message = string.Format("LinkMunger: sys_inline_link has no template for {0}.", contentType);
                throw new LinkMungingException(message);
            }

            return _slotContentTypeToTemplateIDMap[i].Value;
        }

        /// <summary>
        /// Attempts to look up the content ID and type associated with a given
        /// pretty URL.
        /// </summary>
        /// <param name="prettyUrl">The pretty URL.</param>
        /// <returns></returns>
        private LinkCmsDetails GetLinkDetails(string prettyUrl)
        {
            LinkCmsDetails details = null;

            ILinkResolver[] linkResolvers = { new MigrationIDResolver(),
                                                new FileResolver(CanonicalHostName),
                                                new ImageResolver(CanonicalHostName)};

            // Attempt to resolve the pretty URL through a series of resolvers.
            foreach (ILinkResolver resolver in linkResolvers)
            {
                details = resolver.ResolveLink(CMSController, prettyUrl);
                if (details != null)
                    break;
            }

            if (details == null)
            {
                string message;
                message = string.Format("LinkMunger: Unable to resolve pretty url '{0}'.", prettyUrl);
                throw new LinkResolutionException(message);
            }


            return details;
        }

        /// <summary>
        /// Rewrites programmatic links with values from the application config file.
        /// Old and New value pairs appear in the MungerConfig section in the list of
        /// Programmatic values.
        /// </summary>
        /// <param name="linkPath">The link path.</param>
        /// <returns></returns>
        private string RewriteProgrammaticLink(string linkPath)
        {
            string newlink;
            string path;
            string arguments;

            // Separate the old link path and its arguments.
            int index = linkPath.IndexOf('?');

            if (index != -1)
            {
                path = linkPath.Substring(0, index).ToLower();
                arguments = linkPath.Substring(index + 1);
            }
            else
            {
                path = linkPath.ToLower();
                arguments = string.Empty;
            }

            // Allows /dictionary/ to be matched with the same test as /dictionary
            if (path.EndsWith("/") && path.Length > 1)
                path = path.Substring(0, path.Length - 1);

            // Attempt to substitute a link path from MungerConfig/Programmatic
            // in the application config file.
            if (_programmaticLinks.ContainsKey(path.ToLower()))
            {
                if (!string.IsNullOrEmpty(arguments))
                    newlink = string.Format("{0}?{1}", _programmaticLinks[path], arguments);
                else
                    newlink = _programmaticLinks[path];
            }
            else
            {
                string message = string.Format("Don't know how to rewrite programmatic link: {0}.", linkPath);
                throw new ProgrammaticLinkException(message);
            }

            return newlink;
        }

        private string ReplaceWithSubstituteLink(string pageUrl)
        {
            return _linkSubstituter.MakeSubstitution(pageUrl);
        }

        private string CleanupPath(string path, string pageUrl)
        {
            string linkpath = path.Trim();
            string relativePath = linkpath;

            // Convert to site relative
            if (linkpath.StartsWith("/"))
            {
                relativePath = linkpath;
            }
            else
            {
                relativePath = RemoveServerHostName(linkpath);
            }

            // Clean up old-style clicklog handler.
            if (relativePath.StartsWith("/clicklog.ashx", StringComparison.InvariantCultureIgnoreCase)
                || relativePath.StartsWith("/common/clickpassthrough.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                const string urlStart = "redirecturl=";

                // Find URL in the redirecturl argument.
                int index = relativePath.IndexOf(urlStart, StringComparison.InvariantCultureIgnoreCase);
                if (index < 0)
                {
                    string message = string.Format("clicklog link has no redirecturl argument. URL: {0}", linkpath);
                    throw new LinkPathException(message);
                }

                relativePath = relativePath.Substring(index + urlStart.Length);

                // Find end of URL
                index = relativePath.IndexOf('&');
                if (index > -1)
                {
                    relativePath = relativePath.Substring(0, index);
                }

                // In case the wrapped URL was to cancer.gov
                relativePath = RemoveServerHostName(relativePath);
            }
            else if (path.StartsWith("#"))
            {// No rewrite for bookmarks.
                relativePath = path;
            }

            // Trim trailing /
            if (relativePath.EndsWith("/") && relativePath.Length > 1)
            {
                relativePath = relativePath.Substring(0, relativePath.Length - 1);
            }

            return relativePath;
        }

        private string RemoveServerHostName(string linkpath)
        {
            string cleanUrl = linkpath;
            Uri linkUri = new Uri(linkpath, UriKind.RelativeOrAbsolute);

            if(linkUri.IsAbsoluteUri && HostAliases.Contains(linkUri.Host))
            {
                cleanUrl = linkUri.PathAndQuery;

                // Did we take out everything?
                if (string.IsNullOrEmpty(cleanUrl))
                {
                    cleanUrl = "/";
                }
            }
            return cleanUrl;
        }

        private void ValidatePath(string linkpath, string linkText)
        {
            string decodedPath = HttpUtility.UrlDecode(linkpath).Trim();

            if (linkpath.StartsWith("../"))
            {
                // TODO: Handle URL with ../
                string message = string.Format("LinkMunger doesn't know how to translate URLs starting with ../. URL: {0}", linkpath);
                throw new LinkPathException(message);
            }
            else if (string.IsNullOrEmpty(linkpath))
            {
                string message = string.Format("LinkMunger doesn't know how to translate blank URLs.  Tag: {0}.", linkText);
                throw new LinkPathException(message);
            }
            else if (LinkIsStaticPage(linkpath) || LinkIsStaticPage(decodedPath))
            {
                string message = string.Format("LinkMunger doesn't translate static pages. Tag: {0}.", linkText);
                throw new StupidContentLocationException(message);
            }
        }

        private bool LinkIsRewritable(string linkUrl)
        {
            string testUrl = linkUrl.ToLower().Trim();

            // External applications with cancer.gov URLs. (Content switch or virtual app directory)
            bool externalApplication;
            string[] externalApps = { "/cam/", "/bcrisktool/", "/dctd/", "/prevention/", "/i131/fallout/" };
            string[] externalAppsExact = { "/cam", "/bcrisktool", "/edrn",  "/livehelp", "/publications", "/colorectalcancerrisk"};
            externalApplication = Array.Exists(externalApps, app => testUrl.StartsWith(app))
                || Array.Exists(externalAppsExact, app => testUrl == app);

            // Don't rewrite if it has a bookmark, unless it's a programmatic link.
            bool hasBookmark = linkUrl.Contains("#") && !LinkIsProgrammatic(linkUrl);

            // Some links are already correct.
            string[] ignoreList = { "/diccionario", "/dictionary", "/drugdictionary", "/search/clinicaltrials" };
            bool safelyIgnored = Array.Exists(ignoreList, item => item == testUrl || (item + "/") == testUrl);

            return !(externalApplication || hasBookmark || safelyIgnored );
        }

        private bool LinkIsProgrammatic(string linkUrl)
        {
            return linkUrl.Contains('?')
               || linkUrl.Contains(".aspx");
        }

        private bool LinkIsSiteInternal(string linkUrl)
        {
            string testUrl = linkUrl.Trim().ToLower();
            Uri uri = new Uri(testUrl, UriKind.RelativeOrAbsolute);

            // Conditions underwhich the link is not internal.
            bool externalSite = false;
            bool unmanagedProtocol = false;

            // Both tests only apply if 
            if (uri.IsAbsoluteUri)
            {
                // External web sites.
                if (!HostAliases.Contains(uri.Host))
                {
                    externalSite = true;
                }

                // Check that it's neither HTTP nor HTTPS.
                unmanagedProtocol =
                    !uri.Scheme.StartsWith("http", StringComparison.InvariantCultureIgnoreCase);
            }

            // If it's neither an external site, nor an unmanaged protocol,
            // then it's an internal link.
            return !(externalSite || unmanagedProtocol);
        }

        private bool LinkIsStaticPage(string linkUrl)
        {
            string[] staticPageExtensions = { ".htm", ".html", ".txt", ".shtml" };

            // Remove doc fragment if necessary.
            int index = linkUrl.LastIndexOf('#');
            if (index != -1)
            {
                linkUrl = linkUrl.Substring(0, index);
            }

            return Array.Exists(staticPageExtensions, extension => linkUrl.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}