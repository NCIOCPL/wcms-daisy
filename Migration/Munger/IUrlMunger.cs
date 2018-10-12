using AngleSharp.Dom;

namespace Munger
{
    /// <summary>
    /// Interface to an object which rewrites the URLs contained in a document.
    /// </summary>
    public interface IUrlMunger
    {
        string RewriteSingleUrl(string url, out string messages);
        void RewriteUrls(IDocument document, string pageUrl, out string messages);
    }
}