namespace Munger
{
    /// <summary>
    /// Interface to an object which rewrites the URLs contained in a document.
    /// </summary>
    public interface IUrlMunger
    {
        string RewriteSingleUrl(string url, out string messages);
        string RewriteUrls(string docBody, string pageUrl, out string messages);
    }
}