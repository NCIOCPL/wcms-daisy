using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Web.UI.WebControls.JSLibraries
{
    public class JSLibraryLoadException : System.Exception
    {
        public JSLibraryLoadException(string message) : base(message) { }
        public JSLibraryLoadException(string message, Exception innerException) : base(message, innerException) { }
    }

}
