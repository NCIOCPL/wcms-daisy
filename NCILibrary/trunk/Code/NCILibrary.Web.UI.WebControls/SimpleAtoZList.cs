using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SimpleAtoZList runat=server></{0}:SimpleAtoZList>")]
    public class SimpleAtoZList : WebControl
    {
        //Defining the letters so I can loop through them.  Plus I do not know if looping
        //from 65-91 and casting the ints to chars will work with the proper encoding.
        //'*' will be replaced with "All" when ShowAll is set true.
        private char[] _letters = new char[] {
            '*', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
            'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public const char AllCharacters = '*';

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue('\0')]
        [Localizable(true)]
        public char SelectedCharacter
        {
            get
            {
                object s = ViewState["SelectedCharacter"];
                return ((s == null) ? '\0' : (char)s);
            }
            set
            {
                ViewState["SelectedCharacter"] = char.ToUpper(value);
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("SelectedChar")]
        [Localizable(true)]
        public string SelectedQueryParamName
        {
            get
            {
                string s = (string)ViewState["SelectedQueryParamName"];
                return ((s == null) ? "SelectedChar" : s);
            }
            set
            {
                ViewState["SelectedQueryParamName"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Url
        {
            get
            {
                string s = (string)ViewState["Url"];
                return ((s == null) ? string.Empty : s);
            }
            set
            {
                ViewState["Url"] = value;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool ShowAll
        {
            get
            {
                bool? b = (bool?)ViewState["ShowAll"];
                return ((b == null) ? false : b.Value);
            }
            set
            {
                ViewState["ShowAll"] = value;
            }
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            bool isFirst = true;

            string outputChar;

            foreach (char c in _letters)
            {
                // Special Handling to allow '*' to be replaced with "All".
                if (c == AllCharacters && !ShowAll)
                    continue;
                else if (c == AllCharacters)
                    outputChar = "All";
                else
                    outputChar = c.ToString();


                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    output.Write("|");
                }

                if (c == SelectedCharacter && c != AllCharacters)
                {
                    output.RenderBeginTag(HtmlTextWriterTag.Span);
                    output.Write(" ");
                    output.Write(outputChar);
                    output.Write(" ");
                    output.RenderEndTag();
                }
                else
                {
                    output.AddAttribute(HtmlTextWriterAttribute.Href, GetUrl(c));
                    output.RenderBeginTag(HtmlTextWriterTag.A);
                    output.Write(" ");
                    output.Write(outputChar);
                    output.Write(" ");
                    output.RenderEndTag();
                }
            }
        }

        private string GetUrl(char c)
        {
            string result;

            if (c != AllCharacters)
            {
                string queryParam = "&";

                if (Url.IndexOf("?") == -1)
                    queryParam = "?";

                result = Url + queryParam + SelectedQueryParamName + "=" + c;
            }
            else
                result = Url;

            return result;
        }
    }
}
