using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

using NCI.CMS.Percussion.Manager.CMS;

namespace Munger
{
    class LinkMunger
    {
        CMSController CMSController;
        Logger MessageLog;

        // Map of link URLs to Percussion content IDs.
        // Deliberately static so it won't go away between calls.
        // This object is not thread-safe.
        private static Dictionary<string, LinkCmsDetails> _linkUrlMap = new Dictionary<string, LinkCmsDetails>();

        // Hash table of Clinical Trial pretty URLs.
        // Deliberately static so it won't go away between calls.
        // Loaded via constructor.
        // This object is not thread-safe.
        private static HashSet<string> _clinicalTrialUrlSet = null;

        // Map of content content types to allowed templates for the inline link slot.
        private List<KeyValuePair<string, PercussionGuid>> _slotContentTypeToTemplateIDMap
            = new List<KeyValuePair<string, PercussionGuid>>();

        // Map of URL substitutions
        LinkSubstituter _linkSubstituter = new LinkSubstituter();

        private PercussionGuid _inlineLinkSlotID;

        public LinkMunger(CMSController controller, Logger messageLog)
        {
            CMSController = controller;
            MessageLog = messageLog;

            SlotInfo inlineLinkSlot = CMSController.SlotManager["sys_inline_link"];
            _inlineLinkSlotID = inlineLinkSlot.CmsGuid;
            foreach (ContentTypeToTemplateInfo info in inlineLinkSlot.AllowedContentTemplatePairs)
            {
                _slotContentTypeToTemplateIDMap.Add(new KeyValuePair<string, PercussionGuid>(info.ContentTypeName, info.TemplateID));
            }

            LoadClinicalTrialUrls();
        }

        // This method should only be called from the LinkMunger constructor.
        private void LoadClinicalTrialUrls()
        {
            if (_clinicalTrialUrlSet == null)
            {
                _clinicalTrialUrlSet = new HashSet<string>();
                List<string> idStrings = DataAccess.GetProtocolPrettyUrlIDs();
                idStrings.ForEach(idvalue =>
                {
                    // Primary pretty URLs are returned first and receive priority.
                    string urlString = "/clinicaltrials/" + idvalue;
                    if (!_clinicalTrialUrlSet.Contains(urlString))
                        _clinicalTrialUrlSet.Add(urlString);
                });
            }
        }

