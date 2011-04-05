using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Util;
using NCI.Web.UI.WebControls;
using NCI.Web.UI.WebControls.JSLibraries;

namespace NCI.Web.UI.WebControls.FormControls
{
    /// <summary>
    /// This custom Control combines a textbox with the autocomplete javascript
    /// which provides the convenience of not having to manually create the link
    /// between the textbox field and the javascript object. The user can set properties
    /// that will be passed through to either the textbox field or the javascript object.
    /// </summary>
    [ToolboxData("<{0}:AutoComplete runat=server></{0}:AutoComplete>"),
    DefaultEvent("TextChanged"), ParseChildren(true, "Text")]
    public class AutoComplete : WebControl, IPostBackDataHandler
    {
        private static readonly object TextChangedEvent = new object();

        #region Enumerations

        /// <summary>
        /// An enumeration which indicates which transfer method to use when
        /// making the html request.
        /// Values are: Get and Post
        /// </summary>
        public enum URLMethodEnum
        {
            GET,
            POST
        };

        /// <summary>
        /// An enumeration which indicates how data is queried on the database.
        /// Values are: BeginsWith and Contains
        /// </summary>
        public enum SearchCriteriaEnum
        {
            BeginsWith,
            Contains
        }

        #endregion

        #region Properties - AutoComplete

