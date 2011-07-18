using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munger
{
    // A
    class HostSet : HashSet<string>
    {
        public new bool Add(string item)
        {
            // Force hostname to be lowercase so we don't have
            // to worry about case-sensitivity.
            if (!string.IsNullOrEmpty(item))
                item = item.ToLowerInvariant();

            return base.Add(item);
        }

        public new bool Contains(string item)
        {
            // Force hostname to be lowercase so we don't have
            // to worry about case-sensitivity.
            if (!string.IsNullOrEmpty(item))
                item = item.ToLowerInvariant();

            return base.Contains(item);
        }

        /// <summary>
        /// Tests whether a URL really is a URL and whether the host
        /// portion matches any of the hosts in the list.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool HostPortionMatches(string url)
        {
            bool result = false;

            if (url.StartsWith("http://") || url.StartsWith("https://"))
            {
                // trim the protocol.
                string host = url.Substring(url.IndexOf("//") + 2);

                // trim the query string, if present.
                if (host.Contains('/'))
                    host = host.Substring(0, host.IndexOf('/'));

                result = this.Contains(host);
            }


            return result;
        }
    }
}