        /// <summary>
        /// Rewrites the a tags within an XHTML document fragment to reference content
        /// via inline links to the items' content IDs.
        /// </summary>
        /// <param name="docBody">The document fragment to be rewritten</param>
        /// <param name="pageUrl">The URL for the page the fragment is stored in.</param>
        /// <param name="messages">Output parameter containing any errors from processing.</param>
        /// <returns>A copy of docBody with rewritten a tags.</returns>
        public void RewriteLinkReferences(XmlDocument doc, string pageUrl, List<string> messages)
        {
            List<string> errors = new List<string>();

            try
            {
                XmlNodeList links = doc.GetElementsByTagName("a");

                foreach (XmlNode link in links)
                {
                    try
                    {
                        XmlAttribute hrefAttrib = link.Attributes["href"];
                        XmlAttribute nameAttrib = link.Attributes["name"];

                        if (hrefAttrib == null && nameAttrib != null)
                        {   // Don't process <a name="foo">.
                            continue;
                        }
                        else if (hrefAttrib == null && nameAttrib == null)
                        {
                            string message =
                                string.Format("LinkMunger doesn't know how to translate links without an href. Link: {0}",
                                link.OuterXml);
                            throw new NoLinkSpecifiedException(message);
                        }

                        string linkTarget = hrefAttrib.Value;

                        if (LinkIsCancerGovInternal(linkTarget))
                        {
                            RewriteAnchorTag(link, pageUrl);
                        }
                    }
                    catch (LinkMungingException ex)
                    {
                        string mungeUrl = link.Attributes["href"].Value ?? "null";
                        string type = ex.GetType().Name;
                        MessageLog.OutputLine("LinkMunger", type, pageUrl, mungeUrl, ex.Message);
                        errors.Add(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        string mungeUrl = link.Attributes["href"].Value ?? "null";
                        string type = ex.GetType().Name;
                        MessageLog.OutputLine("LinkMunger", type, pageUrl, mungeUrl, ex.ToString());
                        string message = string.Format("Unknown error: {0}.", ex.ToString());
                        errors.Add(message);
                    }
                }
            }
            catch (Exception ex)
            {
                string type = ex.GetType().Name;
                MessageLog.OutputLine("LinkMunger", type, pageUrl, "none", ex.ToString());
                string message = string.Format("Unknown error: {0}.", ex.ToString());
                errors.Add(message);
            }

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
            if (LinkIsCancerGovInternal(relativePath))
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
            if (LinkIsCancerGovInternal(relativePath))
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

        private LinkCmsDetails GetLinkDetails(string prettyUrl)
        {
            LinkCmsDetails details = null;

            ILinkResolver[] linkResolvers = { new DatabaseResolver(),
                                                new FileResolver(),
                                                new DrugInfoSummaryResolver(),
                                                new CancerInfoSummaryResolver(),
                                                new ImageResolver()};

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

        private string RewriteProgrammaticLink(string linkPath)
        {
            string newlink;
            string path;
            string arguments;

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

            if (path == "/dictionary/db_alpha.aspx" ||
                path == "/templates/db_alpha.aspx" ||
                path == "/dictionary")
            {
                newlink = string.Format("/dictionary?{0}", arguments);
            }
            else if (path == "/diccionario/db_alpha.aspx" ||
                path == "/diccionario")
            {
                newlink = string.Format("/diccionario?{0}", arguments);
            }
            else if (path == "/drugdictionary/drugdictionary.aspx" ||
                path == "/drugdictionary" ||
                path == "/templates/drugdictionary.aspx")
            {
                newlink = string.Format("/drugdictionary?{0}", arguments);
            }
            else if (path == "/search/clinicaltrialslink.aspx")
            {
                newlink = string.Format("/search/clinicaltrialslink?{0}", arguments);
            }
            else if (path == "/common/popups/popdefinition.aspx")
            {
                newlink = linkPath;
            }
            else if (path == "/newscenter/search")
            {
                newlink = linkPath;
            }
            else if (path == "/search/viewclinicaltrials.aspx" ||
                path == "/search/view_clinicaltrials.aspx" ||
                path == "/clinicaltrials/view_clinicaltrials.aspx")
            {
                newlink = string.Format("/clinicaltrials/search/view?{0}", arguments);
            }
            else if (path == "/search/psrv.aspx")
            {
                newlink = string.Format("/clinicaltrials/search/printresults?{0}", arguments);
            }
            
            else if (path == "/search/resultsclinicaltrials.aspx"
                || path == "/search/resultsclinicaltrialsadvanced.aspx"
                || path == "/search/clinical_trials/resultsclinicaltrialsadvanced.aspx"
                || path == "/search/clinical_trials/results_clinicaltrialsadvanced.aspx")
            {
                newlink = string.Format("/clinicaltrials/search/results?{0}", arguments);
            }
            else if (path == "/search/searchclinicaltrialsadvanced.aspx")
            {
                newlink = string.Format("/clinicaltrials/search/?{0}", arguments);
            }
            else if (path == "/search/geneticsservices/")
            {
                newlink = string.Format("/cancertopics/genetics/directory?{0}", arguments);
            }
            else if (path == "/search/results_geneticsservices.aspx")
            {
                newlink = string.Format("/cancertopics/genetics/directory/results?{0}", arguments);
            }
            else if (path == "/search/view_geneticspro.aspx")
            {
                newlink = string.Format("/cancertopics/genetics/directory/view?{0}", arguments);
            }
            else if (path == "/search/results.aspx")
            {
                newlink = string.Format("/search/results?{0}", arguments);
            }
            else if (path == "/search/clinicaltrialslink.aspx")
            {
                newlink = linkPath;
            }
            else if (path == "/cbsubscribe.aspx" && string.IsNullOrEmpty(arguments))
            {
                newlink = "/ncicancerbulletin/subscribe";
            }
            else if (path == "/search/searchcancertopics.aspx")
            {
                newlink = "/cancertopics/litsearch";
                if (!string.IsNullOrEmpty(arguments))
                {
                    switch (arguments)
                    {
                        case "listid=0d014096-c42e-427f-9ef7-edd09b95df3b":
                            newlink = "/cancertopics/litsearch/aids-related";
                            break;
                        case "listid=4963a5be-238c-4d3c-905d-b6b70f6a86fe":
                            newlink = "/cancertopics/litsearch/breast";
                            break;
                        case "listid=0a86d345-b2c4-4eac-b459-28ee9f5a7ee1":
                            newlink = "/cancertopics/litsearch/genetics";
                            break;
                        case "listid=d4a04f37-500f-4e41-8580-38a97825eedc":
                            newlink = "/cancertopics/litsearch/cardiovascular";
                            break;
                        case "listid=434c580a-29c7-44ee-94ce-50be5de67c56":
                            newlink = "/cancertopics/litsearch/endocrine";
                            break;
                        case "listid=8551ce76-59c9-43f1-8f31-7a2e6e880fce":
                            newlink = "/cancertopics/litsearch/gastrointestinal";
                            break;
                        case "listid=acd05787-6535-46a6-acaf-5b368c7d5555":
                            newlink = "/cancertopics/litsearch/gynecologic";
                            break;
                        case "listid=ca7b46be-75a0-4434-a52a-949d46153dc6":
                            newlink = "/cancertopics/litsearch/head-and-neck";
                            break;
                        case "listid=e7134b11-f556-4012-bf5a-1553d54b3c0d":
                            newlink = "/cancertopics/litsearch/hematologic";
                            break;
                        case "listid=758fbfe3-dd2c-40b9-b9fb-3adce34e3ae4":
                            newlink = "/cancertopics/litsearch/male-reproductive";
                            break;
                        case "listid=e8ecbca0-c98f-4dc7-a621-32e2406b4f35":
                            newlink = "/cancertopics/litsearch/metastatic";
                            break;
                        case "listid=1eb188f0-6a9e-4bbd-a573-eb12561afb07":
                            newlink = "/cancertopics/litsearch/neurologic";
                            break;
                        case "listid=ba08634f-1650-4998-8334-7980974cd21e":
                            newlink = "/cancertopics/litsearch/sarcoma";
                            break;
                        case "listid=abf17978-bd75-4b50-962d-a43965c62cf0":
                            newlink = "/cancertopics/litsearch/skin-and-melanoma";
                            break;
                        case "listid=d488e7dc-9f11-4a2e-baca-2b539787a1aa":
                            newlink = "/cancertopics/litsearch/thoracic";
                            break;
                        case "listid=dd7e90a6-77d5-4a95-b110-c7e56a511e22":
                            newlink = "/cancertopics/litsearch/tobacco";
                            break;
                        case "listid=6784d169-d486-4a5d-817d-2e0533a62410":
                            newlink = "/cancertopics/litsearch/urinary-tract";
                            break;
                        default:
                            string message = string.Format("Programmatic Link: Unexpected cancer topics search: {0}", arguments);
                            throw new ProgrammaticLinkException(message);
                            break;
                    }
                }

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
                relativePath = RemoveCancerGovHost(linkpath);
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
                relativePath = RemoveCancerGovHost(relativePath);
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

        private string RemoveCancerGovHost(string linkpath)
        {
            string cleanUrl = linkpath;

            if (linkpath.StartsWith("http://cancer.gov", StringComparison.InvariantCultureIgnoreCase)
                || linkpath.StartsWith("http://www.cancer.gov", StringComparison.InvariantCultureIgnoreCase)
                || linkpath.StartsWith("http://preview.cancer.gov", StringComparison.InvariantCultureIgnoreCase))
            {
                int endServerIndex = linkpath.IndexOf(".gov", StringComparison.InvariantCultureIgnoreCase) + 4;
                cleanUrl = linkpath.Substring(endServerIndex);

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

            bool clinicalTrial = _clinicalTrialUrlSet.Contains(linkUrl);

            return !(externalApplication || hasBookmark || safelyIgnored || clinicalTrial);
        }

        private bool LinkIsProgrammatic(string linkUrl)
        {
            return linkUrl.Contains('?')
               || linkUrl.Contains(".aspx");
        }

        private bool LinkIsCancerGovInternal(string linkUrl)
        {
            string testUrl = linkUrl.Trim().ToLower();

            // External web sites.
            bool externalSite = false;
            if (testUrl.StartsWith("http:")) // Covers http and https.
            {
                Uri uri = new Uri(testUrl);
                if (uri.Host != "www.cancer.gov"
                   && uri.Host != "cancer.gov"
                    && uri.Host != "preview.cancer.gov")
                {
                    externalSite = true;
                }
            }

            // Non-http protocols
            bool unmanagedProtocol;
            string[] protocolList = { "javascript:", "mailto:" };
            unmanagedProtocol = Array.Exists(protocolList, protocol => testUrl.StartsWith(protocol));

            return !(externalSite || unmanagedProtocol);
        }

        private bool LinkIsStaticPage(string linkUrl)
        {
            string[] staticPageExtensions = { ".htm", ".html" };

            // Remove doc fragment if necessary.
            int index = linkUrl.LastIndexOf('#');
            if (index != -1)
            {
                linkUrl = linkUrl.Substring(0, index);
            }

            return Array.Exists(staticPageExtensions, extension => linkUrl.EndsWith(extension));
        }
    }
}