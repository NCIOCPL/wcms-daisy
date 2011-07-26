using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NCI.Web.UI.WebControls.JSLibraries
{
    /// <summary>
    /// 
    /// </summary>
    public static class ScriptaculousManager
    {
        public const string DefaultVersion = "1.8.0";
        private static object _scriptIsLoaded = new object();
        //This is not only for the valid version,
        //but what version of prototype that the Scriptaculous version needs
        private static readonly Dictionary<string, string> _validVersions = new Dictionary<string,string>();

        static ScriptaculousManager()
        {
            _validVersions.Add("1.7.1", "1.5.1");
            _validVersions.Add("1.8.0", "1.6.0");
        }

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
            if (!_validVersions.ContainsKey(version))
                throw new JSLibraryLoadException(GetInvalidVersionMessage(version));

            //Get the loaded version
            string loadedVersion = HttpContext.Current.Items[_scriptIsLoaded] as string;

            if (loadedVersion == null)
            {
                //Since nothing else is loaded, load the library

                //First we need to try and load the Prototype library
                try
                {
                    PrototypeManager.Load(p, _validVersions[version]);
                }
                catch (Exception ex)
                {
                    throw new JSLibraryLoadException(
                        String.Format(
                            "Could not load the prototype library, v{0} for scriptaculous v{1}.",
                            _validVersions[version],
                            version
                        ), ex);
                }

                //This Javascript is special, and we need to actually add it to the head element.
                HtmlGenericControl scriptControl = new HtmlGenericControl("script");
                p.Header.Controls.Add(scriptControl);
                scriptControl.Attributes.Add("type", "text/javascript");
                scriptControl.Attributes.Add("src",
                    p.ClientScript.GetWebResourceUrl(typeof(ScriptaculousManager),
                        "NCI.Web.UI.WebControls.JSLibraries.Resources.Scriptaculous.v" + version.Replace('.', '_') + ".scriptaculous.js"));
                HttpContext.Current.Items.Add(_scriptIsLoaded, version);

            }
            else if (loadedVersion.CompareTo(version) != 0)
            {
                //Another version of the library is loaded, so we cannot load this
                //without many other things breaking.
                throw new JSLibraryLoadException(
                    String.Format(
                        "Attempted to load Scriptaculous library version {0}, but version {1} is already loaded.",
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
            sb.Append(" is an invalid version for the Scriptaculous library. Valid versions are: ");
            bool isFirst = true;

            foreach (string ver in _validVersions.Keys)
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
