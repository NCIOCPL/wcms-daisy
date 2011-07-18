using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{    
        /// <summary>
        /// Adds a URL filter which modifies the URL referenced by 'string' when GetURL is called.
        /// </summary>
        /// <param name="fieldFilterData"></param>
    public delegate void UrlFilterDelegate(string name, NciUrl url);

}