        /// <summary>
        /// Overrides the base tagkey. Always returns the Input tag.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Input;
            }
        }

        /// <summary>
        /// Property that sets the way data is queried at the databsae.
        /// Default value is BeginsWith
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Determines whether the search should start from the beginnign or anywhere in the string"),
        DefaultValue(SearchCriteriaEnum.BeginsWith),
        Localizable(true)]
        public SearchCriteriaEnum SearchCriteria
        {
            get
            {
                return ((ViewState["SearchCriteria"] == null) ?
                    SearchCriteriaEnum.BeginsWith : (SearchCriteriaEnum)ViewState["SearchCriteria"]);
            }

            set
            {
                ViewState["SearchCriteria"] = value;
            }
        }

        /// <summary>
        /// Property that defines the URL needed for the HTML request.
        /// Default value is '/AJAXService.svc/SearchJSCON/'.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Defines the URL needed for AJAX to get results"),
        DefaultValue("/AJAXService.svc/SearchJSON/"),
        Localizable(true)]
        public string SearchURL
        {
            get
            {
                String s = (String)ViewState["SearchURL"];
                return ((s == null) ? "/AJAXService.svc/SearchJSON/" : s);
            }

            set
            {
                ViewState["SearchURL"] = value;
            }
        }

        /// <summary>
        /// Proptry that sets the number of characters a user must type before the autocomplete
        /// control begins making AJAX calls. 
        /// Default value is 3.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The number of characters the user must type before searching begins"),
        DefaultValue(3),
        Localizable(true)]
        public int MinSearchChars
        {
            get
            {
                return ((ViewState["MinSearchChars"] == null) ? 3 : (int)ViewState["MinSearchChars"]);
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["MinSearchChars"] = value;
            }
        }

        /// <summary>
        /// Proprty that sets the minimum width of the listbox. If no value is supplied, 
        /// the textbox width is used.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The minimum width that the list box must be"),
        DefaultValue(0),
        Localizable(true)]
        public int MinWidth
        {
            get
            {
                return ((ViewState["MinWidth"] == null) ? 0 : (int)ViewState["MinWidth"]);
            }

            set
            {
                if (value < 0 || (MaxWidth > 0 && value > MaxWidth))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["MinWidth"] = value;
            }
        }

        /// <summary>
        /// Property that sets the maximum width that the list box can be.
        /// If no value is set, the width of the textbox is used.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The maximum width that the list box must be"),
        DefaultValue(0),
        Localizable(true)]
        public int MaxWidth
        {
            get
            {
                return ((ViewState["MaxWidth"] == null) ? 0 : (int)ViewState["MaxWidth"]);
            }

            set
            {
                if (value < 0 || (MinWidth > 0 && value < MinWidth))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["MaxWidth"] = value;
            }
        }

        /// <summary>
        /// Property that sets the URLMethod used when making the HTML Request
        /// Default value is GET.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Determines the url's method used for the search GET/POST"),
        DefaultValue("GET"),
        Localizable(true)]
        public URLMethodEnum URLMethod
        {
            get
            {
                return ((ViewState["URLMethod"] == null) ?
                    URLMethodEnum.GET : (URLMethodEnum)ViewState["URLMethod"]);
            }

            set
            {
                ViewState["URLMethod"] = value;
            }
        }

        /// <summary>
        /// Property that defines the delay used by the javascript autocomplete object
        /// before making the HTML Request. If no value is supplied there is no delay.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Defines the delay used before making the AJAX requuest"),
        DefaultValue(0),
        Localizable(true)]
        public int SearchDelay
        {
            get
            {
                return ((ViewState["SearchDelay"] == null) ? 0 : (int)ViewState["SearchDelay"]);
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                ViewState["SearchDelay"] = value;
            }
        }

        /// <summary>
        /// Property used to determine how many rows a user should see in the listbox
        /// at a time. If the number of rows set is less than the maximum rows returned,
        /// the user will be allowed to use the arrow keys to move up or down to view the
        /// remaining rows. If no value is set the maximum rows returned is displayed.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The number of rows to display to the user"),
        DefaultValue(0),
        Localizable(true)]
        public int RowsPerPage
        {
            get
            {
                return ((ViewState["RowsPerPage"] == null) ? 0 : (int)ViewState["RowsPerPage"]);
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["RowsPerPage"] = value;
            }
        }

        /// <summary>
        /// Property that indicates that caching should be used when making HTML requests
        /// Default value is false
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Determines if the control should cache the previous results"),
        DefaultValue(false),
        Localizable(true)]
        public bool CacheResults
        {
            get
            {
                return ((ViewState["CacheResults"] == null) ? false : (bool)ViewState["CacheResults"]);
            }

            set
            {
                ViewState["CacheResults"] = value;
            }
        }

        /// <summary>
        /// Property used to get or set the text in the textbox field.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The Text displayed on the textbox"),
        DefaultValue(""),
        Localizable(true)]
        public string Text
        {
            get
            {
                return (string)(ViewState["Text"] ?? string.Empty);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// Property used to indicate the maximum number of rows to return from the database.
        /// If no value is set, the entire list of rows is returned, if any, from the database.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The maximum number of rows to return from the search"),
        DefaultValue(10),
        Localizable(true)]
        public int MaxRowsToReturn
        {
            get
            {
                return ((ViewState["MaxRowsToReturn"] == null) ? 25 : (int)ViewState["MaxRowsToReturn"]);
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["MaxRowsToReturn"] = value;
            }
        }

        /// <summary>
        /// Property used to indicate the function used to display an AJAX error.
        /// if no function is provided the control will display an alert message.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The function called to display any AJAX error"),
        DefaultValue(null),
        Localizable(true)]
        public string AjaxErrorFunc
        {
            get
            {
                String s = (String)ViewState["AjaxErrorFunc"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["AjaxErrorFunc"] = value;
            }
        }

        /// <summary>
        /// Property to set the text to display on the listbox for the close link.
        /// Used primarily when a language other than English is used.
        /// Default Value is 'Close'.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The text to display for the close link"),
        DefaultValue("Close"),
        Localizable(true)]
        public string CloseLinkText
        {
            get
            {
                String s = (String)ViewState["CloseLinkText"];
                return ((s == null) ? string.Empty : s);
            }

            set
            {
                ViewState["CloseLinkText"] = value;
            }
        }

        /// <summary>
        /// Property used to indicate if the listbox should be a simple box outline or if
        /// it should be decorated with a graphic arrow pointing to the textbox.
        /// Default value is true (box outline).
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Determines whether the list should display as a box or filled with an arrow"),
        DefaultValue(true),
        Localizable(true)]
        public bool ShowBoxOutline
        {
            get
            {
                return ((ViewState["ShowBoxOutline"] == null) ? true : (bool)ViewState["ShowBoxOutline"]);
            }

            set
            {
                ViewState["ShowBoxOutline"] = value;
            }
        }

        /// <summary>
        /// Property used to indicate if the spinning circle icon should be used during
        /// the HTML request. The icon appears inside the textbox.
        /// Default value is false.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Determines whether the hour glass and spinning cirle icons are used during searches"),
        DefaultValue(false),
        Localizable(true)]
        public bool UseNotifierIcon
        {
            get
            {
                return ((ViewState["UseNotifierIcon"] == null) ? false : (bool)ViewState["UseNotifierIcon"]);
            }

            set
            {
                ViewState["UseNotifierIcon"] = value;
            }
        }

        /// <summary>
        /// Property used to set the function that will be triggered when a row is
        /// selected from the listbox.
        /// Default value is null.
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("The callback function used when the user selects a row"),
        DefaultValue(null),
        Localizable(true)]
        public string CallbackFunc
        {
            get
            {
                String s = (String)ViewState["CallbackFunc"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["CallbackFunc"] = value;
            }
        }

        /// <summary>
        /// Property used to define the stylesheet used for the auto complete list
        /// </summary>
        [Bindable(true),
        Category("AutoComplete"),
        Description("Css class name applied to the auto complete list"),
        DefaultValue("autocomplete"),
        Localizable(true)]
        public string ACCssClass
        {
            get
            {
                String s = (String)ViewState["ACCssClass"];
                return ((s == null) ? "autocomplete" : s);
            }

            set
            {
                ViewState["ACCssClass"] = value;
            }
        }

        /// <summary>
        /// Property used to get or set the text in the textbox field.
        /// </summary>
        [Browsable(false)]
        public string Value
        {
            get
            {
                return (string)(ViewState["Text"] ?? string.Empty);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        /// <summary>
        /// Property used to indicate the number of characters that may be entered
        /// in the textbox.
        /// </summary>
        [Bindable(true),
        Category("Behavior"),
        Description("The maximum length, in character, for the textbox"),
        DefaultValue(0),
        Localizable(true)]
        public int MaxLength
        {
            get
            {
                return (int)(ViewState["MaxLength"] ?? 0);
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["MaxLength"] = value;
            }
        }

        /// <summary>
        /// If True, browser is IE.
        /// </summary>
        [Bindable(true),
        Category("Behavior"),
        Description("If True, browser is IE"),
        DefaultValue(0),
        Localizable(true)]
        public bool IsIE
        {
            get
            {
                return (bool)(ViewState["IsIE"] ?? false);
            }

            set
            {
                ViewState["IsIE"] = value;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This method allows users to add handlers to the textChanged event
        /// </summary>
        public event EventHandler TextChanged
        {
            add { Events.AddHandler(TextChangedEvent, value); }
            remove { Events.RemoveHandler(TextChangedEvent, value); }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// This method is overriden so that LiteralControls will not be
        /// added to the ControlCollection.
        /// </summary>
        /// <param name="obj"></param>
        protected override void AddParsedSubObject(object obj)
        {
            if (!(obj is LiteralControl))
            {
                throw new HttpException("This control cannot contain controls");
            }
            this.Text = ((LiteralControl)obj).Text;
        }

        /// <summary>
        /// The method gets notified from the server control and performs any necessary
        /// pre-rendering steps prior to saving view state and rendering content
        /// </summary>
        /// <param name="e">Contains the event data</param>
        protected override void OnPreRender(EventArgs e)
        {
            // Order is important.
            // The control's JavaScript component relies on Prototype being present first.
            PrototypeManager.Load(this.Page);
            JSManager.AddResource(this.Page, typeof(AutoComplete), "NCI.Web.UI.WebControls.FormControls.Resources.AutoComplete.js");
            CssManager.AddResource(this.Page, typeof(AutoComplete), "NCI.Web.UI.WebControls.FormControls.Resources.AutoComplete.css");

            // Register this control to require postback handling when the page
            // is posted back to the server
            Page.RegisterRequiresPostBack(this);
        }


        #endregion

        #region Rendering

        /// <summary>
        /// Method used to render the input tag
        /// </summary>
        /// <param name="writer">The server control output stream</param>
        protected override void Render(HtmlTextWriter writer)
        {
            RenderBeginTag(writer);
            RenderEndTag(writer);
        }

        /// <summary>
        /// Method to render attributes to the begin tag
        /// </summary>
        /// <param name="writer">The server control output stream</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.AutoComplete, "off");
            if (this.MaxLength > 0)
                writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, this.MaxLength.ToString());
            if (!String.IsNullOrEmpty(this.Text))
                writer.AddAttribute(HtmlTextWriterAttribute.Value, this.Text);
        }

        /// <summary>
        /// Method to render the end tag and build the javascript code
        /// </summary>
        /// <param name="writer">The server control output stream</param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            BuildJavascript(writer);
        }

        /// <summary>
        /// This method builds the javascript which instantiates the 
        /// autocomplete class and sets the defining options
        /// </summary>
        /// <param name="output">The server control output stream</param>
        protected virtual void BuildJavascript(HtmlTextWriter output)
        {
            // Now we add in our javascript
            output.AddAttribute("language", "javascript");
            output.RenderBeginTag(HtmlTextWriterTag.Script);

            output.Write(string.Format(
                  "var options = {{isIE:{0},\nscript:'{1}'",
                  IsIE.ToString().ToLower(), SearchURL));

            if (MinSearchChars > 0)
            {
                output.Write(string.Format(",\nminchars:{0}", MinSearchChars));
            }
            if (ACCssClass.Length > 0)
            {
                output.Write(string.Format(",\nclassName:'{0}'", ACCssClass));
            }
            if (RowsPerPage > 0)
            {
                output.Write(string.Format(",\nrowsPerPage:{0}", RowsPerPage));
            }
            if (MaxRowsToReturn > 0)
            {
                output.Write(string.Format(",\nmaxresults:{0}", MaxRowsToReturn));
            }
            if (CloseLinkText.Length > 0)
            {
                output.Write(string.Format(",\ncloseText:'{0}'", CloseLinkText));
            }
            output.Write(string.Format(",\nuseNotifier:{0}", UseNotifierIcon.ToString().ToLower()));
            if (this.CallbackFunc.Length > 0)
            {
                output.Write(string.Format(",\ncallback:{0}", CallbackFunc));
            }
            output.Write(string.Format(",\nmeth:'{0}'", URLMethod.ToString()));
            if (SearchDelay > 0)
            {
                output.Write(string.Format(",\ndelay:{0}", SearchDelay));
            }
            if (MinWidth > 0 || MaxWidth > 0)
            {
                output.Write(",\nsetWidth:true");
                if (MinWidth > 0)
                {
                    output.Write(string.Format(",\nminWidth:{0}", MinWidth));
                }
                if (MaxWidth > 0)
                {
                    output.Write(string.Format(",\nmaxWidth:{0}", MaxWidth));
                }
            }
            output.Write(string.Format(",\ncache:{0}", this.CacheResults.ToString().ToLower()));
            output.Write(string.Format(",\ncontains:{0}",
                (SearchCriteria == SearchCriteriaEnum.Contains ? "true" : "false")));
            if (this.AjaxErrorFunc.Length > 0)
            {
                output.Write(string.Format(",\nOnAjaxError:{0}", AjaxErrorFunc));
            }
            output.Write(string.Format(",\nboxOutline:{0}", ShowBoxOutline.ToString().ToLower()));
            output.Write("};\n");

            // Create our variable
            output.Write(
                //string.Format("var {0}_json=new AutoComplete('{0}',options);\n", ClientID));
                string.Format("AutoComplete.extendTextBox('{0}',options);\n", ClientID));
            // End script
            output.RenderEndTag();
        }


        /// <summary>
        /// Event handler for the TextChangedEvent. Calls the delegate set by the user.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[TextChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }

        }


        #endregion

        #region IPostBackDataHandler Members

        /// <summary>
        /// implementation which automatically loads postback data
        /// </summary>
        /// <param name="postDataKey"></param>
        /// <param name="postCollection"></param>
        /// <returns></returns>
        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string presentValue = Text;
            string postedValue = postCollection[postDataKey];
            if (!presentValue.Equals(postedValue))
            {
                Text = postedValue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// This method gets Signaled when the server control object is
        /// notified that the state of the control has changed. In otherwords,
        /// when the text changes
        /// </summary>
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            OnTextChanged(EventArgs.Empty);
        }

        #endregion
    }
}
