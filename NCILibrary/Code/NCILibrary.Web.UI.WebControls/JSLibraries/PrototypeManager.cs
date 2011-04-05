using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NCI.Web.UI.WebControls.JSLibraries
{
    /// <summary>
    /// Manages the Prototype javascript library on a page.
    /// </summary>
    public static class PrototypeManager
    {
        public const string DefaultVersion = "1.6.0";
        private static object _prototypeIsLoaded = new object(); 
        private static readonly string[] _validVersions = new string[]{"1.5.1", "1.6.0"};

        /// <summary>
        /// Loads the library on a page.  If the library has already been loaded
        /// then we will not load it again, if a different version of the library
        /// has been loaded than the one we are loading we must throw an exception.
        /// </summary>
        /// <param name="p">The page to load the library</param>
        public static void Load(Page p)
        {
            Load(p, DefaultVersion);
        }

        /// <summary>
        /// Loads the library on a page.  If the library has already been loaded
        /// then we will not load it again, if a different version of the library
        /// has been loaded than the one we are loading we must throw an exception.
        /// </summary>
        /// <param name="p">The page to load the library</param>
        /// <param name="version">The version of the library to load.  If you do
        /// not know what libraries are available you should probably just use the
        /// default.
        /// </param>
        public static void Load(Page p, string version)
        {
            //Check arguments
            if (p == null)
                throw new ArgumentNullException("The page passed in is null");

            if (version == null)
                throw new ArgumentNullException("The version passed in is null");

            if (p.Header == null)
                throw new NullReferenceException("The head html element for the page requires the runat=server property.");

            //Check if the version is a know and valid version
            bool isVersionValid = false;

            foreach (string ver in _validVersions)
            {
                if (version.CompareTo(ver) == 0)
                {
                    isVersionValid = true;
                    break; //It is a valid version
                }
            }

            //If the version is not a valid version we cannot continue
            if (!isVersionValid)
            {
                throw new JSLibraryLoadException(GetInvalidVersionMessage(version));
            }

            string loadedVersion = HttpContext.Current.Items[_prototypeIsLoaded] as string;

            if (loadedVersion == null)
            {
                //Since nothing else is loaded, load the library
                //This Javascript is special, and we need to actually add it to the head element.
                HtmlGenericControl scriptControl = new HtmlGenericControl("script");
                p.Header.Controls.Add(scriptControl);
                scriptControl.Attributes.Add("type", "text/javascript");
                scriptControl.Attributes.Add("src",
                    p.ClientScript.GetWebResourceUrl(typeof(PrototypeManager),
                        "NCI.Web.UI.WebControls.JSLibraries.Resources.Prototype.v" + version.Replace('.', '_') + ".prototype.js"));
                HttpContext.Current.Items.Add(_prototypeIsLoaded, version);

            }
            else if (loadedVersion.CompareTo(version) != 0)
            {
                //Another version of the library is loaded, so we cannot load this
                //without many other things breaking.
                throw new JSLibraryLoadException(
                    String.Format(
                        "Attempted to load Prototype library version {0}, but version {1} is already loaded.",
                        version,
                        loadedVersion
                        )
                    );
            }
        }

        /// <summary>
        /// Gets the error message for an version that is invalid
        /// </summary>
        /// <param name="version">The version that is invalid</param>
        /// <returns>The error message</returns>
        private static string GetInvalidVersionMessage(string version)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Version: ");
            sb.Append(version);
            sb.Append(" is an invalid version for the Prototype library. Valid versions are: ");
            bool isFirst = true;

            foreach (string ver in _validVersions)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(", ");

                sb.Append("\"");
                sb.Append(ver);
                sb.Append("\"");
            }

            sb.Append(".");

            return sb.ToString();
        }

    }
}
