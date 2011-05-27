using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

using Munger.Configuration;

namespace Munger
{
    /// <summary>
    /// Subsitutes links which have been discontinued, are known to be invalid, are
    /// being re-located, or are otherwise need to be replaced with their new values.
    /// </summary>
    class LinkSubstituter
    {
        Dictionary<string, string> _replacementMap;

        public LinkSubstituter()
        {
            if (_replacementMap == null)
            {
                _replacementMap = new Dictionary<string, string>();

                MungerConfiguration config = (MungerConfiguration)ConfigurationManager.GetSection("MungerConfig");
                foreach (RewritingElement item in config.SubstitutionList)
                {
                    _replacementMap.Add(item.OldPath, item.NewPath);
                }
            }
        }

        /// <summary>
        /// Replaces a URL with one from the list of substitutes.
        /// If no replacement exists, the original URL is returned instead.
        /// </summary>
        /// <param name="originalUrl">URL to replace</param>
        /// <returns>A replacement URL.</returns>
        public string MakeSubstitution(string originalUrl)
        {
            string replacement = originalUrl;

            // All keys are lowercase.
            string key = originalUrl.ToLower();

            if (_replacementMap.ContainsKey(key))
            {
                replacement = _replacementMap[key];
            }
            else if (originalUrl.StartsWith("/directorscorner/")
                || originalUrl.StartsWith("/aboutnci/directorscorner"))
            {
                replacement="/aboutnci/director";
            }

            return replacement;
        }
    }
}
