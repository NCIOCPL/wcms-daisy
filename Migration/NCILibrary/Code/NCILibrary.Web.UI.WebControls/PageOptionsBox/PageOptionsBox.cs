using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using NCI.Web.UI.WebControls.Configuration;

namespace NCI.Web.UI.WebControls
{
    [DefaultProperty("BoxTitle")]
    [ToolboxData("<{0}:PageOptionsBox runat=server></{0}:PageOptionsBox>")]
    public class PageOptionsBox : WebControl
    {
        PageOptionsCollection _optionCollection;

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.
        /// </returns>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string BoxTitle
        {
            get
            {
                String s = (String)ViewState["BoxTitle"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["BoxTitle"] = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty), MergableProperty(false), DefaultValue((string)null), Description("DataControls_Columns"), Category("Default")]
        public virtual PageOptionsCollection PageOptions
        {
            get
            {
                if (this._optionCollection == null)
                {
                    this._optionCollection = new PageOptionsCollection();
                    //this._optionCollection.OptionsChanged += new EventHandler(this.OnFieldsChanged); //Don't need this now?
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager)this._optionCollection).TrackViewState();
                    }
                }
                return this._optionCollection;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //Don't render the box if there is nothing to render.
            if (PageOptions.Count > 0)
                base.Render(writer);
        }

        /// <summary>
        /// Renders the HTML opening tag of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {            
            base.RenderBeginTag(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.H4);
            writer.Write(this.BoxTitle);
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "po-options");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
        }

        /// <summary>
        /// Renders the contents of the control to the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            foreach (PageOption option in PageOptions)
            {
                if (option is LinkPageOption)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);

                    if (((LinkPageOption)option).OnClick != null)
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, ((LinkPageOption)option).OnClick);
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "po-option " + option.CssClass);
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, ((LinkPageOption)option).Href);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write(option.LinkText);
                    writer.RenderEndTag();

                    writer.RenderEndTag(); //Closing LI
                }
                else if (option is AddThisPageOption)
                {
                    writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "NCIAddThisLI");
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);

                    //This should be configurable.
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, "http://s7.addthis.com/js/200/addthis_widget.js");
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                    writer.RenderBeginTag(HtmlTextWriterTag.Script);
                    writer.RenderEndTag();

                    //Set the title for the page
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                    writer.RenderBeginTag(HtmlTextWriterTag.Script);
                    writer.Write("var addthis_share = {");
                    writer.Write("title: \"" + CleanTitle(((AddThisPageOption)option).PageTitle) + "\"");
                    writer.Write("}");
                    writer.RenderEndTag();

                    AddThisConfigElement elem = AddThisConfig.GetByCultureLanguage(CultureInfo.CurrentUICulture);
                    ((AddThisPageOption)option).Settings.CompactServicesList = elem.CompactServices;
                    ((AddThisPageOption)option).Settings.ExpandedServicesList = elem.ExpandedServices;

                    //Setup options
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                    writer.RenderBeginTag(HtmlTextWriterTag.Script);
                    if (
                        (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.CompactServicesList)) ||
                        (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.ExpandedServicesList)) ||
                        (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.Language)) ||
                        (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.UserName))
                        )
                    {
                        bool hasMoreThanOneOption = false;
                        writer.Write("var addthis_config = {");

                        if (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.UserName)) {
                            if (hasMoreThanOneOption)
                                writer.Write(", ");
                            else
                                hasMoreThanOneOption = true;
                            writer.Write("username: '");
                            writer.Write(((AddThisPageOption)option).Settings.UserName);
                            writer.Write("'");
                        }

                        //Language should either not be set for english or es for spanish
                        if (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.Language)) {
                            if (hasMoreThanOneOption)
                                writer.Write(", ");
                            else
                                hasMoreThanOneOption = true;
                            writer.Write("ui_language: '");
                            writer.Write(((AddThisPageOption)option).Settings.Language);
                            writer.Write("'");
                        }

                        if (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.CompactServicesList)) {
                            if (hasMoreThanOneOption)
                                writer.Write(", ");
                            else
                                hasMoreThanOneOption = true;
                            writer.Write("services_compact: '");
                            writer.Write(((AddThisPageOption)option).Settings.CompactServicesList);
                            writer.Write("'");
                        }

                        if (!string.IsNullOrEmpty(((AddThisPageOption)option).Settings.ExpandedServicesList))
                        {
                            if (hasMoreThanOneOption)
                                writer.Write(", ");
                            else
                                hasMoreThanOneOption = true;
                            writer.Write("services_expanded: '");
                            writer.Write(((AddThisPageOption)option).Settings.ExpandedServicesList);
                            writer.Write("'");
                        }

                        writer.Write("}");
                    }
                    writer.RenderEndTag();

                    //Setup the OnDomReady event handler to wire up the JS events for add this
                    writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
                    writer.RenderBeginTag(HtmlTextWriterTag.Script);

                    string webAnalytics = String.Empty;
                    if (((AddThisPageOption)option).OnClick != String.Empty)
                        webAnalytics = ((AddThisPageOption)option).OnClick + ",";
                    writer.Write(@"
                        jQuery(document).ready(function($) {
                            $('#NCIAddThisLI').show();
                            $('#NCIAddThisContainer a').click(function() { return false; });
                            $('#NCIAddThisContainer').mouseout(function() { addthis_close(); });
                            $('#NCIAddThisContainer').click(function() { return " + webAnalytics + "addthis_open(this, '', ''); });});");

                    writer.RenderEndTag();

                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "NCIAddThisContainer");
                    writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    writer.Write(option.LinkText);
                    writer.RenderEndTag();

                    writer.RenderEndTag(); //Closing LI
                }
            }
        }

        /// <summary>
        /// Renders the HTML closing tag of the control into the specified writer. This method is used primarily by control developers.
        /// </summary>
        /// <param name="writer">A <see cref="T:System.Web.UI.HtmlTextWriter"/> that represents the output stream to render HTML content on the client.</param>
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            base.RenderEndTag(writer);
        }

        private string CleanTitle(string title)
        {
            //A lot of content uses &#153; for the trademark symbol.  This is not actually valid, it is
            //old and outdated.  This causes add this and such to break.  We need to use &#8482; instead.
            //this should be fixed at a global level, but that is OOS for this project.
            return HttpUtility.HtmlDecode(title.Replace("&#153;", "&#8482;")).Replace("\"", "");
        }

        /// <summary>
        /// Saves any state that was modified after the <see cref="M:System.Web.UI.WebControls.Style.TrackViewState"/> method was invoked.
        /// </summary>
        /// <returns>
        /// An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.
        /// </returns>
        protected override object SaveViewState()
        {
            object obj1 = base.SaveViewState();
            object obj2 = ((IStateManager)_optionCollection).SaveViewState();

            return new object[] { obj1, obj2 };
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            if (_optionCollection != null)
            {
                ((IStateManager)_optionCollection).TrackViewState();
            }
        }

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
        /// </summary>
        /// <param name="savedState">An object that represents the control state to restore.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;

                base.LoadViewState(objArray[0]);

                if (objArray[1] != null)
                {
                    ((IStateManager)_optionCollection).LoadViewState(objArray[1]);
                }
            }
            else
            {
                base.LoadViewState(null);
            }
        }
    }
}
