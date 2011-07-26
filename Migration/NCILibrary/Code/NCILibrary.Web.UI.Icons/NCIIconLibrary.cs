using System;
using System.Collections.Generic;
using System.Text;

namespace NCI.Web.UI.Icons
{
    /// <summary>
    /// This is here as a stub so that we can do a
    /// GetWebResource(typeof(NCIIconLibrary), iconname)
    /// </summary>
    public static class NCIIconLibrary
    {
        /// <summary>
        /// Gets the web resource name for the file icon for an extension.
        /// </summary>
        /// <param name="ext">The extension with or without a period.</param>
        /// <returns></returns>
        public static string GetFileIconResNameForExt(string ext)
        {
            if (ext == null)
                throw new ArgumentNullException("The extension cannot be null");

            ext = ext.Trim();
            ext = ext.ToLower();

            if (ext == ".")
                throw new ArgumentException("The extension cannot be just a period.");
            else if (ext == string.Empty)
                throw new ArgumentException("The extension cannot be an empty string.");

            //Trim the period
            int perIndex = ext.IndexOf('.');
            if (perIndex > 0)
                throw new ArgumentNullException("An extension cannot contain a period at any position other than 0.");
            else if (perIndex == 0)
                ext = ext.Substring(1);

            
            //Figure out the extension
            switch (ext)
            {
                //Excel
                case "csv" : //Comma separated values file
                case "xls" : //Excel 97-2003 file
                case "xlsx" : //Excel 2007+ file
                case "xlt" : //Excel 97-2003 Template
                case "xltx" : //Excel 2007+ Template
                    return "NCI.Web.UI.Icons.Size16x16.FileIcons.page_white_excel.gif";

                //PowerPoint
                case "pot": //PowerPoint 97-2003 template
                case "potx": //PowerPoint 2007+ template
                case "pps": //PowerPoint 97-2003 slide show
                case "ppsx" : //Powerpoint 2007+ slide show
                case "ppt": //PowerPoint 97-2003 file
                case "pptx" : //PowerPoint 2007+ file
                    return "NCI.Web.UI.Icons.Size16x16.FileIcons.page_white_powerpoint.gif";

                //Word
                case "doc": //Word 97-2003 file
                case "dochtml": //Word 97-2003 Html file
                case "docx": //Word 2007+ file
                case "dot": //Word 97-2003 template
                case "dothtml": //Word 97-2003 HTML template
                case "dotx": //Word 2007+ template
                    return "NCI.Web.UI.Icons.Size16x16.FileIcons.page_white_word.gif";

                //Images
                case "gif": 
                case "tiff": 
                case "tif": 
                case "tga": 
                case "targa":
                case "png": 
                case "jpg": 
                case "jpeg":
                case "jpe": 
                case "dng": 
                case "bmp": 
                case "jp2": 
                case "wmf":
                    return "NCI.Web.UI.Icons.Size16x16.FileIcons.page_white_picture.gif";

                //Html and variants
                case "html":
                case "htm":
                case "mht":
                case "mhtml":
                case "shtml":
                    return "NCI.Web.UI.Icons.Size16x16.FileIcons.page_white_world.gif";

                case "txt" :
                case "text":
                    return "NCI.Web.UI.Icons.Size16x16.FileIcons.page_white_text.gif";

            }

            return "NCI.Web.UI.Icons.Size16x16.FileIcons.page_white.gif";
        }
    }
}
