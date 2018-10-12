using AngleSharp.Dom;
using System.Collections.Generic;
using System.Xml;

namespace Munger
{
    public interface ILinkMunger
    {
        void RewriteLinkReferences(IDocument document, string pageUrl, List<string> messages);
        string RewriteSingleUrl(string link, List<string> errors);
    }
}